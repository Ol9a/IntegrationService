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

        [DataMember]
        public string DoljnostRukovod { get; set; }

        [DataMember]
        public string NameRukovod { get; set; }

        [DataMember]
        public string GroundsOf { get; set; }

    }

    [DataContract]
    public class AccountAddress
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid TypeId { get; set; }

        [DataMember]
        public string FullAddress { get; set; }
    }

    [DataContract]
    public class Account
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AlternativeName { get; set; }

        [DataMember]
        public string Code1C { get; set; }

        [DataMember]
        public string Object1C { get; set; }

        [DataMember]
        public AccountBillingInfo BillingInfo { get; set; }

        [DataMember]
        public AccountAddress JurAddress { get; set; }

        [DataMember]
        public AccountAddress PostAddress { get; set; }
    }

    [DataContract]
    public class Product
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Guid ActivityType1CId { get; set; }

        [DataMember]
        public Guid OfferingType1CId { get; set; }

        [DataMember]
        public Guid StavkaNDS { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Code1C { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public double Quantity { get; set; }

        [DataMember]
        public double Amount { get; set; }

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
        public double Amount { get; set; }

        [DataMember]
        public Account Account { get; set; }

        [DataMember]
        public List<Product> Products { get; set; }
    }
}
