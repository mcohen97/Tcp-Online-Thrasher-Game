using System;
using System.Collections.Generic;
using GameLogicException;

namespace GameLogic
{
    public class GameMap
    {
        public static readonly int EMPTY_SPACES = 20; //empty spaces so map isnt overpopulated

        private int height;
        private int length;
        private Player[,] map;
        private int monsterCount;
        private int survivorCount;
        private int playerCapacity;

        public int PlayerCount {
            get {
                return monsterCount + survivorCount ;
            }
            private set {
                ;
            }
        }
        public int MonsterCount {
            get {
                return monsterCount;
            }
            set {
                monsterCount = value;
            }
        }
        public int SurvivorCount {
            get {
                return survivorCount;
            }
            set {
                survivorCount = value;
            }
        }
        public int PlayerCapacity {
            get {
                return playerCapacity;
            }

            private set {
                playerCapacity = value;
            }
        }
        public Action CheckGame { get; set; }


        public GameMap()
        {
            this.length = 8;
            this.height = 8;
            this.playerCapacity = length * height - EMPTY_SPACES;
            this.map = new Player[length, height];
            this.monsterCount = 0;
            this.survivorCount = 0;
            this.CheckGame += () => { }; //Do nothing
        }
        public GameMap(int length, int height)
        {
            this.length = length;
            this.height = height;
            this.playerCapacity = length * height - EMPTY_SPACES;
            this.map = new Player[length, height];
            this.monsterCount = 0;
            this.survivorCount = 0;
            this.CheckGame += () => { }; //Do nothing

        }

        public void AddPlayerToPosition(Player player, Position initialPosition)
        {
            if (!IsValidPosition(initialPosition))
                throw new InvalidPositionException();

            if (!IsEmptyPosition(initialPosition))
                throw new OccupiedPositionException();

            map[initialPosition.Row, initialPosition.Column] = player;
            player.Map = this;
            player.ActualPosition = initialPosition;
        }

        public void MovePlayer(Position from, Position to)
        {
            if (!IsEmptyPosition(from))
            {
                if (!IsValidPosition(to))
                    throw new InvalidPositionException("| Invalid move - map ends here |");

                if (!IsEmptyPosition(to))
                    throw new OccupiedPositionException("| Invalid move - there's another player in that position ");

                Player playerMoved = map[from.Row, from.Column];
                map[to.Row, to.Column] = playerMoved;
                playerMoved.ActualPosition = to;
                map[from.Row, from.Column] = null;
            }
        }

        public void RemovePlayer(Position position)
        {
            if (!IsValidPosition(position))
                throw new InvalidPositionException();


            if (map[position.Row, position.Column] != null)
            {
                Player playerRemoved = map[position.Row, position.Column];
                if (playerRemoved.Role == Role.MONSTER)
                    MonsterCount--;
                if (playerRemoved.Role == Role.SURVIVOR)
                    SurvivorCount--;                
                map[position.Row, position.Column] = null;
            }
            CheckGame();
        }

        public Player GetPlayer(Position position)
        {
            if (!IsValidPosition(position))
                throw new InvalidPositionException();

            return map[position.Row, position.Column];
        }

        public bool IsEmptyPosition(Position position)
        {
            return IsValidPosition(position) && IsEmptyPosition(position.Row, position.Column);
        }

        private bool IsEmptyPosition(int i, int j)
        {
            return map[i, j] == null;
        }

        private bool IsValidPosition(Position position)
        {

            if (position.Row >= height || position.Row < 0)
                return false;
            if (position.Column >= length || position.Column < 0)
                return false;

            return true;
        }

        public ICollection<Player> GetPlayersNearPosition(Position position)
        {
            ICollection<Player> playersNearPosition = new List<Player>();
            for (int row = position.Row - 1; row <= position.Row + 1; row++)
            {
                for (int column = position.Column - 1; column <= position.Column + 1; column++)
                {
                    Position actualPosition = new Position(row, column);

                    if (IsValidPosition(actualPosition) && !IsEmptyPosition(actualPosition) && !actualPosition.Equals(position))
                        playersNearPosition.Add(GetPlayer(actualPosition));
                }
            }

            return playersNearPosition;
        }

        public bool IsFull()
        {
            return PlayerCount == PlayerCapacity;
        }

        public Position GetEmptyPosition()
        {
            if (IsFull())
                throw new MapIsFullException();

            Position emptyPosition = new Position(-1,-1);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(IsEmptyPosition(i,j))
                    {
                        emptyPosition = new Position(i, j);
                        break;
                    }
                }
            }
            return emptyPosition;
        }

        public ICollection<Player> GetPlayers()
        {
            ICollection<Player> players = new List<Player>();
            foreach (Player player in map)
            {
                if(player != null)
                {
                    players.Add(player);
                }
            }
            return players;
        }
    }
}