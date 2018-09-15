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
        private int monsterCount;
        private int survivorCount;
        private Timer time; 
        bool activeMatch;
        private GameMap map;
        private Role lastWinner;

        public GameMap Map {
            get {
                return map;
            }

            private set {
                
            }
        }
        public bool ActiveMatch {
            get {
                return activeMatch;
            }

            private set {
                activeMatch = value;
            }
        }
        public int MonsterCount {
            get {
                return monsterCount;
            }

            set {
                monsterCount = value;
                if (!ActiveGameConditions())
                    EndMatch();
            }
        }
        public int SurvivorCount {
            get {
                return survivorCount;
            }

            set {
                survivorCount = value;
                if (!ActiveGameConditions())
                    EndMatch();
            }
        }
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
            time = new Timer(180000);
            time.Elapsed += TimeOut;
            monsterCount = 0;
            survivorCount = 0;
            activeMatch = false;
            lastWinner = Role.NEUTRAL;
        }

        public Game(GameMap map)
        {
            this.map = map;
            time = new Timer(180000);
            time.Elapsed += TimeOut;
            monsterCount = 0;
            survivorCount = 0;
            activeMatch = false;
            lastWinner = Role.NEUTRAL;
        }

        private void TimeOut(object sender, ElapsedEventArgs e)
        {
            EndMatch();
        }

        private void EndMatch()
        {
            LastWinner = GetWinner();
            activeMatch = false;
        }

        public void StartMatch()
        {
            time.Start();
            activeMatch = true;
            //Map.UnlockPlayersActions()
        }

        public Role GetWinner()
        {
            if (time.Enabled && ActiveGameConditions())
                throw new UnfinishedMatchException();

            Role winner = Role.SURVIVOR;
            if (SurvivorCount == 0)
            {
                if (MonsterCount == 1)
                    winner = Role.MONSTER;
                else if (MonsterCount > 1)
                    winner = Role.NEUTRAL;
            }

            return winner;
        }

        private bool ActiveGameConditions()
        {
            bool monsterVsMonster = MonsterCount > 1;
            bool survivorVsMosnter = MonsterCount >= 1 && SurvivorCount > 0;

            return monsterVsMonster || survivorVsMosnter;
        }

        public void AddPlayer(Player player)
        {
            //if (activeMatch)
              //  throw new MatchAlreadyStartedException();

            Position initialPosition = Map.GetEmptyPosition();
            Map.AddPlayerToPosition(player, initialPosition);
            player.Join(this, initialPosition);
            ActiveMatch = true;
        }
    }
}
