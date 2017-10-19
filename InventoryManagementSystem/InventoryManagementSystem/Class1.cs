using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryManagementSystem
{
    class Class1
    {
        public static void mainform()
        {
            MainForm cf = new MainForm();
            cf.Show();
        }

        public static void customerform()
        {
            CustomerForm cf = new CustomerForm();
            cf.Show();
        }

        public static void saleform()
        {
            SaleForm sf = new SaleForm();
            sf.Show();
        }

        public static void stockform()
        {
            StockForm sf = new StockForm();
            sf.Show();
        }

        public static void paymentform()
        {
            PaymentForm sf = new PaymentForm();
            sf.Show();
        }
    }
}
