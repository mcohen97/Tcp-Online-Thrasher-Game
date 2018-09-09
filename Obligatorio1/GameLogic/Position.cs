using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Position
    {
        private int row;
        private int column;

        public int Row {
            get { return row; }
            set { row = value; }
        }

        public int Column {
            get { return column; }
            set { column = value; }
        }

        public Position(int row, int column)
        {
            this.row = row;
            this.column = column;
        }


        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Position)) return false;

            bool result = false;
            Position parameter = (Position)obj;
            if (parameter.Row == this.Row && parameter.Column == this.Column)
            {
                result = true;
            }

            return result;
        }

    }
}
