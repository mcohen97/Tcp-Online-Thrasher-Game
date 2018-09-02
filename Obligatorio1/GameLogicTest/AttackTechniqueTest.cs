using GameLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicTest
{
    class AttackTechniqueTest
    {
        //Constructor tests
        [TestMethod]
        public void NewSurvivorAttackTechniqueTest()
        {
            AttackTechnique attackTechnique = new SurvivorAttackTechnique();
            Assert.IsNotNull(attackTechnique);
        }

        [TestMethod]
        public void NewMonsterAttackTechniqueTest()
        {
            AttackTechnique attackTechnique = new MonsterAttackTechnique();
            Assert.IsNotNull(attackTechnique);
        }

        //Hit Points tests
        [TestMethod]
        public void SurvivorAttackTechniqueHitPointsTest()
        {
            AttackTechnique attackTechnique = new SurvivorAttackTechnique();
            Assert.AreEqual(5, attackTechnique.HitPoints);
        }

        [TestMethod]
        public void MonsterAttackTechniqueHitPointsTest()
        {
            AttackTechnique attackTechnique = new MonsterAttackTechnique();
            Assert.AreEqual(10, attackTechnique.HitPoints);
        }

        //Friendly fire tests
        [TestMethod]
        public void MonsterAttackTechniqueCanAttackSurvivorTest()
        {
            AttackTechnique attackTechnique = new MonsterAttackTechnique();
            Assert.IsTrue(attackTechnique.CanAttack(GameRole.SURVIVOR));
        }

        [TestMethod]
        public void MonsterAttackTechniqueCanAttackMonsterTest()
        {
            AttackTechnique attackTechnique = new MonsterAttackTechnique();
            Assert.IsTrue(attackTechnique.CanAttack(GameRole.SURVIVOR));
        }

        [TestMethod]
        public void SurvivorAttackTechniqueCanAttackMonsterTest()
        {
            AttackTechnique attackTechnique = new SurvivorAttackTechnique();
            Assert.IsTrue(attackTechnique.CanAttack(GameRole.MONSTER));
        }

        [TestMethod]
        public void SurvivorAttackTechniqueCanAttackSurvivorTest()
        {
            AttackTechnique attackTechnique = new SurvivorAttackTechnique();
            Assert.IsFalse(attackTechnique.CanAttack(GameRole.SURVIVOR));
        }

    }
}
