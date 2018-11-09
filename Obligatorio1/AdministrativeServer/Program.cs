using System;
using System.Collections.Generic;
using UserCRUDService;

namespace AdministrativeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            AdministrativeServer admin = new AdministrativeServer();
            admin.RunServer();
            Console.ReadLine();

        }
    }
}
