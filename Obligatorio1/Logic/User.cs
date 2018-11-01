using System;
using LogicExceptions;
using Protocol;
using System.Drawing;

namespace UsersLogic
{
    public class User
    {

        private string nickname;
        public string Nickname { get { return nickname; } set { SetNickname(value); } }

        public string Path { get;  set; }

        public User(string aNickname) {
            Nickname = aNickname;
            Path = string.Empty;
        }
        public User(string aNickname, string aPath):this(aNickname)
        {
            Path = aPath;
        }
        
        private void SetNickname(string aNickname)
        {
            if (String.IsNullOrWhiteSpace(aNickname)) {
                throw new InvalidUserDataException("Invalid nickname");
            }
            if (aNickname.Contains(Package.LIST_SEPARATOR_SIMBOL)) {
                throw new InvalidUserDataException("The nickname can't contain the character ';'");
            }
            nickname = aNickname;   
        }

        public override bool Equals(object obj)
        {
            bool equals;
            if (obj == null || !obj.GetType().Equals(GetType()))
            {
                equals = false;
            }
            else {
                User known = (User)obj;
                equals = Nickname.Equals(known.Nickname);
            }
            return equals;
        }

        public override string ToString()
        {
            return nickname;
        }
    }
}