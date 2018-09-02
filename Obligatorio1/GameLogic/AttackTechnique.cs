namespace GameLogic
{
    public abstract class AttackTechnique
    {
        public abstract int HitPoints { get; protected set; }
        public abstract bool CanAttack(Role role);
    }
}