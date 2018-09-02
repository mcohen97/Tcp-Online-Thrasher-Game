using System;

namespace GameLogic
{
    public class Survivor:Player
    {
        private int health;
        private static readonly int DEFAULT_SURVIVOR_HEALTH = 20;

        public Survivor()
        {
            this.health = DEFAULT_SURVIVOR_HEALTH;
            base.attackTechnique = new SurvivorAttackTechnique();
            base.role = Role.SURVIVOR;

        }

        public override int Health {
            get {
                return this.health;
            }

            protected set {
                health = value;
            }
        }
    }
}