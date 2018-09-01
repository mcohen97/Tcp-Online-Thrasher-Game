using System;
using LogicExceptions;

namespace Logic
{
    internal class User
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
                throw new InvalidUserDataException("Invalid nickname");
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


    }
}