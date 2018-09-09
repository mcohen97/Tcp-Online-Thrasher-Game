using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol
{
    public class Package
    {
        public static int MESSAGE_SIZE_MAX = 9999;
        public static int DATA_SIZE_MAX = MESSAGE_SIZE_MAX - Header.HEADER_LENGTH;

        public Header Header { get; set; }
        public byte[] Data { get; set; }

        public Package(string wholePackage) {
            Header = new Header(wholePackage);
            string dataPart = wholePackage.Substring(Header.HEADER_LENGTH, Header.DataLength);
            Data = Encoding.Default.GetBytes(dataPart);
        }

        public Package(Header aHeader) {
            Header = aHeader;
        }
        public int DataLength()
        {
            return Header.DataLength - Header.HEADER_LENGTH;
        }

        public CommandType Command()
        {
            return Header.Command;
        }

        public byte[] GetBytesToSend()
        {
            byte[] header = Header.GetBytes();

            if (Data != null)
            {
                return header.Concat(Data).ToArray();
            }
            else
            {
                return header;
            }
        }

        public void CalculateLengthIntoHeader()
        {
            int length = 0;
            if (Data != null)
            {
                length = Data.Length;
            }
            Header.DataLength = length + Header.HEADER_LENGTH;
        }

    }
}
