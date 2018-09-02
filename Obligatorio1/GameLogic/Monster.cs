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

        public Monster()
        {
            this.health = DEFAULT_MONSTER_HEALTH;
            base.attackTechnique = new MonsterAttackTechnique();
            this.role = Role.MONSTER;
            this.position = new Position(0, 0);

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
    }
}
