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
            Assert.AreEqual(20, survivor.Health);
        }

        [TestMethod]
        public void DamageTest()
        {
            Survivor survivor = new Survivor();
            int attackPoints = 10;
            survivor.Damage(attackPoints);
            Assert.AreEqual(10, survivor.Health);
        }

        [TestMethod]
        public void TotalDamageTest()
        {
            Survivor survivor = new Survivor();
            int attackPoints = 25;
            survivor.Damage(attackPoints);
            Assert.AreEqual(0, survivor.Health);
        }

        [TestMethod]
        public void AttackSurvivorTest()
        {
            Survivor survivorAttacking = new Survivor();
            Survivor survivorAttacked = new Survivor();
            survivorAttacking.Attack(survivorAttacked);
            Assert.AreEqual(20, survivorAttacked.Health);
        }
    }
}
