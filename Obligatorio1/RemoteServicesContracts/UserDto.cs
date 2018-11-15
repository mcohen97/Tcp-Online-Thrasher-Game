using System;

namespace UserCRUDService
{
    [Serializable]
    public class UserDto
    {
        public string Nickname { get; set; }
        public byte[] Photo { get; set; }
    }
}
