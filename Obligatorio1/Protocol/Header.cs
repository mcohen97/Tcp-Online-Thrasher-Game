using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol
{
    public class Header
    {
        public static readonly int HEADER_LENGTH = 9;

        
        public HeaderType Type { get; set; }
       
        public CommandType Command { get; set; }

        public int DataLength { get; set; }

        public Header() {
        }
        public Header(string headerString)
        {
            GetInfoFromString(headerString);
        }

        private void GetInfoFromString(string str)
        {
            ExtractHeaderType(str);
            ExtractCommand(str);
            ExtractDataLength(str);
        }

        private void ExtractHeaderType(string str)
        {
            var typeStr = str.Substring(0, 3);

            if (typeStr.Equals("REQ"))
            {
                this.Type = HeaderType.REQUEST;
            }
            else if (typeStr.Equals("RES"))
            {
                this.Type = HeaderType.RESPONSE;
            }
            else
            {
                throw new Exception("Error.Header type not valid!.");
            }
        }

        private void ExtractCommand(string str)
        {
            string commadString = str.Substring(3, 2);
            int commandInt = int.Parse(commadString);
            this.Command = (CommandType)commandInt;
        }
        private void ExtractDataLength(string str)
        {
            var lengthStr = str.Substring(5, 4);
            this.DataLength = int.Parse(lengthStr);
        }

        public byte[] GetBytes()
        {
            string commandFormat = "00";
            string lengthFormat = "0000";

            string ret = Type == HeaderType.REQUEST ? "REQ" : "RES";
            ret += ((int)Command).ToString(commandFormat);
            ret += DataLength.ToString(lengthFormat);

            return System.Text.Encoding.UTF8.GetBytes(ret);
        }
    }


}

