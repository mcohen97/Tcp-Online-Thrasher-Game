using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;

namespace GameLogicTest
{
    [TestClass]
    public class SurvivorTest
    {

        [TestMethod]
        public void NewSurvivorTest()
        {
            Survivor survivor = new Survivor();
            Assert.IsNotNull(survivor);
        }

        [TestMethod]
        public void GetHealthTest()
        {
            Survivor survivor = new Survivor();
            Assert.AreEqual(Survivor.DEFAULT_SURVIVOR_HEALTH, survivor.Health);
        }

        [TestMethod]
        public void AttackSurvivorTest()
        {
            Survivor survivorAttacking = new Survivor();
            Survivor survivorAttacked = new Survivor();
            survivorAttacking.Attack(survivorAttacked);
            Assert.AreEqual(Survivor.DEFAULT_SURVIVOR_HEALTH, survivorAttacked.Health);
        }

        [TestMethod]
        public void AttackMonsterTest()
        {
            Survivor survivorAttacking = new Survivor();
            Monster monsterAttacked = new Monster();
            int healthBeforeAttack = monsterAttacked.Health;
            survivorAttacking.Attack(monsterAttacked);
            int expectedHealth = healthBeforeAttack - survivorAttacking.HitPoints;
            Assert.AreEqual(expectedHealth, monsterAttacked.Health);
        }

        [TestMethod]
        public void IsDeadSurvivorTest()
        {
            Survivor survivor = new Survivor();
            Monster monster = new Monster();
            monster.Attack(survivor);
            monster.Attack(survivor);
            Assert.IsTrue(survivor.IsDead);
        }

        [TestMethod]
        public void IsNotDeadSurvivorTest()
        {
            Survivor survivor = new Survivor();
            Monster monster = new Monster();
            monster.Attack(survivor);
            Assert.IsFalse(survivor.IsDead);
        }

        [TestMethod]
        public void GetPosition()
        {
            Survivor survivor = new Survivor();
            survivor.ActualPosition = new Position(1, 1);
            Assert.AreEqual(survivor.ActualPosition, new Position(1, 1));
        }
    }
}
