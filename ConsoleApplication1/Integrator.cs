using System;
using System.Reflection;
using System.Runtime.InteropServices;
namespace IntegrationService
{
    public class Integrator : IDisposable
    {
        private object _object1C;
        private Type _type1C;
       

        public Integrator(string login, string password, string pathToDb)
        {
            _type1C = Type.GetTypeFromProgID("v77.Application");
            _object1C = Activator.CreateInstance(_type1C);
            object rmtrade = _type1C.InvokeMember("RMTrade", BindingFlags.GetProperty, null, _object1C, null);
            var connectionString = String.Format("/D\"{0}\" /N\"{1}\" /P\"{2}\"", pathToDb, login, password);
            var arguments = new object[] { rmtrade, connectionString, "NO_SPLASH_SHOW" };

            bool res = (bool)_type1C.InvokeMember("Initialize", BindingFlags.InvokeMethod, null, _object1C, arguments);
        }


        private object Open(string name)
        {
            return _type1C.InvokeMember("CreateObject", BindingFlags.Public | BindingFlags.InvokeMethod, null, _object1C, new object[] { name });
        }

        private object Create(object obj)
        {
            return _type1C.InvokeMember(@"Новый", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { });
        }

        private void Save(object obj)
        {
            obj.GetType().InvokeMember(@"Записать", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, null); 
        }

        private object FindByName(object obj, string name)
        {
            double res = (double)_type1C.InvokeMember(@"НайтиПоНаименованию", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { name, "1" });
            if (res > 0)
            {
                var current = _type1C.InvokeMember("ТекущийЭлемент", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { });
                return current;
            }
            return null;
        }
        private object FindByCode(object obj, string code)
        {
            double res = (double)_type1C.InvokeMember(@"НайтиПоКоду", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { code, "1" });
            if (res > 0)
            {
                var current = _type1C.InvokeMember("ТекущийЭлемент", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { });
                return current;
            }
            return null;
        }
        private object FindByProperty(object obj, string attribute, string value)
        {
            double res = (double)_type1C.InvokeMember(@"НайтиПоРеквизиту", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { attribute,value, "1" });
            if (res > 0)
            {
                var current = _type1C.InvokeMember("ТекущийЭлемент", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { });
                return current;
            }
            return null;
        }
        private object FindByNumber(object obj, string number, DateTime date  )
        {
            double res = (double)_type1C.InvokeMember(@"НайтиПоНомеру", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { number, date, "1" });
            if (res > 0)
            {
                var current = _type1C.InvokeMember("ТекущийЭлемент", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { });
                return current;
            }
            return null;
        }
        private void Set(object obj, string attributeName, object value)
        {
            _type1C.InvokeMember("УстановитьАтрибут", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { attributeName, value });
        }

        private void OpenTable(object obj)
        {
            _type1C.InvokeMember("НоваяСтрока", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { });
        }
        private void SetCode(object obj) {
            _type1C.InvokeMember(@"УстановитьНовыйКод", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { "1" });
        }
        /// <summary>
        /// Product
        /// </summary>
        /// <param name="product1C"></param>
        /// <param name="p"></param>
        /// <param name="code1c"></param>
        private void UpdateProduct(object product1C, Product p, string code1c) 
        {
            Set(product1C, "Артикул", code1c);
            Set(product1C, "Наименование", p.Name);
            Set(product1C, "ПолнНаименование", p.FullName);
            Set(product1C, "ТипНоменклатуры", p.OfferingType.Code1C);
            var offering = FindByCode(product1C, p.ActivityType.Code1C);
            if (offering != null)
            {
                Set(product1C, "ВидНоменклатуры", offering);
            }
            var unit = FindByProperty(product1C, "ПолнНаименование", p.Unit.Name);
            if (unit != null)
            {
                Set(product1C, "ЕдиницаИзмерения", unit);
            }
            Save(product1C);
        }
        private void SyncProduct(Invoice i) 
        {
            string code1c=i.Products[0].Product.Code1C;
            object product=Open("Справочник.Номенклатура");
            var curproduct = FindByProperty(product, "Артикул", code1c);
            if (curproduct != null)
            {
                UpdateProduct(product, i.Products[0].Product, code1c);

            }
            else
            {
                Create(product);
                SetCode(product);
                UpdateProduct(product, i.Products[0].Product, code1c);
                
            }
        
        }
        /// <summary>
        /// Account
        /// </summary>
        /// <param name="account1C"></param>
        /// <param name="a"></param>
        /// <param name="code1c"></param>
        private void UpdateAccount(object account1C, Account a, string code1c) 
        {

            Set(account1C, "Наименование", a.Name);
            Set(account1C, "ПолнНаименование", a.AlternativeName);
            Set(account1C, "Телефоны", a.Phone);
            Set(account1C, "ИНН", a.BillingInfo.INN + '/' + a.BillingInfo.KPP);
            Set(account1C, "ЮридическийАдрес", a.JurAddress.FullAddress);
            Set(account1C, "ПочтовыйАдрес", a.PostAddress.FullAddress);
            Set(account1C, "ВидКонтрагента", a.Type);

            Save(account1C);
        
        }
        private void SyncAccount(Invoice i)
        {

            
            string code1c = i.Account.Code1C;
            object account = Open("Справочник.Контрагенты");
            var curaccount = FindByCode(account, code1c);
            if (curaccount != null)
            {
                UpdateAccount(account, i.Account, code1c);

            }
            else
            {
                Create(account);
                Set(account, "Код", code1c);
                UpdateAccount(account, i.Account, code1c);
            }
            SyncAccountBilling(i.Account.BillingInfo,curaccount, i.Account);
            SyncCalculateInvoice(i.Account.BillingInfo, curaccount, i.Account);
            /*
            object accbill = Open("Справочник.яКонтрагенты");

            var current = _type1C.InvokeMember("ИспользоватьВладельца", BindingFlags.Public | BindingFlags.InvokeMethod, null, accbill, new object[] { curaccount });
            if (current != null)
            {
                Create(accbill);
                UpdateAccountBilling(accbill, i.Account.BillingInfo, i.Account);
            }
            else { UpdateAccountBilling(accbill, i.Account.BillingInfo, i.Account); }
           
            */
        }
        private void SetOwner(object obj, Account a) 
        {
            _type1C.InvokeMember(@"ИспользоватьВладельца", BindingFlags.Public | BindingFlags.InvokeMethod, null, obj, new object[] { a });


        }
        private void UpdateAccountBilling(object acbill1C, AccountBillingInfo abi, object curaccount) 
        {
            Set(acbill1C, "Руководитель", abi.NameRukovod);
            Set(acbill1C, "Владелец", curaccount);
            Set(acbill1C, "ДолжностьРуководителя", abi.DoljnostRukovod);
            Set(acbill1C, "РуководительДействуетНаОсновании", abi.GroundsOf);
           // Set(acbill1C,"ОГРН",abi.)
            Save(acbill1C);
        
        }

        private void SyncAccountBilling( AccountBillingInfo abi,object curaccount, Account a) 
        {
            object accbill = Open("Справочник.яКонтрагенты");

            var current = _type1C.InvokeMember("ИспользоватьВладельца", BindingFlags.Public | BindingFlags.InvokeMethod, null, accbill, new object[] { curaccount });
            if (current != null)
            {
                Create(accbill);
                UpdateAccountBilling(accbill, abi, curaccount);
            }
            else { UpdateAccountBilling(accbill, abi, curaccount); }
        
        }
        private void UpdateCalculateInvoice(object acbil1C, AccountBillingInfo abi, object curaccount) 
        {
            Set(acbil1C, "Номер", abi.RS);
            Set(acbil1C, "Владелец", curaccount);
            Set(acbil1C, "Наименование", "Основной");
            Set(acbil1C, "БанкОрганизации", abi.Bank);
            SetCode(acbil1C);
        }
        private void SyncCalculateInvoice(AccountBillingInfo abi, object curaccount, Account a) 
        {
            object accbill = Open("Справочник.РасчетныеСчета");
            var current = _type1C.InvokeMember("ИспользоватьВладельца", BindingFlags.Public | BindingFlags.InvokeMethod, null, accbill, new object[] { curaccount });
            if (current != null)
            {
                Create(accbill);
                UpdateCalculateInvoice(accbill, abi, curaccount);
            }
            else { UpdateCalculateInvoice(accbill, abi, curaccount); }
        }


        /// <summary>
        /// Invoice
        /// </summary>
        /// <param name="invoice1C"></param>
        /// <param name="i"></param>
        private void UpdateInvoice(object invoice1C, Invoice i) 
        {
            Set(invoice1C, "ДатаДок", i.StartDate);
            Set(invoice1C, "НомерДок", i.Code1C);
            Set(invoice1C, "НомерДоговора", i.Contract.Number);
            Set(invoice1C, "ДатаДоговора", i.Contract.StartDate);
            var accountcode = i.Account.Code1C;
            object account = Open("Справочник.Контрагенты");
            var curaccount = FindByCode(account, accountcode);
            if (curaccount != null)
            {
                Set(invoice1C, "Контрагент", curaccount);
                Set(invoice1C, "Плательщик", curaccount);
            }
            
            Save(invoice1C);
        }
        private void SyncInvoice(Invoice i)
        {


            var code1c = i.Number;
            object invoice = Open("Документ.Счет");
            DateTime date = i.StartDate;
            var curinvoice = FindByNumber(invoice, code1c, date);
            if (curinvoice != null)
            {
                UpdateInvoice(invoice, i);
            }
            else
            {
                Create(invoice);
                UpdateInvoice(invoice, i);
              
            }

        }

        public void IntegrateInvoice(Invoice i)
        {
           SyncAccount(i);
           SyncProduct(i);
           SyncInvoice(i);
    
            
            
           // Dispose();
        }

        public void Dispose()
        {
            if (_object1C != null)
            {
                Marshal.FinalReleaseComObject(_object1C);
                _object1C = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}