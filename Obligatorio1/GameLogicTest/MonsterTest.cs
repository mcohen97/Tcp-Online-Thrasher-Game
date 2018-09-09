using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;
using GameLogicException;

namespace GameLogicTest
{
    [TestClass]
    public class MonsterTest
    {
        [TestMethod]
        public void NewMonsterTest()
        {
            Monster monster = new Monster();
            Assert.IsNotNull(monster);
        }

        [TestMethod]
        public void GetHealthTest()
        {
            Monster monster = new Monster();
            Assert.AreEqual(Monster.DEFAULT_MONSTER_HEALTH, monster.Health);
        }

        [TestMethod]
        public void AttackSurvivorTest()
        {
            Monster monster = new Monster();
            Survivor survivorAttacked = new Survivor();
            int healthBeforeAttack = survivorAttacked.Health;
            monster.Attack(survivorAttacked);
            int healthExpected = healthBeforeAttack - monster.HitPoints;
            Assert.AreEqual(healthExpected, survivorAttacked.Health);
        }

        [TestMethod]
        public void AttackMonsterTest()
        {
            Monster monster = new Monster();
            Monster monsterAttacked = new Monster();
            int healthBeforeAttack = monsterAttacked.Health;
            monster.Attack(monsterAttacked);
            int healthExpected = healthBeforeAttack - monster.HitPoints;
            Assert.AreEqual(healthExpected, monsterAttacked.Health);
        }

        [TestMethod]
        public void IsDeadMonsterTest()
        {
            Monster monster = new Monster();
            Monster monsterAttacked = new Monster();
            for (int i = 0; i < 10; i++)
            {
                monster.Attack(monsterAttacked);
            }
            Assert.IsTrue(monsterAttacked.IsDead);
        }

        [TestMethod]
        public void IsNotDeadMonsterTest()
        {
            Monster monsterAttacked = new Monster();
            Monster monster = new Monster();
            monster.Attack(monsterAttacked);
            Assert.IsFalse(monsterAttacked.IsDead);
        }

        [TestMethod]
        public void GetPosition()
        {
            Monster monster = new Monster();
            monster.Position = new Position(1, 1);
            Assert.AreEqual(monster.Position, new Position(1, 1));
        }

        [TestMethod]
        public void MoveForwardSouthTest()
        {
            Monster monster = new Monster();
            GameMap map = new GameMap(8, 8);
            map.AddPlayerToPosition(monster, new Position(1, 1));
            monster.CompassDirection = CardinalPoint.SOUTH;
            monster.Move(Movement.FORWARD);
            Assert.AreEqual(monster.Position, new Position(2, 1));
        }

        [TestMethod]
        public void MoveForwardNorthTest()
        {
            Monster monster = new Monster();
            GameMap map = new GameMap(8, 8);
            map.AddPlayerToPosition(monster, new Position(1, 1));
            monster.CompassDirection = CardinalPoint.NORTH;
            monster.Move(Movement.FORWARD);
            Assert.AreEqual(monster.Position, new Position(0, 1));
        }

        [TestMethod]
        public void MoveForwardEastTest()
        {
            Monster monster = new Monster();
            GameMap map = new GameMap(8, 8);
            map.AddPlayerToPosition(monster, new Position(1, 1));
            monster.CompassDirection = CardinalPoint.EAST;
            monster.Move(Movement.FORWARD);
            Assert.AreEqual(monster.Position, new Position(1, 2));
        }

        [TestMethod]
        public void MoveForwardWestTest()
        {
            Monster monster = new Monster();
            GameMap map = new GameMap(8, 8);
            map.AddPlayerToPosition(monster, new Position(1, 1));
            monster.CompassDirection = CardinalPoint.WEST;
            monster.Move(Movement.FORWARD);
            Assert.AreEqual(monster.Position, new Position(1, 0));
        }

        [TestMethod]
        public void MoveBackwardTest()
        {
            Monster monster = new Monster();
            GameMap map = new GameMap(8, 8);
            map.AddPlayerToPosition(monster, new Position(1, 1));
            monster.CompassDirection = CardinalPoint.SOUTH;
            monster.Move(Movement.BACKWARD);
            Assert.AreEqual(monster.Position, new Position(0, 1));
        }

        [TestMethod]
        public void TurnEastTest()
        {
            Monster monster = new Monster();
            GameMap map = new GameMap(8, 8);
            map.AddPlayerToPosition(monster, new Position(1, 1));
            monster.CompassDirection = CardinalPoint.SOUTH;
            monster.Turn(CardinalPoint.EAST);
            monster.Move(Movement.FORWARD);
            Assert.AreEqual(monster.Position, new Position(1, 2));
        }

        [TestMethod]
        public void TurnWestTest()
        {
            Monster monster = new Monster();
            GameMap map = new GameMap(8, 8);
            map.AddPlayerToPosition(monster, new Position(1, 1));
            monster.CompassDirection = CardinalPoint.SOUTH;
            monster.Turn(CardinalPoint.WEST);
            monster.Move(Movement.FORWARD);
            Assert.AreEqual(monster.Position, new Position(1, 0));
        }

        [TestMethod]
        public void TurnNorthTest()
        {
            Monster monster = new Monster();
            GameMap map = new GameMap(8, 8);
            map.AddPlayerToPosition(monster, new Position(1, 1));
            monster.CompassDirection = CardinalPoint.SOUTH;
            monster.Turn(CardinalPoint.NORTH);
            monster.Move(Movement.FORWARD);
            Assert.AreEqual(monster.Position, new Position(0, 1));
        }

        [TestMethod]
        public void MoveFastTest()
        {
            Monster monster = new Monster();
            GameMap map = new GameMap(8, 8);
            map.AddPlayerToPosition(monster, new Position(1, 1));
            monster.CompassDirection = CardinalPoint.SOUTH;
            monster.MoveFast(Movement.FORWARD);
            Assert.AreEqual(monster.Position, new Position(3, 1));
        }

        [TestMethod]
        public void MonsterAttackZoneTest()
        {
            GameMap gameMap = new GameMap();
            Monster monster = new Monster();
            Player player1 = new Survivor();
            Player player2 = new Survivor();
            Player player3 = new Survivor();
            Player player4 = new Survivor();
            Player player5 = new Survivor();
            Player player6 = new Survivor();
            Player player7 = new Survivor();
            Player player8 = new Survivor();

            gameMap.AddPlayerToPosition(player1, new Position(3, 3));
            gameMap.AddPlayerToPosition(player2, new Position(3, 4));
            gameMap.AddPlayerToPosition(player3, new Position(3, 5));
            gameMap.AddPlayerToPosition(player4, new Position(4, 3));
            gameMap.AddPlayerToPosition(player5, new Position(4, 5));
            gameMap.AddPlayerToPosition(player6, new Position(5, 3));
            gameMap.AddPlayerToPosition(player7, new Position(5, 4));
            gameMap.AddPlayerToPosition(player8, new Position(5, 5));
            gameMap.AddPlayerToPosition(monster, new Position(4, 4));

            monster.AttackZone();
            monster.AttackZone();

            Assert.AreEqual(0, gameMap.GetPlayersNearPosition(monster.Position).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPositionException))]
        public void InvalidPositionMovementTest()
        {
            GameMap map = new GameMap(8, 8);
            Monster player = new Monster();
            map.AddPlayerToPosition(player, new Position(1, 1));
            player.CompassDirection = CardinalPoint.NORTH;
            player.MoveFast(Movement.FORWARD);
        }

        [TestMethod]
        [ExpectedException(typeof(OccupiedPositionException))]
        public void OccupiedPositionMovementTest()
        {
            GameMap map = new GameMap(8, 8);
            Monster player = new Monster();
            Monster player2 = new Monster();
            map.AddPlayerToPosition(player, new Position(1, 1));
            map.AddPlayerToPosition(player2, new Position(1, 2));
            player.CompassDirection = CardinalPoint.EAST;
            player.Move(Movement.FORWARD);
        }
    }
}
