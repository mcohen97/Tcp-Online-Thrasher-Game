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

        private Header header;

        private byte[] data;
        public byte[] Data { get {return data; } set { SetData(value); } }

        public Package(Header info, string message) {
            /*byte[] data = Encoding.Default.GetBytes(wholePackage.Substring(0,Header.HEADER_LENGTH));
            header = new Header(wholePackage);
            string dataPart = wholePackage.Substring(Header.HEADER_LENGTH, header.DataLength);
            Data = Encoding.Default.GetBytes(dataPart);*/
            header = info;
            Data = Encoding.Default.GetBytes(message);
        }

        public Package(Header aHeader) {
            header = aHeader;
        }

        public string Message()
        {
            return Encoding.Default.GetString(data);
        }

        private void SetData(byte[] message)
        {
            header.DataLength = message.Length;
            data=message;
        }
        public HeaderType HeaderType() {
            return header.Type;
        }
        public int DataLength()
        {
            return header.DataLength - Header.HEADER_LENGTH;
        }

        public CommandType Command()
        {
            return header.Command;
        }

        public byte[] GetBytesToSend()
        {
            byte[] header = this.header.GetBytes();

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
            header.DataLength = length + Header.HEADER_LENGTH;
        }

    }
}
