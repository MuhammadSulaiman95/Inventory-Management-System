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
    public partial class LoginForm : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        public LoginForm()
        {
            InitializeComponent();
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from tbllogin where username=@u and password=@p",con);
            cmd.Parameters.AddWithValue("@u",txtuname.Text);
            cmd.Parameters.AddWithValue("@p",txtpass.Text);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                //while (dr.Read())
                //{
                //    if (dr["type"].ToString() == "admin") 
                //    {
                //        MessageBox.Show("Admin");
                //    }
                //    else if (dr["type"].ToString()=="user")
                //    {
                //        MessageBox.Show("User");
                //    }
                //    else
                //    {
                //        MessageBox.Show("Wrong Value inserted");
                //    }
                //}

                this.Hide();
                Class1.mainform();
            }
            con.Close();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
