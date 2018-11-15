using GameLogicException;
using System;

namespace GameLogic
{
    public class Survivor:Player
    {
        public static readonly int DEFAULT_SURVIVOR_HEALTH = 20;
        public static readonly int DEFAULT_SURVIVOR_KILLSCOREPOINTS = 50;

        private Role role;
        private int health;
        private Position actualPosition;
        private AttackTechnique technique;
        private GameMap map;
        private bool enabledAttackAction;
        private int killScorePoints;

        public Survivor()
        {
            this.health = DEFAULT_SURVIVOR_HEALTH;
            this.role = Role.SURVIVOR;
            this.actualPosition = new Position(0, 0);
            this.technique = new SurvivorAttackTechnique();
            this.killScorePoints = DEFAULT_SURVIVOR_KILLSCOREPOINTS;

        }

        public Survivor(Position initialPosition)
        {
            this.health = DEFAULT_SURVIVOR_HEALTH;
            this.technique = new SurvivorAttackTechnique();
            this.role = Role.SURVIVOR;
            this.actualPosition = initialPosition;
        }

        public override int KillScorePoints { get { return killScorePoints; } protected set { killScorePoints = value; } }

        public override int Health {
            get {
                return this.health;
            }

            protected set {
                health = value;
            }
        }

        public override Role Role {
            get {
                return this.role;
            }

            protected set {               
            }
        }

        public override Position ActualPosition {
            get {
                return this.actualPosition;
            }

            set {
                actualPosition = value;
            }
        }

        protected override AttackTechnique Technique {
            get {
                return technique;
            }

            set {
                technique = value;
            }
        }

        public override GameMap Map {
            get {
                return map;
            }

            set {
                map = value;
            }
        }

        public override bool EnabledAttackAction {
            get {
                return enabledAttackAction;
            }

            set {
                enabledAttackAction = value;
            }
        }

        protected override void Damage(int hitPoints, Player attacker)
        {
            Health -= hitPoints;
            attacker.Notify("Attack hit on enemy " + ToString() + ". Enemy HP: " +Health);
            Notify("You are being ATTACKED by " + attacker.ToString() + "!!! Your HP: " + Health + " / Enemy HP: " + this.Health);
            NotifyServer(ToString() + " / HP = " + Health);

            if (Health <= 0)
            {
                Notify("RIP - you are dead");
                NotifyServer(this.ToString() + "is dead");
                attacker.AddPoints(attacker.KillScorePoints);
                Health = 0;
                Player attacked = Map.GetPlayer(ActualPosition);
                Map.SurvivorCount--;
                Map.RemovePlayer(ActualPosition);
                Map.PlayerRemovedEvent();
            }
        }

        

        public override void Join(Game game)
        {
            if (game.TooManySurvivors())
                throw new MapIsFullException("There are too many survivors, join as monster or wait");

            Map = game.Map;
            Map.AddPlayerToEmptyPosition(this);
            Map.SurvivorCount++;
            NotifyServer += game.Notify;
            NotifyServer(this.ToString() + " joined the game");
            SpotNearbyPlayers();
        }
    }
}
