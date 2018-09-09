using GameLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicTest
{
    [TestClass]
    public class RoleMethodsTest
    {
        [TestMethod]
        public void GetRolesTest()
        {
            bool[] roles = RoleMethods.GetBooleanArrayOfRoles();
            int maxRoleEnumValue = (int)Enum.GetValues(typeof(Role)).Cast<Role>().Max();
            Assert.AreEqual(maxRoleEnumValue + 1, roles.Length);
        }
    }
}
