using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;


namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Inicializando servidor...");
            Server s = new Server();
            s.HandleClients();
        }
    }
}
