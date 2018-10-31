using DataAccessInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UsersLogic;

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
            
            lock (Users)
            {
                bool repeated = instance.Value.Users.Any(u => u.Nickname.Equals(newUser.Nickname));
                if (repeated)
                {
                    throw new UserAlreadyExistsException("Nickname already taken");
                }
            }
            instance.Value.Users.Add(newUser);
        }

        public User GetUser(string nickname)
        {
            User query;
            lock (Users) {
                query = TryGet(nickname);
            }
            return query;
        }

        public User TryGet(string nickname) {
            User query;
            try
            {
                query = instance.Value.Users.First(u => u.Nickname.Equals(nickname));
            }
            catch (InvalidOperationException)
            {
                throw new UserNotFoundException("User does not exist");
            }
            return query;
        }

        public ICollection<User> GetAllUsers()
        {
            return instance.Value.Users;
        }

        public void DeleteUser(string nickname)
        {
            User toDelete;
            try
            {
              toDelete = instance.Value.Users.First(u => u.Nickname.Equals(nickname));
              instance.Value.Users.Remove(toDelete);
            }
            catch (InvalidOperationException)
            {
                throw new UserNotFoundException("User does not exist");
            }
        }

        public void ModifyUser(string oldNickname, User toModify)
        {
            if (!Exists(oldNickname)) {
                throw new UserNotFoundException("User does not exist");
            }
            //Avoid changing nickname to one that already exists.
            if ((oldNickname != toModify.Nickname) && Exists(toModify.Nickname)) {
                throw new UserAlreadyExistsException("A user with than nickname already exists");
            }

            DeleteUser(oldNickname);
            AddUser(toModify);
        }

        private bool Exists(string nickName) {
            return instance.Value.Users.Any(u => u.Nickname.Equals(nickName));
        }
    }
}
