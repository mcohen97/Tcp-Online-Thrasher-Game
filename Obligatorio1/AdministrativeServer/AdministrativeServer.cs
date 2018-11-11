using System;
using System.Collections.Generic;
using System.Messaging;
using System.ServiceModel;
using System.ServiceModel.Description;
using UserCRUDService;

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
                Console.WriteLine("There is an issue with the server" + ex.Message);
                gameServiceHost.Abort();

            }
            catch (GameServiceException ex) {
                Console.WriteLine("There is an issue with the server" + ex.Message);
                gameServiceHost.Abort();
            }

        }

        private void SetUpService()
        {
            //Base Address for StudentService
            Uri httpBaseAddress = new Uri("http://localhost:4321/AdministrativeService");

            //Instantiate ServiceHost
            gameServiceHost = new ServiceHost(
                typeof(WebService),
                httpBaseAddress);

            /*WSHttpBinding binding = new WSHttpBinding();
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;*/
            BasicHttpBinding bind = new BasicHttpBinding();
            bind.MaxBufferSize = 2147483647;
            bind.MaxReceivedMessageSize = 2147483647;
            bind.MaxBufferPoolSize = 2147483647;
            bind.ReaderQuotas.MaxBytesPerRead = 2147483647;
            bind.ReaderQuotas.MaxArrayLength = 2147483647;
            bind.ReaderQuotas.MaxStringContentLength = 2147483647;
            bind.ReaderQuotas.MaxNameTableCharCount = 2147483647;
            bind.ReaderQuotas.MaxDepth = 64;

            //Add Endpoint to Host
            gameServiceHost.AddServiceEndpoint(
                typeof(IWebService), bind, "");

            //Metadata Exchange
            ServiceMetadataBehavior serviceBehavior =
                new ServiceMetadataBehavior();
            serviceBehavior.HttpGetEnabled = true;
            gameServiceHost.Description.Behaviors.Add(serviceBehavior);

            //Open
            gameServiceHost.Open();
            Console.WriteLine("Service is live now at: {0}", httpBaseAddress);
        }
       

    }
}
