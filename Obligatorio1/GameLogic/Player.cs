using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public abstract class Player
    {
        public abstract Role Role { get; protected set; }
        public abstract int Health { get; protected set; }
        public virtual int HitPoints {
            get {
                return attackTechnique.HitPoints;
            }
            protected set {

            }
        }
        public bool IsDead {
            get {
                return Health == 0;
            }
            private set {
            }
        }
        public abstract Position Position { get; set; }
        protected AttackTechnique attackTechnique;

        protected virtual void Damage(int hitPoints)
        {
            Health -= hitPoints;
            if(Health < 0)
            {
                Health = 0;
            }
        }
        public virtual void Attack(Player objective)
        {
            if (attackTechnique.CanAttack(objective.Role))
            {
                objective.Damage(attackTechnique.HitPoints);
            }
        }
    }
}
