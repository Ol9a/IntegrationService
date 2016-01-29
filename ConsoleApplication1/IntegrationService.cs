using System;
using System.Configuration;

namespace IntegrationService
{

    public class IntegrationService : IIntegrationService
    {
        private readonly Integrator _integrator;

        public IntegrationService()
        {
            var oneSUser = ConfigurationManager.AppSettings["1CUser"];
            var oneSPassword = ConfigurationManager.AppSettings["1CPassword"];
            var pathToDb = ConfigurationManager.AppSettings["pathToDb"];

            _integrator = new Integrator(oneSUser, oneSPassword, pathToDb);
        }

        public Result IntegrateInvoice(string id, Invoice invoice)
        {
            var result = new Result
            {
                Message = "",
                Status = "OK",
                InvoiceId = invoice.Id
            };
            try
            {
                _integrator.IntegrateInvoice(invoice);
            }
            catch (Exception e)
            {

                result.Message = e.Message;
                result.Status = "Error";
            }
            
            return result;
        }
    }
}
