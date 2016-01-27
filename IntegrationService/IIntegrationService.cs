using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace IntegrationService
{

    [ServiceContract]
    public interface IIntegrationService
    {

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/invoice/{id}")]
        string IntegrateInvoice(string id, Invoice invoice);
       
    }


    

    [DataContract]
    public class AccountBillingInfo
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string RS { get; set; }

        [DataMember]
        public string Bank { get; set; }

        [DataMember]
        public string INN { get; set; }

        [DataMember]
        public string KPP { get; set; }

    }

    [DataContract]
    public class Account
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public AccountBillingInfo BillingInfo { get; set; }
    }
  
    [DataContract]
    public class Invoice
    {
        [DataMember]
        public Guid Id {get; set;}

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public string Object1C { get; set; }

        [DataMember]
        public string Code1C { get; set; }

        [DataMember]
        public string Amount { get; set; }

        [DataMember]
        public Account Account { get; set; }
    }
}
