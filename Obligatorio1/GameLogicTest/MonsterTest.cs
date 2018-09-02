using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;

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
    }
}
