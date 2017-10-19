using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public static void mainform()
        {
            MainForm cf = new MainForm();
            cf.Show();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            this.Hide();
            Class1.saleform();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            Class1.customerform();
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            this.Hide();
            Class1.stockform();
        }

        private void brnReturn_Click(object sender, EventArgs e)
        {
            
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            this.Hide();
            Class1.paymentform();
        }
    }
}
