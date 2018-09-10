using GameLogicException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public static class RoleMethods
    {
        public static bool[] GetBooleanArrayOfRoles()
        {
            int maxRoleEnumValue = (int)Enum.GetValues(typeof(Role)).Cast<Role>().Max();
            bool[] roles = new bool[maxRoleEnumValue + 1];
            for (int i = 0; i < roles.Length; i++)
            {
                roles[i] = false;
            }
            return roles;
        }

       
    }

    
}
