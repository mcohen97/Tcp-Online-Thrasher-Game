using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using LogicExceptions;
namespace LogicTest
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void NickNameTest()
        {
            User fake = new User("nickname", "path");
            Assert.AreEqual(fake.Nickname,"nickname");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void EmptyUserNameTest() {
            User fake = new User("", "path");

        }

        [TestMethod]
        public void PathTest() {
            User fake = new User("nickname", "path");
            Assert.AreEqual(fake.Path, "path");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void InvalidPathTest() {
            User fake = new User("nickname", "");
        }

        [TestMethod]
        public void EqualsTest() {
            User fake1 = new User("nickname", "path1");
            User fake2 = new User("nickname", "path2");
            Assert.AreEqual(fake1, fake2);
        }

        [TestMethod]
        public void NotEqualsDifferentNicknamesTest() {
            User fake1 = new User("nickname", "path1");
            User fake2 = new User("nicknameee", "path2");
            Assert.AreNotEqual(fake1, fake2);
        }

        [TestMethod]
        public void NotEqualsOtherTypeTest() {
            User fake1 = new User("nickname", "path1");
            Assert.IsFalse(fake1.Equals(new object()));
        }

        [TestMethod]
        public void NotEqualsNullTest() {
            User fake1 = new User("nickname", "path1");
            Assert.IsFalse(fake1.Equals(null));
        }
    }
}
