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
        private bool activeMatch;
        private bool activeGame;
        private GameMap map;
        private string lastWinner;
        private ICollection<Score> scores;

        public GameMap Map {
            get {
                return map;
            }

            private set {
                map = value;
            }
        }
        public bool ActiveMatch {
            get {
                return activeMatch;
            }

            private set {
                activeMatch = value;
            }
        } //ActiveMatch means players are playing
        public bool ActiveGame {
            get {
                return activeGame;
            }
            set {
                activeGame = value;
            }
        }   //ActiveGame means players are in the Map
        public string LastWinner {
            get {
                return lastWinner;
            }

            private set {
                lastWinner = value;
            }
        }
        public Action EndMatchEvent { get; set; }
        public Action<string> Notify { get; set; }
        public Action<ICollection<Score>> AddScoresEvent { get; set; }
        
        public readonly object gameAccess;

        public Game(int preMatchMiliseconds, int matchMiliseconds)
        {
            map = new GameMap(8, 8);
            matchTimer = new Timer(matchMiliseconds);
            matchTimer.Elapsed += TimeOut;
            preMatchTimer = new Timer(preMatchMiliseconds);
            preMatchTimer.Elapsed += PreMatchTimeOut;
            activeMatch = false;
            activeGame = true;
            lastWinner = "None";
            map.PlayerRemovedEvent += CheckEndMatch;
            EndMatchEvent += () => { }; //Do nothing
            Notify += (s) => { }; //Do nothing
            scores = new List<Score>();
            gameAccess = new object();
        }

        private void RestartMap()
        {
            Map = new GameMap(8,8);
            Map.PlayerRemovedEvent += CheckEndMatch;
        }

        public void StartPreMatchTimer()
        {
            activeGame = true;
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
                activeMatch = false;
                activeGame = false;
                EndMatchEvent();
                Notify("MATCH ENDED --> " + LastWinner);
                RestartMap();
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
                    activeMatch = true;
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
                    winner = Map.GetPlayers().First().Name + " won the match. Congrats!";
                else if (Map.MonsterCount > 1)
                    winner = "Its a tie between monsters";
            }
            else
            {
                winner = "The survivors won the match. Congrats!";
            }

            return winner;
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
            return map.GetPlayers();
        }
    }
}
