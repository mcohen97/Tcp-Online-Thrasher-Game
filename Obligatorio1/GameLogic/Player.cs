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
                return Technique.HitPoints;
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
        public abstract Position ActualPosition { get; set; }
        protected IPlayerController playerController;
        protected abstract AttackTechnique Technique { get; set; }
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
                Map.RemovePlayer(ActualPosition);
            }
        }

        public virtual void Attack(Player target)
        {
            if (Technique.CanAttack(target.Role))
            {
                target.Damage(Technique.HitPoints);
            }
        }

        public void AttackZone()
        {
            ICollection<Player> targets = Map.GetPlayersNearPosition(ActualPosition);
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
            Position newPosition = playerController.Move(this.ActualPosition, movement, 1);
            Map.MovePlayer(this.ActualPosition, newPosition);
        }

        public void Turn(CardinalPoint direction)
        {
            playerController.Turn(direction);
        }

        public void MoveFast(Movement movement)
        {
            Position newPosition = playerController.Move(this.ActualPosition, movement, 2);
            Map.MovePlayer(this.ActualPosition, newPosition);
        }

        public abstract void Join(Game game, Position initialPosition);

        public ICollection<Player> SpotNearbyPlayers()
        {
            return Map.GetPlayersNearPosition(ActualPosition);
        }

    }
}
