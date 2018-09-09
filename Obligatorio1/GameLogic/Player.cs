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
                return AttackTechnique.HitPoints;
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
        protected IPlayerController playerController;
        protected abstract AttackTechnique AttackTechnique { get; set; }
        public abstract GameMap Map { get; set; }

        public Player()
        {
            playerController = new TankMovementController(CardinalPoint.NORTH);
            Map = new GameMap(1, 1);
        }

        protected virtual void Damage(int hitPoints)
        {
            Health -= hitPoints;
            if(Health <= 0)
            {
                Health = 0;
                Map.RemovePlayer(Position);
            }
        }

        public virtual void Attack(Player target)
        {
            if (AttackTechnique.CanAttack(target.Role))
            {
                target.Damage(AttackTechnique.HitPoints);
            }
        }

        public void AttackZone()
        {
            ICollection<Player> targets = Map.GetPlayersNearPosition(Position);
            foreach (Player target in targets)
            {
                Attack(target);
            }
        }

        public CardinalPoint CompassDirection {
            get {
                return playerController.ActualCompassDirection();
            }

            set {
                playerController.Turn(value);
            }
        }

        public void Move(Movement movement)
        {
            Position newPosition = playerController.Move(this.Position, movement, 1);
            Map.MovePlayer(this.Position, newPosition);
        }

        public void Turn(CardinalPoint direction)
        {
            playerController.Turn(direction);
        }

        public void MoveFast(Movement movement)
        {
            Position newPosition = playerController.Move(this.Position, movement, 2);
            Map.MovePlayer(this.Position, newPosition);
        }

    }
}
