using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

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
        Result IntegrateInvoice(string id, Invoice invoice);
       
    }


    [DataContract]
    public class Result
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public Guid InvoiceId { get; set; }
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
        public string BIK { get; set; }

        [DataMember]
        public string Code1C { get; set; }

        [DataMember]
        public string CorrInvoice { get; set; }

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
        public string Zip { get; set; }

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

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Type { get; set; }

    }

    [DataContract]
    public class ActivityType
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code1C { get; set; }

        [DataMember]
        public string Object1C { get; set; }
    }

    [DataContract]
    public class OfferingType
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string EnumCaption { get; set; }

        [DataMember]
        public string Code1C { get; set; }

        [DataMember]
        public string Object1C { get; set; }
    }

    [DataContract]
    public class Unit
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class Tax
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Percent { get; set; }
    }

    [DataContract]
    public class Product
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public ActivityType ActivityType { get; set; }

        [DataMember]
        public OfferingType OfferingType { get; set; }

        [DataMember]
        public Unit Unit { get; set; }

        [DataMember]
        public Tax Tax { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Code1C { get; set; }

        [DataMember]
        public string Object1C { get; set; }
    }

    [DataContract]
    public class InvoiceProduct
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Product Product { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public double Quantity { get; set; }

        [DataMember]
        public double TotalAmount { get; set; }

        [DataMember]
        public double TaxAmount { get; set; }

        [DataMember]
        public string Object1C { get; set; }
        
    }

    [DataContract]
    public class Contract
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public string Number { get; set; }
    }

    [DataContract]
    public class RS
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public string Korrespondent { get; set; }

        [DataMember]
        public string Destination { get; set; }

        [DataMember]
        public string Object1C { get; set; }

        [DataMember]
        public string Code1C { get; set; }

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
        public DateTime StartDate { get; set; }

        [DataMember]
        public RS RS { get; set; }

        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public Account Account { get; set; }

        [DataMember]
        public Contract Contract { get; set; }

        [DataMember]
        public List<InvoiceProduct> Products { get; set; }
    }
}
