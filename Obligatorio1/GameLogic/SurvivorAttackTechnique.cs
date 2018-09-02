namespace GameLogic
{
    public class SurvivorAttackTechnique:AttackTechnique
    {
        private int hitPoints;
        private bool[] isAbleToAttackRole;
        public static readonly int DEFAULT_SURVIVOR_HITPOINTS = 5;

        public SurvivorAttackTechnique()
        {
            isAbleToAttackRole = RoleMethods.GetBooleanArrayOfRoles();
            SetRolesAbleToAttack();
            hitPoints = DEFAULT_SURVIVOR_HITPOINTS;
        }

        private void SetRolesAbleToAttack()
        {
            isAbleToAttackRole[(int)Role.MONSTER] = true;
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
            return isAbleToAttackRole[(int)role];
        }
    }
}