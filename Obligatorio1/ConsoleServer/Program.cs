using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Network;


namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server();
            Thread listen = new Thread(new ThreadStart(() =>
            {
                s.ListenToRequests();
            }));
            listen.IsBackground = true;
            listen.Start();
            s.ServerManagement();     
        }
    }
}
