using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersLogic;

namespace DataAccessInterface
{
    public interface IUserRepository
    {
        void AddUser(User newUser);

        User GetUser(string nickname);

        ICollection<User> GetAllUsers();
    }
}
