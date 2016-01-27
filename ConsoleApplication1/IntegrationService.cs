using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace IntegrationService
{

    public class IntegrationService : IIntegrationService
    {
        private Integrator _integrator;

        public IntegrationService()
        {
            _integrator = new Integrator("", "", "");
        }

        public string IntegrateInvoice(string id, Invoice invoice)
        {
            _integrator.IntegrateInvoice(invoice);
            return "";
        }
    }
}
