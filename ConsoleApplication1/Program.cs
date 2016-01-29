using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace IntegrationService
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpoint = ConfigurationManager.AppSettings["Endpoint"];

            WebHttpBinding webBinding = new WebHttpBinding();

            var sHost = new ServiceHost(typeof(IntegrationService));

            sHost.AddServiceEndpoint(typeof(IIntegrationService), webBinding, endpoint).EndpointBehaviors.Add(new WebHttpBehavior());

            sHost.Open();
            Console.ReadLine();
        }
    }
}
