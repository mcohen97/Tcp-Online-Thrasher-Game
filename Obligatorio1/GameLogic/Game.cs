using GameLogicException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using GameLogicException;

namespace GameLogic
{
    public class Game
    {
        public static readonly int PREMATCH_MILLISECONDS = 5000;
        public static readonly int MATCH_MILLISECONDS = 180000;

        private Timer matchTimer;
        private Timer preMatchTimer;
        private bool activeMatch;
        private bool activeGame;
        private GameMap map;
        private Role lastWinner;
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
        public Role LastWinner {
            get {
                return lastWinner;
            }

            private set {
                lastWinner = value;
            }
        }

        public Game()
        {
            map = new GameMap(8, 8);
            matchTimer = new Timer(MATCH_MILLISECONDS);
            matchTimer.Elapsed += TimeOut;
            preMatchTimer = new Timer(PREMATCH_MILLISECONDS);
            preMatchTimer.Elapsed += PreMatchTimeOut;
            activeMatch = false;
            activeGame = true;
            lastWinner = Role.NEUTRAL;
            map.CheckGame += CheckEndMatch;
        }

        private void RestartMap()
        {
            Map = new GameMap(8,8);
            Map.CheckGame += CheckEndMatch;
        }

        public void StartPreMatchTimer()
        {
            activeGame = true;
            preMatchTimer.Start();
        }

        private void TimeOut(object sender, ElapsedEventArgs e)
        {
            EndMatch();
        }
        private void EndMatch()
        {
            matchTimer.Stop();
            LastWinner = GetWinner();
            foreach (Player player in Map.GetPlayers())
            {
                player.Notify("END MATCH - Winner is " + RoleMethods.RoleToString(LastWinner));
            }
            activeMatch = false;
            activeGame = false;
            RestartMap();
            StartPreMatchTimer();
        }

        public void PreMatchTimeOut(object sender, ElapsedEventArgs e)
        {
            StartMatch();
        }
        private void StartMatch()
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
            }
            else
                preMatchTimer.Start();
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

        public Role GetWinner()
        {
            if (matchTimer.Enabled && ActiveGameConditions(Map.MonsterCount, Map.SurvivorCount))
                throw new UnfinishedMatchException();

            Role winner = Role.SURVIVOR;
            if (Map.SurvivorCount == 0)
            {
                if (Map.MonsterCount == 1)
                    winner = Role.MONSTER;
                else if (Map.MonsterCount > 1)
                    winner = Role.NEUTRAL;
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
            if (TooManyPlayers())
                throw new MapIsFullException("Map is full. Try next match.");
            if (ActiveMatch)
                throw new MatchAlreadyStartedException("There is a match going on. Wait for next match.");

            player.EnabledAttackAction = false;
            player.Join(this);
            player.Notify("You are in the map. Your attack action will be unlocked when match starts. Stay alert!");
        }
    }
}
