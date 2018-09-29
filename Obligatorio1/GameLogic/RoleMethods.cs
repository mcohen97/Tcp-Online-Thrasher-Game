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

       public static string RoleToString(Role role)
        {
            string roleString = "";
            switch (role)
            {
                case Role.MONSTER:
                    roleString = "Monster";
                    break;
                case Role.SURVIVOR:
                    roleString = "Survivor";
                    break;
                case Role.NEUTRAL:
                    roleString = "Neutral";
                    break;
                default:
                    break;
            }

            return roleString;
        }
    }

    
}
