using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersLogic;

namespace DataAccessInterface
{
    public class UsersInMemory : IUserRepository
    {
        public static readonly Lazy<UsersInMemory> instance = new Lazy<UsersInMemory>(() => new UsersInMemory());
        private ICollection<User> Users;
        
        public readonly object synAddLock;

        private UsersInMemory()
        {
            Users = new List<User>();
            synAddLock = new object();
        }
        public void AddUser(User newUser)
        {
            
            bool repeated = instance.Value.Users.Any(u => u.Nickname.Equals(newUser.Nickname));
            lock (synAddLock)
            {
                if (repeated)
                {
                    throw new UserAlreadyExistsException("El nombre de usuario ya fue tomado");
                }
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
                throw new UserNotFoundException("El nombre de usuario no existe");
            }
            return query;
        }

        public ICollection<User> GetAllUsers()
        {
            return instance.Value.Users;
        }
    }
}
