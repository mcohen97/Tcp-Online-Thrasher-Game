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
        public ICollection<PlayerReportField> registers;
        public GameReport() {
            Date = DateTime.Now;
            registers = new List<PlayerReportField>();
        }
    }
}
