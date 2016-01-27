using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHttpBinding webBinding = new WebHttpBinding();

            var sHost = new ServiceHost(typeof(IntegrationService.IntegrationService));

            sHost.AddServiceEndpoint(typeof(IntegrationService.IIntegrationService), webBinding, "http://dev-03-dt:6060").EndpointBehaviors.Add(new WebHttpBehavior());

            sHost.Open();
            Console.ReadLine();
        }
    }
}
