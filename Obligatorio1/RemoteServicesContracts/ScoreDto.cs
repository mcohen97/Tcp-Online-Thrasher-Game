using GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreService
{
    public class ScoreDto
    {
        public string UserNickname { get; set; }
        public DateTime Date { get; set; }
        public int Points { get; set; }
        public Role RolePlayed { get; set; }
    }
}
