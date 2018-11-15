using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesInfoService
{
    [Serializable]
    public class ScoreDto
    {
        public string UserNickname { get; set; }
        public DateTime Date { get; set; }
        public int Points { get; set; }
        public string RolePlayed { get; set; }
    }
}
