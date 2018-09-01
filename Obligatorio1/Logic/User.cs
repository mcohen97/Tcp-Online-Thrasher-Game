using System;

namespace Logic
{
    internal class User
    {

        private string nickname;
        public string Nickname { get { return nickname; } set { SetNickname(value); } }

        public string Path { get; private set; }

        public User(string aNickname, string aPath)
        {
            Nickname = aNickname;
            Path = aPath;
        }
        

        private void SetNickname(string aNickname)
        {
            if (String.IsNullOrWhiteSpace(aNickname)) {
                

            }
            nickname = aNickname;   
        }


    }
}