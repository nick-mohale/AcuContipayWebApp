using AcuContiPay;
using Newtonsoft.Json;
using PX.Api;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.BQLConstants;
using PX.Objects.CA;
using PX.Objects.CR;
using PX.Objects.SO;
using PX.Objects.SO.GraphExtensions.SOInvoiceEntryExt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using static PX.Objects.TX.CSTaxCalcType;

namespace AcuContiPaySSP
{
    public class AcuContiPayEntry : PXGraph<AcuContiPayEntry>
    {

        #region Declarations
        public class constContiPay : BqlString.Constant<constContiPay>
        {
            public constContiPay() : base("CONTIPAY") { }
        }
        string cpToken = string.Empty;
        bool completeSuccess = false;
        string merchRef = string.Empty;

        bool tranExists = false;
        Helpers helpers = new Helpers();
        //public PXSave<SOContipay> Save;
        public PXCancel<SOContipay> Cancel;

        public PXFilter<SOContipay> MasterView;
        public PXFilter<SOContipay> DetailsView;

        public PXSelect<CP_Log> logs;

        //12-Oct-23
        //Add Approved filter
        public SelectFrom<SOOrder>.
                Where<SOOrder.paymentMethodID.IsEqual<constContiPay>.
                    // 28/12 - Yama, relax Approved isTrue
                    And<SOOrder.approved.IsEqual<True>>>.
                        OrderBy<Desc<SOOrder.orderNbr>>.View OrdersView;

        //public SelectFrom<Users>.
        //           Where<Users.username.
        //               IsEqual<AccessInfo.userName.FromCurrent>>.View UserView;

        #endregion

        #region Actions
        public PXAction<SOOrder> UseSelected;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Use...", Enabled = true)]
        protected virtual void useSelected()
        {
            SOOrder row = OrdersView.Current;
            SOContipay conti = MasterView.Current;
            conti.OrderType = row.OrderType;
            conti.SOOrderNbr = row.OrderNbr;
            conti.Curyid = row.CuryID;
            conti.CuryAmt = row.CuryUnpaidBalance;
            clearFields();

        }

        public PXAction<SOContipay> PayContiPay;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Pay...", Enabled = false)]
        protected virtual void payContiPay()
        {
            SOContipay row = MasterView.Current;
            if (row == null) return;
            if (!checkIfFieldsCaptured(checkIfCardtran(row), row)) return;
            cpToken = merchRef = string.Empty;
            ProcessPayment(row);
            if (cpToken != string.Empty)
            {
                if (completeSuccess)
                {
                    ProcessSoPayment(merchRef);
                    if (!checkIfCardtran(row))
                    {
                        string msg = "EMA: ContiPay - Payment Authorisation";
                        string url = $"https://verify-uat.contipay.co.zw/acquire/complete?redirect-token=";
                        //string token = "WmQ0dWVodExMTTBoTXhQeTBTTjJnZ2g3L2VzY20yN0dpc0RqMGhpYm1pVHV4TlAwK2lzQzU4OUZjYW41cG1DSm5QMk9HT3NWTjN2VWtxUFhHaURqek1XMHZ5Y3p6ZnA3dS9zZjFuc0NQbGxGa21pendJLzVGZU1Pd3d0bGlqM29SMXh1U1FBZG91RDBvcEt4OVBFdTVFR09YR3BoNzFiRnJVblFYNlZZMmpFS3VZMnpOYmU4SmsrL0g5OUxEWkR3UXArbFdKNy9WZXR2R0dGdkdSdFFiYUEzbEo3OS85VFpSa3VnR1E2eTVDSm5tZ1c0ZTFqN3JBbExWVzRFZFZ6QXJpYzdWR2d4L1dJZ0lyc0ZWY3hnMXh0cG80am5RL1BRaEczR003NStMN0pCVVFtaTVpMXhnRGlhRzB0NXN3MGh6aEtEa0hOR1l0anBEVE1LemYvbVJEN29YTUZZaFM4NXZFNytYZHBJaFZQRXk0Y2h0ZW1uc3ExbzlEaDJHK0lJM3N2TGxGYW1HQVp6Vm42Tlc5d0U4Njk1eFUwMFVaWUVlVEtjVldlQ1V3WjE0ZDVlMVNscElRWC96b0QxcnQ2ckdFZlhtc0FvbzdlSVoyeVR2cHVhODgrM3k3Q3dOdlNndHNZRkVMQ0p3dWd4bVhEYUZqNTF6TlRyR2xiZ0JDcTdiaDhZZ3JPWjJXSzFwelNJNC9LdnEwYVR5Z080MG1TQXJkTmd1WTZrYmxvVkRPWkNLbUsvLzJFZzVYajdrbWtJWGZKNHZ2enVyVGFBT2h4aW93dlNRUUdtWmZWSHFIY2MrbVprNDRNUG1NTGpUNkVZM2ZtWk1IUHNiZ05YdUZzdDZQa0Q2VUV4WUJWZU5Pck9BUmVGMjlMMmNuLzhPOEkvUzFHNVVMZ0xyVEI5V3htVGI0L2UvSTdIanlPZVVHTVlINlhJSHd0bjhVRXd2aDFObGdLZG5BPT0 |";
                        throw new PXRedirectToUrlException(url + cpToken, PXBaseRedirectException.WindowMode.NewWindow, msg);
                    }
                }
            }
        }

        public string GetUrl(string sourceStr)
        {

            return sourceStr.Substring(sourceStr.LastIndexOf("=") + 1); ;
        }

        public string GetExtRefNbr()
        {
            string merchantRefNbr;
            SOOrder order = OrdersView.Current;
            merchantRefNbr = order.CustomerRefNbr;
            if (merchantRefNbr == null)
            {
                merchantRefNbr = order.OrderType + "-" + order.OrderNbr + "-" + "00";
            }
            else
            {
                string extRefNbr = merchantRefNbr.Split('-').Last();
                if (extRefNbr == null)
                {
                    merchantRefNbr = order.OrderType + "-" + order.OrderNbr + "-" + "00";
                }
                else
                {
                    //Extract Suffix, convert to number, increment Suffix by 1
                    int intRefNbr = int.Parse(extRefNbr) + 1;
                    //connvert to string - change format to XX
                    merchantRefNbr = order.OrderType + "-" + order.OrderNbr + "-" + intRefNbr.ToString("##");
                }
            }
            try
            {
                SOOrderEntry sOrdEntry = PXGraph.CreateInstance<SOOrderEntry>();

                var orderType = order.OrderType;
                var orderNbr = order.OrderNbr;

                sOrdEntry.Document.Current = sOrdEntry.Document.Search<SOOrder.orderNbr>(orderNbr, orderType); ;
                order.CustomerRefNbr = merchantRefNbr;
                sOrdEntry.Document.Update(order);

                sOrdEntry.Actions.PressSave();
            }
            catch (Exception ex)
            {
                int x = 0;
            }
            return merchantRefNbr;
        }

        private void openUrl(string token)
        {
            //Authenticate ContiPay using provided token
            string msg = "EMA: ContiPay - Payment Authorisation";
            string url = $"https://verify-uat.contipay.co.zw/acquire/complete?redirect-token=";
            //string token = "WmQ0dWVodExMTTBoTXhQeTBTTjJnZ2g3L2VzY20yN0dpc0RqMGhpYm1pVHV4TlAwK2lzQzU4OUZjYW41cG1DSm5QMk9HT3NWTjN2VWtxUFhHaURqek1XMHZ5Y3p6ZnA3dS9zZjFuc0NQbGxGa21pendJLzVGZU1Pd3d0bGlqM29SMXh1U1FBZG91RDBvcEt4OVBFdTVFR09YR3BoNzFiRnJVblFYNlZZMmpFS3VZMnpOYmU4SmsrL0g5OUxEWkR3UXArbFdKNy9WZXR2R0dGdkdSdFFiYUEzbEo3OS85VFpSa3VnR1E2eTVDSm5tZ1c0ZTFqN3JBbExWVzRFZFZ6QXJpYzdWR2d4L1dJZ0lyc0ZWY3hnMXh0cG80am5RL1BRaEczR003NStMN0pCVVFtaTVpMXhnRGlhRzB0NXN3MGh6aEtEa0hOR1l0anBEVE1LemYvbVJEN29YTUZZaFM4NXZFNytYZHBJaFZQRXk0Y2h0ZW1uc3ExbzlEaDJHK0lJM3N2TGxGYW1HQVp6Vm42Tlc5d0U4Njk1eFUwMFVaWUVlVEtjVldlQ1V3WjE0ZDVlMVNscElRWC96b0QxcnQ2ckdFZlhtc0FvbzdlSVoyeVR2cHVhODgrM3k3Q3dOdlNndHNZRkVMQ0p3dWd4bVhEYUZqNTF6TlRyR2xiZ0JDcTdiaDhZZ3JPWjJXSzFwelNJNC9LdnEwYVR5Z080MG1TQXJkTmd1WTZrYmxvVkRPWkNLbUsvLzJFZzVYajdrbWtJWGZKNHZ2enVyVGFBT2h4aW93dlNRUUdtWmZWSHFIY2MrbVprNDRNUG1NTGpUNkVZM2ZtWk1IUHNiZ05YdUZzdDZQa0Q2VUV4WUJWZU5Pck9BUmVGMjlMMmNuLzhPOEkvUzFHNVVMZ0xyVEI5V3htVGI0L2UvSTdIanlPZVVHTVlINlhJSHd0bjhVRXd2aDFObGdLZG5BPT0 |";
            throw new PXRedirectToUrlException(url + token, PXBaseRedirectException.WindowMode.NewWindow, msg);
        }

        public PXAction<SOOrder> openLink;

        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Open Link", Visible = true)]
        //[PXButton(CommitChanges = true, DisplayOnMainToolbar = true), PXUIField(DisplayName = "Open Link", MapEnableRights = PXCacheRights.Select, Visible = true)]
        protected virtual IEnumerable OpenLink(PXAdapter adapter)
        {
            // You should encode any part of the URL that could be invalid if used directly
            string url = $"https://en.wikipedia.org/w/index.php?title=" + Uri.EscapeDataString("Acumatica") + "&param2=" + Uri.EscapeDataString("Another parameter");

            throw new PXRedirectToUrlException(url, "App");

        }
        #endregion

        #region Event Handlers
        protected void _(Events.RowSelected<SOContipay> e)
        {
            SOContipay row = e.Row;

            //Set default method for this screen to "CONTIPAY"
            PaymentMethod paymentMethod = SelectFrom<PaymentMethod>.
                                            Where<PaymentMethod.paymentMethodID.
                                                IsEqual<constContiPay>>.View.Select(this);
            row.PaymentMethodID = paymentMethod.PMInstanceID;
            row.OrderType = "SO";

            setEnabled(e.Cache, row);

        }

        protected void _(Events.FieldUpdated<SOContipay, SOContipay.providerID> e)
        {
            SOContipay row = e.Row;
            if (row == null) return;
            setEnabled(e.Cache, row);
            //clearFields(e.Cache, row);
        }

        protected void _(Events.FieldUpdated<SOContipay, SOContipay.accountNbr> e)
        {
            SOContipay row = e.Row;
            if (row == null) return;

            if (!checkIfCardtran(row))
                row.SmsNbr = row.AccountNbr;

            //19/10-setPayContiPayEnabled(checkIfCardtran(row), row);
        }

        protected void _(Events.FieldUpdated<SOContipay, SOContipay.accountName> e)
        {
            //19/10-setPayContiPayEnabled(checkIfCardtran(e.Row), e.Row);
        }

        protected void _(Events.FieldUpdated<SOContipay, SOContipay.code> e)
        {
            //19/10-setPayContiPayEnabled(checkIfCardtran(e.Row), e.Row);
        }

        protected void _(Events.FieldUpdated<SOContipay, SOContipay.expiry> e)
        {
            //19/10-setPayContiPayEnabled(checkIfCardtran(e.Row), e.Row);
        }
        #endregion


        public class Helpers
        {

        }
        #region Custom Methods
        // Helper method to get CPProvider
        public CPProvider GetProvider(int? providerID)
        {
            return SelectFrom<CPProvider>
                    .Where<CPProvider.providerID.IsEqual<PX.Data.BQL.@P.AsInt>>.View
                    .Select(this, providerID);
        }

        // Helper method to get CPSetup
        public CPSetup GetCPSetup()
        {
            return SelectFrom<CPSetup>.View.Select(this);
        }

        private void clearHeader()
        //private void clearFields(PXCache cache, SOContipay row)
        {
            SOContipay row = MasterView.Current; ;
            row.SOOrderNbr = string.Empty;
            row.CuryInfoID = null;
            row.CuryAmt = null;
        }

        private void clearFields()
        //private void clearFields(PXCache cache, SOContipay row)
        {
            SOContipay row = MasterView.Current; ;
            row.ProviderID = null;
            row.AccountNbr = row.AccountName = row.Code = row.Expiry = row.SmsNbr = row.contiRefNbr = string.Empty;
            row.StatusID = 99;
            cpToken = string.Empty;
        }

        private void setEnabled(PXCache cache, SOContipay row)
        {
            bool enableCardCols = checkIfCardtran(row);

            PXUIFieldAttribute.SetEnabled<SOContipay.code>(cache, row, enableCardCols);
            PXUIFieldAttribute.SetEnabled<SOContipay.expiry>(cache, row, enableCardCols);
            PXUIFieldAttribute.SetEnabled<SOContipay.smsNbr>(cache, row, !enableCardCols);
            setPayContiPayEnabled(enableCardCols, row);
        }

        private void setPayContiPayEnabled(bool cardPayment, SOContipay row)
        {
            bool fieldsCompleted = checkIfFieldsCaptured(cardPayment, row);
            PayContiPay.SetEnabled(fieldsCompleted);
        }

        private bool checkIfCardtran(SOContipay row)
        {
            CPProvider provider = GetProvider(row.ProviderID);
            return provider?.Code.ToUpper() == "MA" || provider?.Code.ToUpper() == "VA";
        }

        private bool checkIfFieldsCaptured(bool cardPayment, SOContipay row)
        {
            bool fieldsCompleted =
                row.ProviderID != null &&
                !string.IsNullOrEmpty(row.SOOrderNbr) &&
                !string.IsNullOrEmpty(row.Curyid) &&
                row.CuryAmt != null &&
                !string.IsNullOrEmpty(row.AccountNbr);
            //&& !string.IsNullOrEmpty(row.AccountName);

            if (cardPayment)
            {
                fieldsCompleted = fieldsCompleted &&
                    !string.IsNullOrEmpty(row.Code) &&
                    !string.IsNullOrEmpty(row.Expiry);
            }
            return fieldsCompleted;
        }

        #region Process Payment Methods
        private string ProcessPayment(SOContipay row)
        {
            try
            {
                //Get ContiPay Setup
                CPSetup setup = GetCPSetup();
                if (setup == null) return string.Empty;
                #region variables
                string actionUrl = "/acquire/payment";
                int? merchantId = setup.MerchantID;
                merchRef = GetExtRefNbr();
                //29/12 - Yama, suffix as per 
                string endpointUrl = setup.Baseurl;
                var authKey = setup.AuthKey;
                string authSecret = setup.AuthSecret.Replace("Password@", "");

                //GetTran(endpointUrl, actionUrl,
                //                merchantId, merchantRef,
                //                    authKey, authSecret);
                if (tranExists)
                {

                }
                else
                {
                    CPProvider provider = GetProvider(row.ProviderID);
                    Task<HttpResponseMessage> taskAquirePayment = Task.Run(async () => await
                                                                    AcquirePayment(row, setup, provider,
                                                                        endpointUrl, actionUrl,
                                                                            merchantId, merchRef,
                                                                                authKey, authSecret).ConfigureAwait(false));
                }


                #endregion
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        //private async void GetTran(string endpointUrl, string actionUrl, int? merchantId, string merchantRef, string authKey, string authSecret, ref bool tranExists)
        //private  void GetTran(string endpointUrl, string actionUrl, int? merchantId, string merchantRef, string authKey, string authSecret)
        //{
        //    tranExists = false;
        //    HttpResponseMessage httpResponse = new HttpResponseMessage();
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(endpointUrl);
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{authKey}:{authSecret}")));
        //        httpResponse =  client.GetAsync(actionUrl + $"?merchantId={merchantId}&merchantRef={merchantRef}");

        //        string jSonContent = httpResponse.Content.ReadAsStringAsync().Result;
        //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Response));
        //        var reader = new System.IO.StringReader(jSonContent);
        //        var response = (Response)xmlSerializer.Deserialize(reader);
        //        //response jSonResponse = JsonConvert.DeserializeObject<response>(jSonContent);
        //    }
        //    //return tranExists;
        //}

        public async Task<HttpResponseMessage> AcquirePayment(SOContipay row, CPSetup setup, CPProvider provider,
                                                                        string endpointUrl, string actionUrl,
                                                                        int? merchantId, string merchantRef,
                                                                            string authKey, string authSecret)
        {

            //
            var authenticationString = $"{authKey}:{authSecret}";
            var base64String = Convert.ToBase64String(
               System.Text.Encoding.ASCII.GetBytes(authenticationString));

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(endpointUrl);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.ASCII.GetBytes($"{authKey}:{authSecret}")));
                //Build jSon Object
                PaymentObj paymentObj = BuildJSonObject(row, setup, provider, merchantRef);
                var jSon = JsonConvert.SerializeObject(paymentObj);
                var httpContent = new StringContent(jSon, System.Text.Encoding.UTF8, "application/json");

                httpResponse = await client.PostAsync(actionUrl, httpContent);
                string xmlResult;
                if (httpResponse != null)
                {
                    xmlResult = httpResponse.Content.ReadAsStringAsync().Result;
                    System.IO.StringReader reader = new System.IO.StringReader(xmlResult);

                    var xmlContent = XElement.Parse(xmlResult);
                    var redirectUrl = xmlContent.Elements("redirectUrl").FirstOrDefault();

                    XmlSerializer serializer = new XmlSerializer(typeof(Response));
                    var response = (Response)serializer.Deserialize(reader);

                    processResponse(row, response, merchantId, merchantRef, endpointUrl, actionUrl, authKey, authSecret, redirectUrl);
                }
            }
            return httpResponse;
        }

        private void processResponse(SOContipay row, Response jSonResp, int? mId, string mRef, string ePoint, string aUrl, string aKey, string aSecret, XElement redirectUrl)
        {
            //Redirect if needed

            if (redirectUrl != null)
            {
                //throw new Exception("Redirect7:" + redirectUrl.Value);
                cpToken = GetUrl(redirectUrl.Value);
                //openUrl(redirectUrl.Value);
                deductAmtDue();
                //throw new PXRedirectToUrlException(redirectUrl.Value, PXBaseRedirectException.WindowMode.NewWindow, "Redirect:" + redirectUrl.Value);
                //throw new PXRedirectToUrlException(redirectUrl.Value, "App");
            }

            WriteToLog(jSonResp, aUrl);
            if (jSonResp.StatusCode == 0)
            {
                if (jSonResp.Message.ToLower() == "pending subscriber validation")
                {

                }
            }
            else if (jSonResp.StatusCode == 3)
            {
                if (jSonResp.Message.ToLower() == "duplicate record found!")
                {
                    GetDuplicateAndUpdateTran(mId, mRef, ePoint, aUrl, aKey, aSecret);
                }
                else if (jSonResp.Message.ToLower() == "payment reference has expired")
                {
                    //Do something
                    row.StatusID = 3;
                }
            }
        }

        private void deductAmtDue()
        {
            //Process SO Payments and Applications
            //ProcessSoPayment(merchantRef);
            UpdateSo(merchRef);
            clearHeader();
            clearFields();
        }

        private void UpdateSo(string merchantRef)
        {
            //var sOrdEntry = PXGraph.CreateInstance<SOOrderEntry>();

            //var orderType = order.OrderType;
            //var orderNbr = order.OrderNbr;

            //sOrdEntry.Document.Current = sOrdEntry.Document.Search<SOOrder.orderNbr>(orderNbr, orderType); ;

            //order.Status = "H";
            //order.Hold = true;
            //order.CuryPaymentTotal = order.CuryUnpaidBalance;
            //order.CuryUnpaidBalance = 0;
            //order.CustomerRefNbr = merchantRef;
            //sOrdEntry.Document.Update(order);

            //sOrdEntry.Actions.PressSave();
        }

        private void ProcessSoPayment(string merchantRef)
        {
            SOOrder order = OrdersView.Current;
            var paymentEntry = CreateInstance<ARPaymentEntry>();

            ARPayment arPayment = new ARPayment();
            arPayment.ExtRefNbr = order.ExtRefNbr;
            arPayment.CustomerID = order.CustomerID;
            //arPayment.CustomerLocationID = 1;
            //arPayment.PaymentMethodID = "CONTIPAY";
            arPayment.CashAccountID = 102100;
            arPayment.DocDesc = "CONTIPAY - Automated Self-Service Portal Payment";
            arPayment.CuryOrigDocAmt = order.CuryUnpaidBalance;

            paymentEntry.Document.Update(arPayment);
            paymentEntry.Actions.PressSave();
        }

        private void WriteToLog(Response jSonResp, string aUrl)
        {
            CP_Log log = new CP_Log();
            log.LogDate = DateTime.Now;
            log.StatusCode = jSonResp.StatusCode;
            log.LogMessage = jSonResp.Message;
            PXGraph graph = new PXGraph<PXGraph, CP_Log>();
        }

        private async void GetDuplicateAndUpdateTran(int? mId, string mRef, string ePoint, string aUrl, string aKey, string aSecret)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ePoint);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{aKey}:{aSecret}")));
                httpResponse = await client.GetAsync(aUrl + $"?merchantId={mId}&merchantRef={mRef}");

                string jSonContent = httpResponse.Content.ReadAsStringAsync().Result;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Response));
                var reader = new System.IO.StringReader(jSonContent);
                var response = (Response)xmlSerializer.Deserialize(reader);
                //response jSonResponse = JsonConvert.DeserializeObject<response>(jSonContent);
            }
        }

        public PaymentObj BuildJSonObject(SOContipay row, CPSetup setup, CPProvider provider, string merchantRef)
        {
            PaymentObj objPayment = new PaymentObj();
            //Populate Complex Object
            objPayment.customer = BuildCustomer(row, setup);
            objPayment.transaction = BuildTransaction(row, setup, provider, merchantRef);
            objPayment.accountDetails = BuildAccDetail(row, setup);

            return objPayment;
        }
        #region Transaction Classes
        public class PaymentObj
        {
            public Customer customer { get; set; }
            public Transaction transaction { get; set; }
            public AccountDetails accountDetails { get; set; }
        }

        public class Customer
        {
            public string nationalId { get; set; }
            public string surname { get; set; }
            public string firstName { get; set; }
            public string middleName { get; set; }
            public string email { get; set; }
            public string cell { get; set; }
            public string countryCode { get; set; }
        }

        public class Transaction
        {
            public string providerCode { get; set; }
            public string providerName { get; set; }
            public string currencyCode { get; set; }
            public int? merchantId { get; set; }
            public string reference { get; set; }
            public string description { get; set; }
            public decimal? amount { get; set; }
            public string webhookUrl { get; set; }
        }

        public class AccountDetails
        {
            public string accountNumber { get; set; }
            public string accountName { get; set; }
            public AccountExtra accountExtra { get; set; }
        }

        public class AccountExtra
        {

            public string code { get; set; }
            public string expiry { get; set; }
            public string smsNumber { get; set; }
            public string bankName { get; set; }
            public string branchName { get; set; }
        }

        public Customer BuildCustomer(SOContipay row, CPSetup setup)
        {
            Guid id = PXAccess.GetUserID();

            Contact contact = PXSelect<Contact,
                                        Where<Contact.userID,
                                            Equal<Required<Contact.userID>>>>.Select(this, id);
            if (contact == null) return null;
            var cellphone = contact.Phone1 ?? contact.Phone2 ?? contact.Phone3 ?? "0776214825";
            Address contactAddress = PXSelect<Address,
                                        Where<Address.bAccountID,
                                            Equal<Required<Address.bAccountID>>>>.Select(this, contact.BAccountID);
            GetProvider(1);


            Customer customer = new Customer
            {
                nationalId = "-",
                surname = contact.LastName != null ? contact.LastName : "-",
                firstName = contact.FirstName != null ? contact.FirstName : "-",
                middleName = contact.MidName != null ? contact.MidName : "-",
                email = contact.EMail != null ? contact.EMail : "-",
                cell = cellphone,
                countryCode = contactAddress.CountryID != null ? contactAddress.CountryID : "ZW"
            };
            return customer;
        }
        public static Transaction BuildTransaction(SOContipay row, CPSetup setup, CPProvider provider, string merchantRef)
        {
            Transaction transaction = new Transaction
            {
                providerCode = provider.Code,
                providerName = provider.Name,
                currencyCode = row.Curyid,
                merchantId = setup.MerchantID,
                reference = merchantRef,
                description = "Acumation Order # " + row.OrderType + "-" + row.SOOrderNbr,
                amount = row.CuryAmt,
                webhookUrl = "-"
            };

            return transaction;
        }
        public static AccountDetails BuildAccDetail(SOContipay row, CPSetup setup)
        {
            AccountExtra accExtra = BuildAccExtra(row, setup);
            AccountDetails accountDetails = new AccountDetails
            {
                accountNumber = row.AccountNbr,
                accountName = row.AccountName,
                accountExtra = accExtra
            };

            return accountDetails;
        }
        public static AccountExtra BuildAccExtra(SOContipay row, CPSetup setup)
        {
            AccountExtra accountExtra = new AccountExtra
            {
                smsNumber = row.SmsNbr != null ? row.SmsNbr : "-",
                code = row.Code != null ? row.Code : "-",
                expiry = row.Expiry != null ? row.Expiry : "-"
            };

            return accountExtra;
        }

        //public class response
        //{
        //    public string status { get; set; }
        //    public string statusCode { get; set; }
        //    public string message { get; set; }
        //    public string mode { get; set; }
        //}

        [XmlRoot(ElementName = "response")]
        public class Response
        {

            [XmlElement(ElementName = "status")]
            public string Status { get; set; }

            [XmlElement(ElementName = "statusCode")]
            public int StatusCode { get; set; }

            [XmlElement(ElementName = "message")]
            public string Message { get; set; }

            [XmlElement(ElementName = "mode")]
            public string Mode { get; set; }
        }

        #endregion

        #endregion

        #endregion
    }
}