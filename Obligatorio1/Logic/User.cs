using System;
using LogicExceptions;
using Protocol;
using System.Drawing;

namespace Logic
{
    public class User
    {

        private string nickname;
        public string Nickname { get { return nickname; } set { SetNickname(value); } }

        private byte[] photo;
        public byte[] Photo { get { return photo; }  set { SetPhoto(value); } }

        public User(string aNickname, byte[] aPhoto)
        {
            Nickname = aNickname;
            Photo = aPhoto;
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

        private void SetPhoto(byte[] aPhoto)
        {
            photo = aPhoto;
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