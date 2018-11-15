using System;
using System.Collections.Generic;
using System.Configuration;
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
            var settings = new AppSettingsReader();
            string queueName = (string)settings.GetValue("MessageQueue", typeof(string));

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
            var settings = new AppSettingsReader();
            string webServerIp = (string)settings.GetValue("WebServerIp", typeof(string));
            int port = (int)settings.GetValue("WebServerPort", typeof(int));
            Uri httpBaseAddress = new Uri("http://"+webServerIp+":"+port+"/AdministrativeService");

            //Instantiate ServiceHost
            gameServiceHost = new ServiceHost(
                typeof(WebService),
                httpBaseAddress);

            //more powerfull binding
            BasicHttpBinding binding = new BasicHttpBinding();


            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
            binding.ReaderQuotas.MaxDepth = 64;

            binding.OpenTimeout = new TimeSpan(0, 0, 4);
            binding.CloseTimeout = new TimeSpan(0, 0, 4);
            binding.SendTimeout = new TimeSpan(0, 0, 4);
            binding.ReceiveTimeout = new TimeSpan(0, 0, 4);

            //Add Endpoint to Host
            gameServiceHost.AddServiceEndpoint(
                typeof(IWebService), binding, "");

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
