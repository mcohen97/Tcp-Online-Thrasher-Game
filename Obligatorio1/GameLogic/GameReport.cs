using GameLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class GameReport
    {
        public DateTime Date { get; private set; }
        public ICollection<PlayerReportField> registers { get; private set; }
        public GameReport() {
            Date = DateTime.Now;
            registers = new List<PlayerReportField>();
        }

        internal void AddPlayerField(PlayerReportField field)
        {
            if (registers.Any(r => r.PlayerName.Equals(field.PlayerName))) {
                throw new InvalidGameReportException();
            }
            registers.Add(field);
        }
    }
}
