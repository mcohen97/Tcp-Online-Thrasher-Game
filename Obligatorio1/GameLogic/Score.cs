using GameLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Score
    {
        public string PlayerName { get; set; }
        public Role PlayerRole { get; set; }
        public DateTime Date { get; set; }
        private int points;
        public int Points { get { return points; } set { SetPoints(value); } }

        private void SetPoints(int value)
        {
            if (value < 0) {
                throw new InvalidScoreException();
            }
            points = value;
        }
         
        public Score(Player player,int scored, DateTime gameDate) {
            PlayerName = player.Name;
            PlayerRole = player.Role;
            Points = scored;
            Date = gameDate;
        }
    }
}
