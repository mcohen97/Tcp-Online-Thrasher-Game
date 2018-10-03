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
            set { name = value; }
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
        protected IMovementController movementController;
        protected abstract AttackTechnique Technique { get; set; }
        public abstract GameMap Map { get; set; }
        public Action<string> Notify { get; set; }
        public Action<string> NotifyServer { get; set; }
        public abstract bool EnabledAttackAction { get; set; }

        public Player()
        {
            movementController = new TankMovementController(CardinalPoint.NORTH);
            Map = new GameMap(1, 1);
            name = "Unidentified Player";
            EnabledAttackAction = true;
            Notify += (s) => { }; //Do nothing
            NotifyServer += (s) => { }; //Do nothing
        }

        protected abstract void Damage(int hitPoints);

        public virtual void Attack(Player target)
        {
            if (Technique.CanAttack(target.Role))
            {
                target.Damage(Technique.HitPoints);
                target.Notify("You are being ATTACKED by " + this.ToString() + "!!! Your HP: " + target.Health + " / Enemy HP: " + this.Health);
                this.Notify("Attack hit on enemy " + target.ToString() + ". Enemy HP: " + target.Health);
                this.NotifyServer(this.ToString() + " attacked " + target.ToString());
                this.NotifyServer(target.ToString() + " / HP = " + target.Health);
            }
        }

        public void AttackZone()
        {
            if (EnabledAttackAction)
            {
                ICollection<Player> targets = Map.GetPlayersNearPosition(ActualPosition);
                if (targets.Any())
                {
                    foreach (Player target in targets)
                    {
                        Attack(target);
                    }
                }
                else
                {
                    Notify("No one is near you");
                }

               
                
            }
            else
            {
                Notify("Your attack action is disable");
            }
            

        }

        public CardinalPoint CompassDirection {
            get {
                return movementController.ActualCompassDirection();
            }

            set {
                movementController.Turn(value);
            }
        }

        public void Move(Movement movement)
        {
            Position newPosition = movementController.Move(this.ActualPosition, movement, 1);
            Map.MovePlayer(this.ActualPosition, newPosition);
            Notify("You moved to " + this.ActualPosition);
            NotifyServer(this.ToString() + " moved to " + this.ActualPosition);
            SpotNearbyPlayers();
        }

        public void Turn(CardinalPoint direction)
        {
            movementController.Turn(direction);
            Notify("You are looking at " + Enum.GetName(typeof(CardinalPoint), direction));
        }

        public void MoveFast(Movement movement)
        {
            Position newPosition = movementController.Move(this.ActualPosition, movement, 2);
            Map.MovePlayer(this.ActualPosition, newPosition);
            Notify("You moved to " + this.ActualPosition);
            NotifyServer(this.ToString() + " moved to " + this.ActualPosition);
            SpotNearbyPlayers();
        }

        public abstract void Join(Game game);

        public ICollection<Player> SpotNearbyPlayers()
        {
            ICollection<Player> playersSpotted = Map.GetPlayersNearPosition(ActualPosition);
            foreach (Player player in playersSpotted)
            {
                player.Notify("You've been SPOTTED by "+this.ToString()+" at "+ActualPosition);
                this.Notify(player+ " is close to you at "+ player.ActualPosition);
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
            return this.Name == parameter.Name;
        }
    }

}
