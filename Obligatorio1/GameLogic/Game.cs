using GameLogicException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GameLogic
{
    public class Game
    {
        private Timer matchTimer;
        private Timer preMatchTimer;
        private string lastWinner;
        private ICollection<Score> scores;
        public GameReport PlayerRegistries { get; private set; }
        private ICollection<string> logs;


        public GameMap Map { get;set;}
        public bool ActiveMatch {get;set;} //ActiveMatch means players are playing
        public bool ActiveGame {get;set;}   //ActiveGame means players are in the Map
        public string LastWinner {get;set;}
        public Action EndMatchEvent { get; set; }
        public Action<string> Notify { get; set; }
        
        public readonly object gameAccess;

        public Game(int preMatchMiliseconds, int matchMiliseconds)
        {
            Map = new GameMap(8, 8);
            matchTimer = new Timer(matchMiliseconds);
            matchTimer.Elapsed += TimeOut;
            preMatchTimer = new Timer(preMatchMiliseconds);
            preMatchTimer.Elapsed += PreMatchTimeOut;
            ActiveMatch = false;
            ActiveGame = true;
            lastWinner = "None";
            Map.PlayerRemovedEvent += CheckEndMatch;
            Map.SendRemovedEvent += RegisterDeadPlayer;
            EndMatchEvent += () => { }; //Do nothing
            Notify += (s) => { }; //Do nothing
            gameAccess = new object();
            RestartGameInfoRegistries();
        }

     
        private void RestartGame()
        {
            RestartGameInfoRegistries();
            RestartMap();
        }

        private void RestartGameInfoRegistries()
        {
            scores = new List<Score>();
            PlayerRegistries = new GameReport();
            logs = new List<string>();
        }

        private void RestartMap()
        {
            Map = new GameMap(8, 8);
            Map.PlayerRemovedEvent += CheckEndMatch;
            Map.SendRemovedEvent += RegisterDeadPlayer;

        }

        public void StartPreMatchTimer()
        {
            ActiveGame = true;
            preMatchTimer.Start();
            Notify("Prematch timer started");
        }

        private void TimeOut(object sender, ElapsedEventArgs e)
        {
            Notify("Match time out");
            EndMatch();
        }
        private void EndMatch()
        {
            lock (gameAccess)
            {
                matchTimer.Stop();
                LastWinner = GetWinner();
                foreach (Player player in Map.GetPlayers())
                {
                    player.Notify("MATCH ENDED --> " + LastWinner);
                }
                string endMessage = "MATCH ENDED --> " + LastWinner;
                Notify(endMessage);
                LogAction(endMessage);
                ActiveMatch = false;
                ActiveGame = false;
                EndMatchEvent();
                RestartGame();
                StartPreMatchTimer();
            }          
        }

        public void PreMatchTimeOut(object sender, ElapsedEventArgs e)
        {
            StartMatch();
        }
        private void StartMatch()
        {
            lock (gameAccess)
            {
                preMatchTimer.Stop();
                if (ActiveGameConditions(Map.MonsterCount, Map.SurvivorCount))
                {
                    ActiveMatch = true;
                    foreach (Player player in Map.GetPlayers())
                    {
                        player.Notify("Match started!");
                        player.EnabledAttackAction = true;
                    }
                    matchTimer.Start();
                    Notify("Match started");
                }
                else
                {
                    Notify("Can't start match with actual players in map. Prematch timer restarted");
                    preMatchTimer.Start();
                }
            }          
        }

        public bool TooManyPlayers()
        {
            return Map.PlayerCount >= Map.PlayerCapacity;
        }
        public bool TooManyMonsters()
        {
            return false;
        }
        public bool TooManySurvivors()
        {
            return Map.SurvivorCount == Map.PlayerCapacity - 1;
        }

        public string GetWinner()
        {
            if (matchTimer.Enabled && ActiveGameConditions(Map.MonsterCount, Map.SurvivorCount))
                throw new UnfinishedMatchException();

            string winner = "";
            if (Map.SurvivorCount == 0)
            {
                if (Map.MonsterCount == 1)
                {
                    Player lastPlayer = Map.GetPlayers().First();
                    winner = lastPlayer.Name + " won the match. Congrats!";
                    lastPlayer.AddPoints(200);
                    AddReportRegistry(lastPlayer, true);
                }
                else if (Map.MonsterCount > 1)
                {
                    winner = "Its a tie between monsters";
                    foreach (Player player in Map.GetPlayers())
                    {
                        player.AddPoints(100);
                        AddReportRegistry(player, false);
                    }
                }
            }
            else
            {
                winner = "The survivors won the match. Congrats!";
                foreach (Player player in Map.GetPlayers())
                {
                    if(player.Role != Role.MONSTER)
                        player.AddPoints(300);
                    AddReportRegistry(player, player.Role == Role.SURVIVOR);
                }
            }

            return winner;
        }

        private void RegisterDeadPlayer(Player player)
        {
            AddReportRegistry(player, false);
        }

        private void AddReportRegistry(Player player, bool didWin)
        {
            lock (gameAccess)
            {
                PlayerReportField register = new PlayerReportField(player.Name, player.Role, didWin);
                PlayerRegistries.AddPlayerField(register);
            }
        }

        public bool ActiveGameConditions(int monsterCount, int survivorCount)
        {
            bool monsterVsMonster = monsterCount > 1;
            bool survivorVsMosnter = monsterCount >= 1 && survivorCount > 0;

            return monsterVsMonster || survivorVsMosnter;
        }
        public void CheckEndMatch()
        {
            if (matchTimer.Enabled && !ActiveGameConditions(Map.MonsterCount, Map.SurvivorCount))
                EndMatch();
        }

        public void AddPlayer(Player player)
        {
            lock (gameAccess)
            {
                if (TooManyPlayers())
                    throw new MapIsFullException("Map is full. Try next match.");
                if (ActiveMatch)
                    throw new MatchAlreadyStartedException("There is a match going on. Wait for next match.");
                if (Map.IsPlayerInMap(player))
                    throw new PlayerAlreadyInMatch("This player has already been taken, select other player");

                player.EnabledAttackAction = false;
                player.Join(this);
            }
        }

        public ICollection<Player> GetPlayers()
        {
            return Map.GetPlayers();
        }

        public void LogAction(string log)
        {
            logs.Add(log);
        }

        public ICollection<string> GetLogs()
        {
            return new List<string>(logs);
        }

        public void AddScore(Score score)
        {
            this.scores.Add(score);
        }

        public ICollection<Score> GetLastScores()
        {
            return new List<Score>(scores);
        }
    }
}
