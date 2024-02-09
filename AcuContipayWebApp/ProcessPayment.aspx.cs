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
                dt.Columns.AddRange(new DataColumn[10] {
            new DataColumn("orderType", typeof(string)),
            new DataColumn("orderNbr", typeof(string)),
            new DataColumn("approved", typeof(string)),
            new DataColumn("status", typeof(string)),
            new DataColumn("orderDate", typeof(string)),
            new DataColumn("CreatedByID_Creator_Username", typeof(string)),
            new DataColumn("OrderQty", typeof(int)),
            new DataColumn("CuryID", typeof(string)),
            new DataColumn("CuryOrderTotal", typeof(decimal)),
            new DataColumn("CuryUnpaidBalance", typeof(decimal))
                         });

                // Add some dummy data to the DataTable
                dt.Rows.Add("Type1", "123", "Yes", "Approved", DateTime.Now.ToShortDateString(), "User1", 5, "USD", 100.00, 50.00);
                dt.Rows.Add("Type2", "456", "No", "Pending", DateTime.Now.ToShortDateString(), "User2", 10, "EUR", 200.00, 150.00);

                // Bind the DataTable to the GridView
                gvOpenOrders.DataSource = dt;
                gvOpenOrders.DataBind();
            }
        }

        protected void gvSelIdxChanged(object sender, EventArgs e)
        {
            // Your event handler logic here, if any
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