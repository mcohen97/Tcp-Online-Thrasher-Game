using GameLogicException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Monster : Player
    {
        public static readonly int DEFAULT_MONSTER_HEALTH = 100;
        private Role role;
        private int health;
        private Position actualPosition;
        private AttackTechnique technique;
        private GameMap map;
        private bool enabledAttackAction;

        public Monster()
        {
            this.health = DEFAULT_MONSTER_HEALTH;
            this.role = Role.MONSTER;
            this.actualPosition = new Position(0, 0);
            this.technique = new MonsterAttackTechnique();
        }

        public Monster(Position initialPosition)
        {
            this.health = DEFAULT_MONSTER_HEALTH;
            this.role = Role.MONSTER;
            this.actualPosition = initialPosition;
            this.technique = new MonsterAttackTechnique();
        }

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
                this.actualPosition = value;
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

        protected override void Damage(int hitPoints)
        {
            Health -= hitPoints;
            if (Health <= 0)
            {
                Health = 0;
                Map.RemovePlayer(ActualPosition);
                Map.MonsterCount--;
                Map.PlayerRemovedEvent();
                this.Notify("RIP - you are dead");
                NotifyServer(this.ToString() + "is dead");

            }
        }



        public override void Join(Game game)
        {
            if (game.TooManyMonsters())
                throw new MapIsFullException("There are too many monsters, join as survivor or wait");

            Map = game.Map;
            Map.AddPlayerToEmptyPosition(this);
            Map.MonsterCount++;
            NotifyServer += game.Notify;
            NotifyServer(this.ToString() + " joined the game");
            SpotNearbyPlayers();
        }
    }
}
