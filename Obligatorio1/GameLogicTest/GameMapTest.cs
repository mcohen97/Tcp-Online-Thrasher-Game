using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLogic;

namespace GameLogicTest
{
    [TestClass]
    public class GameMapTest
    {
        [TestMethod]
        public void NewGameMap()
        {
            GameMap gameMap = new GameMap();
            Assert.IsNotNull(gameMap);
        }
    }
}
