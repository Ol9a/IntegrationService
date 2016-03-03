using System;
using System.Configuration;

namespace IntegrationService
{

    public class IntegrationService : IIntegrationService
    {

        private string _oneSUser;
        private string _oneSPassword;
        private string _pathToDb;

        public IntegrationService()
        {
            _oneSUser = ConfigurationManager.AppSettings["1CUser"];
            _oneSPassword = ConfigurationManager.AppSettings["1CPassword"];
            _pathToDb = ConfigurationManager.AppSettings["pathToDb"];    
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
                using (var integrator = new Integrator(_oneSUser, _oneSPassword, _pathToDb))
                {
                    integrator.IntegrateInvoice(invoice);
                }
               
            }
            catch (Exception e)
            {
               // _integrator.Dispose();
                result.Message = e.Message;
                result.Status = "Error";
            }
            
            return result;
        }
    }
}
