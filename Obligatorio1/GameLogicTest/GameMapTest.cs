using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;
using GameLogicException;
using System.Collections.Generic;

namespace GameLogicTest
{
    [TestClass]
    public class GameMapTest
    {
        [TestMethod]
        public void NewGameMapTest()
        {
            GameMap gameMap = new GameMap();
            Assert.IsNotNull(gameMap);
        }

        [TestMethod]
        public void ParametterConstructorTest()
        {
            int length = 8;
            int height = 8;
            GameMap gameMap = new GameMap(length, height);
            Assert.IsNotNull(gameMap);
        }

        [TestMethod]
        public void AddPlayerToPositionTest()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            Position initialPosition = new Position(0, 0);
            gameMap.AddPlayerToPosition(player, initialPosition);
            Assert.AreEqual(player.ActualPosition, initialPosition);
        }

        [TestMethod]
        public void MovePlayerTest()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            Position initialPosition = new Position(0, 0);
            gameMap.AddPlayerToPosition(player, initialPosition);
            Position finalPosition = new Position(1, 1);
            gameMap.MovePlayer(initialPosition, finalPosition);
            Assert.AreEqual(player.ActualPosition, finalPosition);
        }

        [TestMethod]
        public void IsPositionEmptyFalseTest()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            Position position = new Position(0, 0);
            gameMap.AddPlayerToPosition(player, position);
            Assert.IsFalse(gameMap.IsEmptyPosition(position));
        }

        [TestMethod]
        public void RemovePlayerTest()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            Position position = new Position(0, 0);
            gameMap.AddPlayerToPosition(player, position);
            gameMap.RemovePlayer(player.ActualPosition);
            Assert.IsTrue(gameMap.IsEmptyPosition(position));
        }

        [TestMethod]
        public void PlayersInGameTest()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player1 = new Monster();
            Player player2 = new Survivor();
            Player player3 = new Survivor();

            gameMap.AddPlayerToPosition(player1, new Position(0, 0));
            gameMap.AddPlayerToPosition(player2, new Position(0, 1));
            gameMap.AddPlayerToPosition(player3, new Position(0, 2));
            gameMap.RemovePlayer(player3.ActualPosition);

            Assert.AreEqual(2, gameMap.GetPlayers().Count);
        }

        [TestMethod]
        public void PlayersNearPositionTest()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player1 = new Monster();
            Player player2 = new Survivor();
            Player player3 = new Survivor();
            Player player4 = new Survivor();
            Player player5 = new Survivor();
            Player player6 = new Survivor();
            Player player7 = new Monster();
            Player player8 = new Monster();

            gameMap.AddPlayerToPosition(player1, new Position(3, 3));
            gameMap.AddPlayerToPosition(player2, new Position(3, 4));
            gameMap.AddPlayerToPosition(player3, new Position(3, 5));
            gameMap.AddPlayerToPosition(player4, new Position(4, 3));
            gameMap.AddPlayerToPosition(player5, new Position(4, 5));
            gameMap.AddPlayerToPosition(player6, new Position(5, 3));
            gameMap.AddPlayerToPosition(player7, new Position(5, 4));
            gameMap.AddPlayerToPosition(player8, new Position(5, 5));

            ICollection<Player> playersNearPosition = gameMap.GetPlayersNearPosition(new Position(4, 4));

            Assert.AreEqual(8, playersNearPosition.Count);
        }

        [TestMethod]
        public void PlayersNearPositionTest2()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player1 = new Monster();
            Player player2 = new Survivor();
            Player player3 = new Survivor();
            Player player4 = new Survivor();
            Player player5 = new Survivor();
            Player player6 = new Survivor();
            Player player7 = new Monster();
            Player player8 = new Monster();
            Player playerInPosition = new Monster();

            gameMap.AddPlayerToPosition(player1, new Position(3, 3));
            gameMap.AddPlayerToPosition(player2, new Position(3, 4));
            gameMap.AddPlayerToPosition(player3, new Position(3, 5));
            gameMap.AddPlayerToPosition(player4, new Position(4, 3));
            gameMap.AddPlayerToPosition(player5, new Position(4, 5));
            gameMap.AddPlayerToPosition(player6, new Position(5, 3));
            gameMap.AddPlayerToPosition(player7, new Position(5, 4));
            gameMap.AddPlayerToPosition(player8, new Position(5, 5));
            gameMap.AddPlayerToPosition(player8, new Position(4, 4));

            ICollection<Player> playersNearPosition = gameMap.GetPlayersNearPosition(new Position(4, 4));

            Assert.AreEqual(8, playersNearPosition.Count);
        }

        [TestMethod]
        public void PlayersNearPositionTest3()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player2 = new Survivor();
            Player player4 = new Survivor();
            Player player6 = new Survivor();
            Player player7 = new Monster();
            Player player8 = new Monster();
            Player playerInPosition = new Monster();

            gameMap.AddPlayerToPosition(player2, new Position(3, 4));
            gameMap.AddPlayerToPosition(player4, new Position(4, 3));
            gameMap.AddPlayerToPosition(player6, new Position(5, 3));
            gameMap.AddPlayerToPosition(player7, new Position(5, 4));
            gameMap.AddPlayerToPosition(player8, new Position(5, 5));
            gameMap.AddPlayerToPosition(player8, new Position(4, 4));

            ICollection<Player> playersNearPosition = gameMap.GetPlayersNearPosition(new Position(4, 4));

            Assert.AreEqual(5, playersNearPosition.Count);
        }

        [TestMethod]
        public void PlayersNearPositionTest4()
        {
            GameMap gameMap = new GameMap(8, 8);

            ICollection<Player> playersNearPosition = gameMap.GetPlayersNearPosition(new Position(4, 4));

            Assert.AreEqual(0, playersNearPosition.Count);
        }

        //Exceptions Tests

        [TestMethod]
        [ExpectedException(typeof(InvalidPositionException))]
        public void InvalidInitialPositionTest1()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            gameMap.AddPlayerToPosition(player, new Position(-1, -1));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPositionException))]
        public void InvalidInitialPositionTest2()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            gameMap.AddPlayerToPosition(player, new Position(10, 0));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPositionException))]
        public void InvalidInitialPositionTest3()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            gameMap.AddPlayerToPosition(player, new Position(2, 10));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPositionException))]
        public void InvalidMovePositionTest1()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            gameMap.AddPlayerToPosition(player, new Position(0, 0));
            gameMap.MovePlayer(player.ActualPosition, new Position(-1, 0));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPositionException))]
        public void InvalidMovePositionTest2()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            gameMap.AddPlayerToPosition(player, new Position(0, 0));
            gameMap.MovePlayer(player.ActualPosition, new Position(10, 0)); //GameMap doesnt have the responsability of control game rules
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPositionException))]
        public void InvalidMovePositionTest3()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            gameMap.AddPlayerToPosition(player, new Position(0, 0));
            gameMap.MovePlayer(player.ActualPosition, new Position(2, 10));
        }

        [TestMethod]
        [ExpectedException(typeof(OccupiedPositionException))]
        public void OccupiedPositionExceptionTest()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player playerMoved = new Monster();
            Player playerInPosition = new Survivor();
            Position initialPosition = new Position(0, 0);
            Position finalPosition = new Position(0, 1);

            gameMap.AddPlayerToPosition(playerMoved, new Position(0, 0));
            gameMap.AddPlayerToPosition(playerInPosition, new Position(0, 1));

            gameMap.MovePlayer(initialPosition, finalPosition);

        }
    }
}
