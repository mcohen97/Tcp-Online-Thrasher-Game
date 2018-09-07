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

        public enum HeaderEnum { REQUEST, RESPONSE }
        public HeaderEnum HeaderType { get; private set; }

        public enum CommandEnum { }
        public CommandEnum Command { get; private set; }

        public int DataLength { get; set; }

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
                this.HeaderType = HeaderEnum.REQUEST;
            }
            else if (typeStr.Equals("RES"))
            {
                this.HeaderType = HeaderEnum.RESPONSE;
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
            this.Command = (CommandEnum)commandInt;
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

            string ret = HeaderType == HeaderEnum.REQUEST ? "REQ" : "RES";
            ret += ((int)Command).ToString(commandFormat);
            ret += DataLength.ToString(lengthFormat);

            return System.Text.Encoding.UTF8.GetBytes(ret);
        }
    }


}

