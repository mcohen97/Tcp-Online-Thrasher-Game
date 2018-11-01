using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ConsoleAccess
    {
        public readonly object consoleLock;

        public ConsoleAccess()
        {
            consoleLock = new object();
        }

        public void Write(string text)
        {
            lock (consoleLock)
            {
                Console.Write(text);
            }
        }

        public void WriteLine(string text)
        {
            lock (consoleLock)
            {
                Console.WriteLine(text);
            }
        }

        public void Clear()
        {
            lock (consoleLock)
            {
                Console.Clear();
            }
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
