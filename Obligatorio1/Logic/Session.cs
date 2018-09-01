using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Session
    {
        public User UserLogged { get; }

        public Session(User aUser)
        {
            UserLogged = aUser;
        }

    }
}
