using System;
using System.Collections.Generic;
using UserABM;

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
            admin.ModifyFakeUser();
            Console.WriteLine("Se modifico");
            Console.ReadLine();
            admin.DeleteFakeUser();
            Console.WriteLine("Se borro");
            Console.ReadLine();
            admin.AddFakeUser();
            Console.WriteLine("Se agrego");
            Console.WriteLine("Restantes");
            ICollection<UserDto> dtos =admin.GetAll();
            foreach (UserDto dto in dtos) {
                Console.WriteLine(dto.nickname);
            }
            Console.ReadLine();

        }
    }
}
