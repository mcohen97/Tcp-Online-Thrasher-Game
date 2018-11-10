using System;

namespace UserCRUDServiceContract
{
     [Serializable]
    public class PlayerFieldDto
    {
        public string PlayerName { get; set; }
        public string RolePlayed { get; set; }
        public bool Won { get; set; }
    }
}