﻿using System;
using System.Collections.Generic;
using System.Messaging;
using System.ServiceModel;
using System.ServiceModel.Description;
using UserABM;

namespace AdministrativeServer
{
    public class AdministrativeServer
    {
        ServiceHost gameServiceHost;

        public AdministrativeServer() {
            CreateQueueIfNotExists();
        }

        private void CreateQueueIfNotExists()
        {
            string queueName = @".\private$\LogServer";

            if (!MessageQueue.Exists(queueName))
            {
                MessageQueue.Create(queueName);
            }
        }

        public void RunServer()
        {
            try
            {
                SetUpService();
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine("There is an issue with Game Service" + ex.Message);
                gameServiceHost.Abort();

            }

        }

        private void SetUpService()
        {
            //Base Address for StudentService
            Uri httpBaseAddress = new Uri("http://localhost:4321/StudentService");

            //Instantiate ServiceHost
            gameServiceHost = new ServiceHost(
                typeof(WebService),
                httpBaseAddress);

            //Add Endpoint to Host
            gameServiceHost.AddServiceEndpoint(
                typeof(IWebService), new WSHttpBinding(), "");

            //Metadata Exchange
            ServiceMetadataBehavior serviceBehavior =
                new ServiceMetadataBehavior();
            serviceBehavior.HttpGetEnabled = true;
            gameServiceHost.Description.Behaviors.Add(serviceBehavior);

            //Open
            gameServiceHost.Open();
        }
        /* internal void ModifyFakeUser()
{
   UserDto fake = new UserDto { nickname = "richard", photoPath = "test" };
    remoteUserStorage.ModifyUser("test", fake);
}

internal void DeleteFakeUser()
{
    remoteUserStorage.DeleteUser("richard");
}

internal ICollection<UserDto> GetAll()
{
    return remoteUserStorage.GetAllUsers();
}

public void AddFakeUser() {
    UserDto fake = new UserDto { nickname = "test", photoPath = "test" };
    remoteUserStorage.AddUser(fake);
}*/

    }
}
