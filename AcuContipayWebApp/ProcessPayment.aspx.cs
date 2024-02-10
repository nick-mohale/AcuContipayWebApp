using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            // Your event handler logic here, if any
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
                    string orderDateString = orderDate.ToString("yyyy-MM-dd"); // or any other desired date format
                    txtOrderDate.Text = orderDateString;
                    txtCuryID.Text = CuryID;
                    txtCuryOrderTotal.Text = curyOrderTotal;
                    txtUpaidBalance.Text = curyUnpaidBalance;
                    txtContiPayStatus.Text = contiPayStatus;
                }
            }
        }
        protected void btnPay_Click(object sender, EventArgs e)
        {
            // Add your payment processing logic here
        }

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