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
        private void Product(Invoice i) {
            string code1c=i.Products[0].Product.Code1C;
            object product=Open("Справочник.Номенклатура");
            var curproduct = FindByProperty(product, "Артикул", code1c);
            if (curproduct != null)
            {
                Set(product, "Артикул", code1c);
                Set(product, "Артикул", code1c);
                Set(product, "Наименование", i.Products[0].Product.Name);
                Set(product, "ПолнНаименование", i.Products[0].Product.FullName);
                Set(product, "ТипНоменклатуры", i.Products[0].Product.OfferingType.Code1C);
                var offering = FindByCode(product, i.Products[0].Product.ActivityType.Code1C);
                if (offering != null)
                {
                    Set(product, "ВидНоменклатуры", offering);
                }
                var unit = FindByProperty(product, "ПолнНаименование", i.Products[0].Product.Unit.Name);
                if (unit != null)
                {
                    Set(product, "ЕдиницаИзмерения", unit);
                }
                Save(product);

            }
            else
            {
                Create(product);
                SetCode(product);
                Set(product, "Артикул", code1c);
                Set(product, "Наименование", i.Products[0].Product.Name);
                Set(product, "ПолнНаименование", i.Products[0].Product.FullName);
                Set(product, "ТипНоменклатуры", i.Products[0].Product.OfferingType.Code1C);
                var offering = FindByCode(product, i.Products[0].Product.ActivityType.Code1C);
                if (offering != null)
                {
                    Set(product, "ВидНоменклатуры", offering);
                }
                var unit = FindByProperty(product, "ПолнНаименование", i.Products[0].Product.Unit.Name);
                if (unit != null)
                {
                    Set(product, "ЕдиницаИзмерения", unit);
                }


                Save(product);
            }
        
        }
        private void Account(Invoice i)
        {

            
             var code1c = i.Account.Code1C;
            object account = Open("Справочник.Контрагенты");
            var curaccount = FindByCode(account, code1c);
            if (curaccount != null)
            {


                Set(account, "Наименование", i.Account.Name);
                Set(account, "ПолнНаименование", i.Account.AlternativeName);
                //Set(account, "Телефоны", i.Account);
                Set(account, "ИНН", i.Account.BillingInfo.INN+'/'+i.Account.BillingInfo.KPP);
                Set(account, "ЮридическийАдрес", i.Account.JurAddress.FullAddress);
                Set(account, "ПочтовыйАдрес", i.Account.PostAddress.FullAddress);
                //Set(account, "ВидКонтрагента", i.Account.);

                Save(account);

            }
            else
            {
                Create(account);
                Set(account, "Код", code1c);
                Set(account, "Наименование", i.Account.Name);
                Set(account, "ПолнНаименование", i.Account.AlternativeName);
                //Set(account, "Телефоны", i.Account);
                Set(account, "ИНН", i.Account.BillingInfo.INN + '/' + i.Account.BillingInfo.KPP);
                Set(account, "ЮридическийАдрес", i.Account.JurAddress.FullAddress);
                Set(account, "ПочтовыйАдрес", i.Account.PostAddress.FullAddress);
                //Set(account, "ВидКонтрагента", i.Account.);

                Save(account);
            }

        }
        private void Invoice(Invoice i)
        {


            var code1c = i.Number;
            object invoice = Open("Документ.Счет");

            var curinvoice = FindByCode(invoice, code1c);
            if (curinvoice != null)
            {

                /*
                Set(invoice, "Наименование", i.invoice.Name);
                Set(invoice, "ПолнНаименование", i.invoice.AlternativeName);
                //Set(invoice, "Телефоны", i.invoice);
                Set(invoice, "ИНН", i.invoice.BillingInfo.INN + '/' + i.invoice.BillingInfo.KPP);
                Set(invoice, "ЮридическийАдрес", i.invoice.JurAddress.FullAddress);
                Set(invoice, "ПочтовыйАдрес", i.invoice.PostAddress.FullAddress);
                //Set(invoice, "ВидКонтрагента", i.invoice.);

                Save(invoice);
                */
            }
            else
            {
                Create(invoice);
                Set(invoice, "Код", code1c);
              //  Set(invoice, "ДатаДок", i.);
                Set(invoice, "НомерДок", i.Code1C);
              /*  Set(invoice, "ДатаДоговора", i.);
                Set(invoice, "НомерДоговора", i.invoice.BillingInfo.INN + '/' + i.invoice.BillingInfo.KPP);
                Set(invoice, "ЮридическийАдрес", i.invoice.JurAddress.FullAddress);
                Set(invoice, "ПочтовыйАдрес", i.invoice.PostAddress.FullAddress);
                //Set(invoice, "ВидКонтрагента", i.invoice.);

                Save(invoice);*/
            }

        }

        public void IntegrateInvoice(Invoice i)
        {
           Account(i);
           Invoice(i);
           Product(i);
            
            
            
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