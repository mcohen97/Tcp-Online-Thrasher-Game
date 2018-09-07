using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;
using GameLogicException;

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
            Assert.AreEqual(player.Position, initialPosition);
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
            Assert.AreEqual(player.Position, finalPosition);
        }

        [TestMethod]
        public void IsPositionEmptyFalseTest()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            Position position = new Position(0, 0);
            gameMap.AddPlayerToPosition(player, position);
            Assert.IsFalse(gameMap.IsPositionEmpty(position));
        }

        [TestMethod]
        public void RemovePlayerTest()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            Position position = new Position(0, 0);
            gameMap.AddPlayerToPosition(player, position);
            gameMap.RemovePlayer(player.Position);
            Assert.IsTrue(gameMap.IsPositionEmpty(position));
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
            gameMap.RemovePlayer(player3.Position);

            Assert.AreEqual(2, gameMap.PlayerCount);
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
            gameMap.MovePlayer(player.Position, new Position(-1, 0));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPositionException))]
        public void InvalidMovePositionTest2()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            gameMap.AddPlayerToPosition(player, new Position(0, 0));
            gameMap.MovePlayer(player.Position, new Position(10, 0)); //GameMap doesnt have the responsability of control game rules
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPositionException))]
        public void InvalidMovePositionTest3()
        {
            GameMap gameMap = new GameMap(8, 8);
            Player player = new Monster();
            gameMap.AddPlayerToPosition(player, new Position(0, 0));
            gameMap.MovePlayer(player.Position, new Position(2, 10));
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
