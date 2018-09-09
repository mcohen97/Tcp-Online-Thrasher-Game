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
        private Position position;
        private AttackTechnique attackTechnique;
        private GameMap map;

        public Monster()
        {
            this.health = DEFAULT_MONSTER_HEALTH;
            this.role = Role.MONSTER;
            this.position = new Position(0, 0);
            this.attackTechnique = new MonsterAttackTechnique();
        }

        public Monster(Position initialPosition)
        {
            this.health = DEFAULT_MONSTER_HEALTH;
            this.role = Role.MONSTER;
            this.position = initialPosition;
            this.attackTechnique = new MonsterAttackTechnique();
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

        public override Position Position {
            get {
                return this.position;
            }

            set {
                this.position = value;
            }
        }

        protected override AttackTechnique AttackTechnique {
            get {
                return attackTechnique;
            }

            set {
                attackTechnique = value;
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

        
    }
}
