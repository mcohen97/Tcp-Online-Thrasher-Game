using System;
using GameLogic;
using GameLogicException;

namespace GameLogicTest
{
    public class GameMap
    {
        private int height;
        private int length;
        private Player[,] map;
        private int playerCount;

        public int PlayerCount {
            get {
                return playerCount;
            }
            private set {
                playerCount = value;
            }
        }

        public GameMap()
        {
            this.length = 8;
            this.height = 8;
            this.map = new Player[length, height];
            this.PlayerCount = 0;
        }

        public GameMap(int length, int height)
        {
            this.length = length;
            this.height = height;
            this.map = new Player[length, height];
            this.PlayerCount = 0;
        }

        public void AddPlayerToPosition(Player player, Position initialPosition)
        {
            if (!IsPositionValid(initialPosition))
                throw new InvalidPositionException();

            if (!IsPositionEmpty(initialPosition))
                throw new OccupiedPositionException();

            map[initialPosition.Row, initialPosition.Column] = player;
            player.Position = initialPosition;
            playerCount++;
            
        }

        public void MovePlayer(Position from, Position to)
        {
            if (!IsPositionEmpty(from))
            {
                if (!IsPositionValid(to))
                    throw new InvalidPositionException();

                if (!IsPositionEmpty(to))
                    throw new OccupiedPositionException();

                Player playerMoved = map[from.Row, from.Column];
                map[to.Row, to.Column] = playerMoved;
                playerMoved.Position = to;
                map[from.Row, from.Column] = null;
            }
        }

        public void RemovePlayer(Position position)
        {
            if (!IsPositionValid(position))
                throw new InvalidPositionException();

            map[position.Row, position.Column] = null;
            playerCount--;
        }

        public Player GetPlayer(Position position)
        {
            if (!IsPositionValid(position))
                throw new InvalidPositionException();

            return map[position.Row, position.Column];
        }

        public bool IsPositionEmpty(Position position)
        {
            if (!IsPositionValid(position)) return false;

            return map[position.Row, position.Column] == null;
        }

        private bool IsPositionValid(Position position)
        {

            if (position.Row >= height || position.Row < 0)
                return false;
            if (position.Column >= length || position.Column < 0)
                return false;

            return true;
        }
    }
}