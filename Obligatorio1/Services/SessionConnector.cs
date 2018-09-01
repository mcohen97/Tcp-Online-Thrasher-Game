using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using DataAccess;

namespace Logic
{
    public class SessionConnector
    {
        public Session LogIn(string nickname,  IUserRepository userStorage)
        {
            User userLogging = userStorage.GetUser(nickname);
            Session created = new Session(userLogging);
            return created;
        }
    }
}
