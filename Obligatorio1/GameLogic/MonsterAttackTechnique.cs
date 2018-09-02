using System;
using System.Linq;

namespace GameLogic
{
    public class MonsterAttackTechnique : AttackTechnique
    {
        private int hitPoints;
        private bool[] isAbleToAttackRole;
        public static readonly int DEFAULT_MONSTER_HITPOINTS = 10;

        public MonsterAttackTechnique()
        {
            isAbleToAttackRole = RoleMethods.GetBooleanArrayOfRoles();
            SetRolesAbleToAttack();
            hitPoints = DEFAULT_MONSTER_HITPOINTS;
        }

        private void SetRolesAbleToAttack()
        {
            for (int i = 0; i < isAbleToAttackRole.Length; i++)
            {
                isAbleToAttackRole[i] = true;
            }
        }

        public override int HitPoints {
            get {
                return hitPoints;
            }

            protected set {
                hitPoints = value;
            }
        }

        public override bool CanAttack(Role role)
        {
            return isAbleToAttackRole[(int) role];
        }
    }
}