using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace DataAccess
{
    public class UsersInMemory : IUserRepository
    {
        public static readonly Lazy<UsersInMemory> instance = new Lazy<UsersInMemory>(() => new UsersInMemory());
        private ICollection<User> Users;

        private UsersInMemory()
        {
            Users = new List<User>();
        }
        public void AddUser(User newUser)
        {
            bool repeated = instance.Value.Users.Any(u => u.Nickname.Equals(newUser.Nickname));
            if (repeated) {
                throw new UserAlreadyExistsException();
            }
            instance.Value.Users.Add(newUser);
        }

        public User GetUser(string nickname)
        {
            User query;
            try
            {
                query = instance.Value.Users.First(u => u.Nickname.Equals(nickname));
            }
            catch (InvalidOperationException)
            {
                throw new UserNotFoundException();
            }
            return query;
        }

        public ICollection<User> GetAllUsers()
        {
            return instance.Value.Users;
        }
    }
}
