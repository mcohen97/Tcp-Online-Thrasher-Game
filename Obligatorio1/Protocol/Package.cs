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

        public Header Header { get; private set; }
        public byte[] Data { get; private set; }

        public Package(string wholePackage) {
            Header = new Header(wholePackage);
        }
        public int DataLength()
        {
            return Header.DataLength - Header.HEADER_LENGTH;
        }

        public bool Command()
        {
            throw new NotImplementedException();
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
