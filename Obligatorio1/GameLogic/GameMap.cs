using System;
using System.Collections.Generic;
using GameLogicException;

namespace GameLogic
{
    public class GameMap
    {
        private int height;
        private int length;
        private Player[,] map;
        private int playerCount;
        private int playerCapacity;

        public int PlayerCount {
            get {
                return playerCount;
            }
            private set {
                playerCount = value;
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

        public GameMap()
        {
            this.length = 8;
            this.height = 8;
            this.playerCapacity = length * height;
            this.map = new Player[length, height];
            this.PlayerCount = 0;
        }

        public GameMap(int length, int height)
        {
            this.length = length;
            this.height = height;
            this.playerCapacity = length * height;
            this.map = new Player[length, height];
            this.PlayerCount = 0;
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
            playerCount++;

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
                map[position.Row, position.Column] = null;
                playerCount--;
            }
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
            return playerCount == playerCapacity;
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