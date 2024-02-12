using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;


namespace AcuContipayWebApp
{
    public partial class ProcessPayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Create a dummy DataTable
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[11] {
            new DataColumn("orderType", typeof(string)),
            new DataColumn("orderNbr", typeof(string)),
            new DataColumn("approved", typeof(string)),
            new DataColumn("status", typeof(string)),
             new DataColumn("contiPayStatus", typeof(string)),
            new DataColumn("orderDate", typeof(string)),
            new DataColumn("CreatedByID_Creator_Username", typeof(string)),
            new DataColumn("OrderQty", typeof(int)),
            new DataColumn("CuryID", typeof(string)),
            new DataColumn("CuryOrderTotal", typeof(decimal)),
            new DataColumn("CuryUnpaidBalance", typeof(decimal)) });

                // Add some dummy data to the DataTable
                DateTime orderDate = DateTime.Now; 
                string orderDateString = orderDate.ToString("dd-MM-yyyy");

                dt.Rows.Add("Type1", "123", "Yes", "Approved","Compilation", orderDateString, "User1", 5, "USD", 100.00, 50.00);
                dt.Rows.Add("Type2", "456", "No", "Pending","Compilation", orderDateString, "User2", 10, "EUR", 200.00, 150.00);

                Session["dtOrderDetails"] = dt;

                // Bind the DataTable to the GridView
                gvOpenOrders.DataSource = dt;
                gvOpenOrders.DataBind();
            }
        }

        protected void gvSelIdxChanged(object sender, EventArgs e)
        {
            GridView gv = sender as GridView;
            if (gv != null && gv.SelectedIndex >= 0)
            {
                // Assuming your data source is a DataTable or similar, and orderNbr is a column in it
                DataTable dt = (DataTable)Session["dtOrderDetails"];

                string orderNbr = dt.Rows[gv.SelectedIndex]["orderNbr"].ToString();
                DateTime orderDate;
                DateTime.TryParse(dt.Rows[gv.SelectedIndex]["orderDate"].ToString(), out orderDate);
                string CuryID = dt.Rows[gv.SelectedIndex]["CuryID"].ToString();
                string curyOrderTotal = dt.Rows[gv.SelectedIndex]["CuryOrderTotal"].ToString();
                string curyUnpaidBalance = dt.Rows[gv.SelectedIndex]["CuryUnpaidBalance"].ToString();
                string contiPayStatus = dt.Rows[gv.SelectedIndex]["contiPayStatus"].ToString();

                if (dt != null)
                {
                    txtOrderNbr.Text = orderNbr;
                    string orderDateString = orderDate.ToString("yyyy-MM-dd");
                    txtOrderDate.Text = orderDateString;
                    txtCuryID.Text = CuryID;
                    txtCuryOrderTotal.Text = curyOrderTotal;
                    txtUpaidBalance.Text = curyUnpaidBalance;
                    txtContiPayStatus.Text = contiPayStatus;
                }

                Session["SessionSOContipay"] = dt;

            }
        }
        protected void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                //    //Get ContiPay Setup
                string WebMerchantId = ConfigurationManager.AppSettings["MerchantId"];
                string baseURL = ConfigurationManager.AppSettings["BaseURL"];
                string authenticationKey = ConfigurationManager.AppSettings["AuthenticationKey"];
                string authenticationSecret = ConfigurationManager.AppSettings["AuthenticationSecret"];

                #region variables
                string actionUrl = "/acquire/payment";
                int merchantId = int.Parse(WebMerchantId);
                string endpointUrl = baseURL;
                var authKey = authenticationKey;
                string authSecret = authenticationSecret;


                    bool tranExists = false;


                //   var merchRef = GetExtRefNbr();

                //GetTran(endpointUrl, actionUrl,
                //                merchantId, merchantRef,
                //                    authKey, authSecret);
                //if (tranExists)
                //{

                //}
                //else
                //{
                //rtow = screen elements


                //CPProvider provider = GetProvider();
                //Task<HttpResponseMessage> taskAquirePayment = Task.Run(async () => await
                //                                                AcquirePayment(
                //                                                    endpointUrl, actionUrl,
                //                                                        merchantId, merchRef,
                //        //                                                            authKey, authSecret).ConfigureAwait(false));
                //    }


                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        //public string GetExtRefNbr()
        //{


        //        string merchantRefNbr = string.Empty;
        //    //    // SOContipay soContipay = new SOContipay(); // Your instance of SOContipay

        //        DataTable dtContiPay = (DataTable)Session["SessionSOContipay"];

        //        if (dtContiPay != null)
        //        {
        //        string orderType = dtContiPay.Rows[0]["orderNbr"].ToString();
        //        string orderNbr = dtContiPay.Rows[0]["orderNbr"].ToString();

        //                SOOrder order = new SOOrder
        //                {
        //                    OrderType = orderType,
        //                    OrderNbr = orderNbr,

        //                };

        //                merchantRefNbr = order.CustomerRefNbr;


        //        //        if (merchantRefNbr == null)
        //        //        {
        //        //            merchantRefNbr = order.OrderType + "-" + order.OrderNbr + "-" + "00";
        //        //        }
        //        //        else
        //        //        {
        //        //            string extRefNbr = merchantRefNbr.Split('-').Last();
        //        //            if (extRefNbr == null)
        //        //            {
        //        //                merchantRefNbr = order.OrderType + "-" + order.OrderNbr + "-" + "00";
        //        //            }
        //        //            else
        //        //            {
        //        //                //Extract Suffix, convert to number, increment Suffix by 1
        //        //                int intRefNbr = int.Parse(extRefNbr) + 1;
        //        //                //connvert to string - change format to XX
        //        //                merchantRefNbr = order.OrderType + "-" + order.OrderNbr + "-" + intRefNbr.ToString("##");
        //        //            }
        //        //        }

        //        //        try
        //        //        {
        //        //            SOOrderEntry sOrdEntry = PXGraph.CreateInstance<SOOrderEntry>();

        //        //            //var orderType = order.OrderType;
        //        //            //var orderNbr = order.OrderNbr;

        //        //            sOrdEntry.Document.Current = sOrdEntry.Document.Search<SOOrder.orderNbr>(orderNbr, orderType); ;
        //        //            order.CustomerRefNbr = merchantRefNbr;
        //        //            sOrdEntry.Document.Update(order);

        //        //            sOrdEntry.Actions.PressSave();
        //        //        }
        //        //        catch (Exception ex)
        //        //        {
        //        //            int x = 0;
        //        //        }

        //    }



        //    return merchantRefNbr;
        //}
        protected void btnVerify_Click(object sender, EventArgs e)
        {
            // Add your verification logic here
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            // Clear all form fields
            //ddlSalesOrderType.SelectedIndex = 0;
            //ddlContipayProvider.SelectedIndex = 0;
            //txtSalesOrderDate.Text = "";
            //txtAccountName.Text = "";
            //txtTranDate.Text = "";
            //txtCodeCVV.Text = "";
            //ddlPaymentMethod.SelectedIndex = 0;
            //txtExpiry.Text = "";
        }

        public void refreshdata()
        {
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True;User Instance=True");
            SqlCommand cmd = new SqlCommand("select * from tbl_data", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            gvOpenOrders.DataSource = ds;
            gvOpenOrders.DataBind();
        }
    }
}