using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public abstract class Player
    {
        protected string name;
        public string Name {
            get { return name; }
            protected set { name = value; }
        }
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
        public Action<string> Notify { get; set; }

        public Player()
        {
            playerController = new TankMovementController(CardinalPoint.NORTH);
            Map = new GameMap(1, 1);
            name = "Unidentified Player";
        }

        protected virtual void Damage(int hitPoints)
        {
            Health -= hitPoints;
            if(Health <= 0)
            {
                Health = 0;
                Map.RemovePlayer(ActualPosition);
                this.Notify("RIP - you are dead");
            }
        }

        public virtual void Attack(Player target)
        {
            if (Technique.CanAttack(target.Role))
            {
                target.Damage(Technique.HitPoints);
                target.Notify("You are being ATTACKED by this MOTHERFUCKER" + this.ToString() + "!!! Your HP: " + this.Health);
                this.Notify("Enemy " + target.ToString() + " has been hit. Enemy HP: " + target.Health);
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
            ICollection<Player> playersSpotted = Map.GetPlayersNearPosition(ActualPosition);
            foreach (Player player in playersSpotted)
            {
                player.Notify("You've been SPOTTED by "+this.ToString());
                this.Notify(player+ " is close to you");
            }
            return playersSpotted;
        }

        public override string ToString()
        {
            return name + " ("+RoleMethods.RoleToString(this.Role)+")";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Player))
                return false;
            Player parameter = (Player)obj;
            return this.ToString() == parameter.ToString();
        }
    }

}
