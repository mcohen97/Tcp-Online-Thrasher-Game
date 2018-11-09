using GameLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class PlayerReportField
    {
        private string playerName;
        public string PlayerName { get {return playerName; } private set {SetName(value); } }

        public Role RolePlayed { get; private set; }
        public bool Won { get; private set; }

        public PlayerReportField(string aName, Role aRole, bool didWin) {
            PlayerName = aName;
            RolePlayed = aRole;
            Won = didWin;
        }

        private void SetName(string aName)
        {
            if (string.IsNullOrWhiteSpace(aName)) {
                throw new InvalidGameReportException();
            }
            playerName = aName;
        }
    }
}
