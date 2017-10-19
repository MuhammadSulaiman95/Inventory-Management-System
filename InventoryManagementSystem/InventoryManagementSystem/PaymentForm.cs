using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace InventoryManagementSystem
{
    public partial class PaymentForm : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        public PaymentForm()
        {
            InitializeComponent();
        }

        public void showdata()
        {
            cmd = new SqlCommand("Select * from Vendor", con);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void btnMainForm_Click(object sender, EventArgs e)
        {
            this.Hide();
            Class1.mainform();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("spVendorInsert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@vid", txtVid.Text);
            cmd.Parameters.AddWithValue("@vn", txtVName.Text);
            cmd.Parameters.AddWithValue("@vad", txtVAdres.Text);
            cmd.Parameters.AddWithValue("@vmob", txtVMob.Text);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Inserted");
            showdata();
        }

        private void PaymentForm_Load(object sender, EventArgs e)
        {
            showdata();

        }
    }
}
