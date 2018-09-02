using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public abstract class Player
    {
        protected AttackTechnique attackTechnique;
        public Role role;
        public abstract int Health { get; protected set; }
        public virtual int HitPoints {
            get {
                return attackTechnique.HitPoints;
            }
            protected set {

            }
        }

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
            if (attackTechnique.CanAttack(objective.role))
            {
                objective.Damage(attackTechnique.HitPoints);
            }
        }
    }
}
