using System;

namespace AdministrativeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            AdministrativeServer admin = new AdministrativeServer();
            admin.AddFakeUser();
            Console.WriteLine("Se agrego");
            Console.ReadLine();
        }
    }
}
