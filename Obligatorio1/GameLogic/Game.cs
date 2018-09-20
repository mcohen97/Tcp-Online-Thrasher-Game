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
        private Queue<Player> preMatchMonsters;
        private Queue<Player> preMatchSurvivors;
        private Timer matchTimer;
        private Timer preMatchTimer;
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
            matchTimer = new Timer(180000);
            matchTimer.Elapsed += TimeOut;
            preMatchTimer = new Timer(60000);
            preMatchTimer.Elapsed += StartMatch;
            activeMatch = false;
            lastWinner = Role.NEUTRAL;
            preMatchMonsters = new Queue<Player>();
            preMatchSurvivors = new Queue<Player>();
            map.CheckGame += CheckEndMatch;
        }
        public Game(GameMap map)
        {
            this.map = map;
            matchTimer = new Timer(180000);
            matchTimer.Elapsed += TimeOut;
            preMatchTimer = new Timer(30000);
            preMatchTimer.Elapsed += StartMatch;
            activeMatch = false;
            lastWinner = Role.NEUTRAL;
            preMatchMonsters = new Queue<Player>();
            preMatchSurvivors = new Queue<Player>();
            map.CheckGame += CheckEndMatch;
        }

        public void StartPreMatchTimer()
        {
            preMatchTimer.Start();
        }

        private void TimeOut(object sender, ElapsedEventArgs e)
        {
            EndMatch();
        }
        private void EndMatch()
        {
            LastWinner = GetWinner();
            activeMatch = false;
            preMatchTimer.Start();
        }

        public void StartMatch(object sender, ElapsedEventArgs e)
        {
            if (preMatchMonsters.Count > 2)
            {
                preMatchTimer.Stop();
                StartMatch();
            }              
            else
                preMatchTimer.Start();
        }
        private void StartMatch()
        {
            Map = new GameMap(8, 8);
            activeMatch = true;
            while(!TooManyPlayers())
            {
                AddSurvivorToMap();
                AddSurvivorToMap();
                AddSurvivorToMap();
                AddMonsterToMap();
            }

            foreach (Player player in Map.GetPlayers())
            {
                player.Notify("Game started!!");
            }

            matchTimer.Start();
        }

        private void AddSurvivorToMap()
        {
            if(preMatchSurvivors.Any())
            {
                Position playerPosition = Map.GetEmptyPosition();
                Player survivor = preMatchSurvivors.Dequeue();
                Map.AddPlayerToPosition(survivor, playerPosition);
            }
        }
        private void AddMonsterToMap()
        {
            if (preMatchMonsters.Any())
            {
                Position playerPosition = Map.GetEmptyPosition();
                Player mosnter = preMatchMonsters.Dequeue();
                Map.AddPlayerToPosition(mosnter, playerPosition);
            }
        }
        private bool TooManyPlayers()
        {
            return Map.PlayerCount + 20 >= Map.PlayerCapacity;
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
            if (player.Role == Role.MONSTER)
                preMatchMonsters.Enqueue(player);
            if (player.Role == Role.SURVIVOR)
                preMatchSurvivors.Enqueue(player);

            player.Notify("You've been added to the game queue. You'll be notified when your match starts. Stay alert!");
        }
    }
}
