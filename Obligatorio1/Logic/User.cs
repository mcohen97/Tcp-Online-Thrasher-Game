using System;
using LogicExceptions;
using Protocol;

namespace Logic
{
    public class User
    {

        private string nickname;
        public string Nickname { get { return nickname; } set { SetNickname(value); } }

        private string path;
        public string Path { get { return path; }  set { SetPath(value); } }

        public User(string aNickname, string aPath)
        {
            Nickname = aNickname;
            Path = aPath;
        }
        
        private void SetNickname(string aNickname)
        {
            if (String.IsNullOrWhiteSpace(aNickname)) {
                throw new InvalidUserDataException("Nickname invalido");
            }
            if (aNickname.Contains(Package.LIST_SEPARATOR_SIMBOL)) {
                throw new InvalidUserDataException("El nickname no puede contener el caracter ';'");
            }
            nickname = aNickname;   
        }

        private void SetPath(string aPath)
        {
            if (String.IsNullOrWhiteSpace(aPath))
            {
                throw new InvalidUserDataException("Invalid photo path");
            }
            path = aPath;
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