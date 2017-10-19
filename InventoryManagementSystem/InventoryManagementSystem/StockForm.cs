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
using System.Text.RegularExpressions;

namespace InventoryManagementSystem
{
    public partial class StockForm : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cs"].ConnectionString);
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        public StockForm()
        {
            InitializeComponent();
        }

        public void autoid()
        {
            con.Open();
            cmd = new SqlCommand("select max(ItemId) from tblStock", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string id;
                id = dr[0].ToString();
                id = Regex.Replace(id, "\\d+", m => (int.Parse(m.Value) + 1).ToString(new string('0', m.Value.Length)));
                txtItemId.Text = id;
            }
            con.Close();
        }

        public void autoComplete()
        {
            txtItemName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtItemName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection cl = new AutoCompleteStringCollection();
            cmd = new SqlCommand("select ItemName from tblStock",con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cl.Add(dr[0].ToString());
            }
            txtItemName.AutoCompleteCustomSource = cl;
            con.Close();
        }

        public void showdata()
        {
            cmd = new SqlCommand("Select * from tblStock", con);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            StockGrid.DataSource = dt;
        }

        public void clear()
        {
            txtItemName.Text = "";
            txtCompany.Text = "";
            txtNewQty.Text = "";
            txtPurPer.Text = "";
            txtPurPrice.Text = "";
            txtSalePrice.Text = "";
            txtTotalAmt.Text = "";
            txtNetAmt.Text = "";
            txtDiscount.Text = "";
            txtPitemGStax.Text = "";
            txtTotalGst.Text = "";
            txtInv.Text = "";
            txtVendorId.Text = "";
            txtVName.Text = "";
            txtUPrice.Text = "";
            txtBarcode.Text = "";
            purDate.ResetText();
            ExpDate.ResetText();
        }

        private void btnMainForm_Click(object sender, EventArgs e)
        {
            this.Hide();
            Class1.mainform();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            purDate.Format = DateTimePickerFormat.Custom;
            purDate.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            con.Open();
            SqlTransaction tra = con.BeginTransaction();
            try
            {
                cmd=new SqlCommand("PStockInsert", con,tra);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Iid", txtItemId.Text.ToString());
                cmd.Parameters.AddWithValue("@ItemName", txtItemName.Text);
                cmd.Parameters.AddWithValue("@ItemCompany", txtCompany.Text);
                cmd.Parameters.AddWithValue("@UnitPrice ", txtUPrice.Text.ToString());
                cmd.Parameters.AddWithValue("@PurchasePrice", txtPurPrice.Text.ToString());
                cmd.Parameters.AddWithValue("@SalePrice", txtSalePrice.Text.ToString());
                cmd.Parameters.AddWithValue("@PurchasePercent", txtPurPer.Text.ToString());
                cmd.Parameters.AddWithValue("@PreQuantity", 0);
                cmd.Parameters.AddWithValue("@NewQuantity", txtNewQty.Text);
                cmd.Parameters.AddWithValue("@TotalQuantity", txtNewQty.Text);
                cmd.Parameters.AddWithValue("@DateIn", purDate.Value.ToString());
                cmd.Parameters.AddWithValue("@VID", txtVendorId.Text);
                cmd.Parameters.AddWithValue("@VName", txtVName.Text);
                cmd.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                cmd.Parameters.AddWithValue("@ExpDate", ExpDate.Value.ToString());
                cmd.Parameters.AddWithValue("@InvNo", txtInv.Text);
                cmd.Parameters.AddWithValue("@GST", txtPitemGStax.Text.ToString());
                cmd.Parameters.AddWithValue("@TGST", txtTotalGst.Text.ToString());
                cmd.Parameters.AddWithValue("@Dis", txtDiscount.Text.ToString());
                cmd.Parameters.AddWithValue("@TAmount", txtTotalAmt.Text.ToString());
                cmd.Parameters.AddWithValue("@NAmt", txtNetAmt.Text.ToString());
                cmd.ExecuteNonQuery();
                tra.Commit();
                MessageBox.Show("Record Inserted");
            }
            catch(Exception ex)
            {
                tra.Rollback();
                MessageBox.Show(ex.Message+"Failed");
            }
            finally
            {
                con.Close();
            }
            showdata();
            clear();
        }

        private void StockForm_Load(object sender, EventArgs e)
        {
            showdata();
            autoid();
            autoComplete();
        }

        private void txtTotalGst_Leave(object sender, EventArgs e)
        {
            decimal c;
            if(decimal.TryParse(txtTotalGst.Text,out c) && decimal.TryParse(txtUPrice.Text,out c))
            {
                txtPitemGStax.Text = (decimal.Parse(txtTotalGst.Text) / int.Parse(txtNewQty.Text)).ToString();
                txtPurPrice.Text = (decimal.Parse(txtUPrice.Text) + decimal.Parse(txtPitemGStax.Text)).ToString();
                txtTotalAmt.Text = (decimal.Parse(txtPurPrice.Text) * int.Parse(txtNewQty.Text)).ToString();
            }
            else
            {
                MessageBox.Show("wrong value");
                txtTotalGst.Clear();
            }
        }

        private void txtSalePrice_Leave(object sender, EventArgs e)
        {
            decimal p,s,g;
            if(decimal.TryParse(txtPurPrice.Text,out p) && decimal.TryParse(txtSalePrice.Text,out p))
            {
                if(decimal.Parse(txtPurPrice.Text)<=decimal.Parse(txtSalePrice.Text))
                {
                p=decimal.Parse(txtPurPrice.Text);
                s=decimal.Parse(txtSalePrice.Text);
                g=s-p;
                decimal gp=(g*100)/p;
                txtPurPer.Text=gp.ToString();
                }
                else
                {
                    MessageBox.Show("sale price must be greater than purchase price");
                    txtSalePrice.Clear();
                }
            }
            else
            {
                MessageBox.Show("wrong value");
                txtSalePrice.Clear();
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void insertStockGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtItemName.Text = StockGrid.CurrentRow.Cells["ItemName"].Value.ToString();
            txtCompany.Text = StockGrid.CurrentRow.Cells["ItemCompany"].Value.ToString();
            txtUPrice.Text = StockGrid.CurrentRow.Cells["UnitPrice"].Value.ToString();
            txtPurPrice.Text = StockGrid.CurrentRow.Cells["PurchasePrice"].Value.ToString();
            txtSalePrice.Text = StockGrid.CurrentRow.Cells["SalePrice"].Value.ToString();
            txtPurPer.Text = StockGrid.CurrentRow.Cells["PurchasePercent"].Value.ToString();
            txtNewQty.Text = StockGrid.CurrentRow.Cells["NewQuantity"].Value.ToString();
            purDate.Text = StockGrid.CurrentRow.Cells["DateIn"].Value.ToString();
            txtVendorId.Text = StockGrid.CurrentRow.Cells["VID"].Value.ToString();
            txtVName.Text = StockGrid.CurrentRow.Cells["VName"].Value.ToString();
            txtBarcode.Text = StockGrid.CurrentRow.Cells["barcode"].Value.ToString();
            ExpDate.Text = StockGrid.CurrentRow.Cells["ExpDate"].Value.ToString();
            txtInv.Text = StockGrid.CurrentRow.Cells["InvNo"].Value.ToString();
            txtPitemGStax.Text = StockGrid.CurrentRow.Cells["GST"].Value.ToString();
            txtTotalGst.Text = StockGrid.CurrentRow.Cells["TGST"].Value.ToString();
            txtDiscount.Text = StockGrid.CurrentRow.Cells["Dis"].Value.ToString();
            txtTotalAmt.Text = StockGrid.CurrentRow.Cells["TAmount"].Value.ToString();
            txtNetAmt.Text = StockGrid.CurrentRow.Cells["NAmt"].Value.ToString();

            txtUpIid.Text = StockGrid.CurrentRow.Cells["ItemId"].Value.ToString();
            txtUpIName.Text = StockGrid.CurrentRow.Cells["ItemName"].Value.ToString();
            txtUpICom.Text = StockGrid.CurrentRow.Cells["ItemCompany"].Value.ToString();
            txtUpUPrice.Text = StockGrid.CurrentRow.Cells["UnitPrice"].Value.ToString();
            //txtUpPPrice.Text = StockGrid.CurrentRow.Cells["PurchasePrice"].Value.ToString();
            txtUpSPrice.Text = StockGrid.CurrentRow.Cells["SalePrice"].Value.ToString();
            //txtUpPPercent.Text = StockGrid.CurrentRow.Cells["PurchasePercent"].Value.ToString();
            txtUpPQty.Text = StockGrid.CurrentRow.Cells["PreQuantity"].Value.ToString();
            //txtUpNQty.Text = StockGrid.CurrentRow.Cells["NewQuantity"].Value.ToString();
            //txtUpTQty.Text = StockGrid.CurrentRow.Cells["TotalQuantity"].Value.ToString();
            upPurDate.Text = StockGrid.CurrentRow.Cells["DateIn"].Value.ToString();
            txtUpVid.Text = StockGrid.CurrentRow.Cells["VID"].Value.ToString();
            txtUpVName.Text = StockGrid.CurrentRow.Cells["VName"].Value.ToString();
            txtUpBar.Text = StockGrid.CurrentRow.Cells["barcode"].Value.ToString();
            UpExpDate.Text = StockGrid.CurrentRow.Cells["ExpDate"].Value.ToString();
            txtUpInv.Text = StockGrid.CurrentRow.Cells["InvNo"].Value.ToString();
            //txtUpPGst.Text = StockGrid.CurrentRow.Cells["GST"].Value.ToString();
            //txtUpTGST.Text = StockGrid.CurrentRow.Cells["TGST"].Value.ToString();
            //txtUpDiscount.Text = StockGrid.CurrentRow.Cells["Dis"].Value.ToString();
            //txtUpTAmt.Text = StockGrid.CurrentRow.Cells["TAmount"].Value.ToString();
            //txtUpNAmt.Text = StockGrid.CurrentRow.Cells["NAmt"].Value.ToString();
        }

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            decimal c,p,s;
            if (decimal.TryParse(txtTotalAmt.Text, out c) && decimal.TryParse(txtDiscount.Text, out c))
            {
                p = decimal.Parse(txtTotalAmt.Text);
                s = decimal.Parse(txtDiscount.Text);
                c = p * (s / 100);
                decimal f = p - c;
                txtNetAmt.Text = f.ToString();
            }
            else
            {
                MessageBox.Show("wrong value");
                txtDiscount.Clear();
            }
        }

        private void txtSearchItemName_TextChanged(object sender, EventArgs e)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = string.Format("ItemName like '%" + txtSearchItemName.Text + "%' ");
            StockGrid.DataSource = dv.ToTable();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            upPurDate.Format = DateTimePickerFormat.Custom;
            upPurDate.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            con.Open();
            try
            {
                cmd = new SqlCommand("PStockUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", txtUpIid.Text);
                cmd.Parameters.AddWithValue("@ItemName", txtUpIName.Text);
                cmd.Parameters.AddWithValue("@ItemCompany", txtUpICom.Text);
                cmd.Parameters.AddWithValue("@UnitPrice ", txtUpUPrice.Text.ToString());
                cmd.Parameters.AddWithValue("@PurchasePrice", txtUpPPrice.Text.ToString());
                cmd.Parameters.AddWithValue("@SalePrice", txtUpSPrice.Text.ToString());
                cmd.Parameters.AddWithValue("@PurchasePercent", txtUpPPercent.Text.ToString());
                cmd.Parameters.AddWithValue("@PreQty", txtUpPQty.Text.ToString());
                cmd.Parameters.AddWithValue("@NewQty", txtUpNQty.Text.ToString());
                //cmd.Parameters.AddWithValue("@TotalQty", txtUpTQty.Text);
                cmd.Parameters.AddWithValue("@DateIn", upPurDate.Value.ToString());
                cmd.Parameters.AddWithValue("@VID", txtUpVid.Text);
                cmd.Parameters.AddWithValue("@VName", txtUpVName.Text);
                cmd.Parameters.AddWithValue("@barcode", txtUpBar.Text);
                cmd.Parameters.AddWithValue("@ExpDate", UpExpDate.Value.ToString());
                cmd.Parameters.AddWithValue("@InvNo", txtUpInv.Text);
                cmd.Parameters.AddWithValue("@GST", txtUpPGst.Text.ToString());
                cmd.Parameters.AddWithValue("@TGST", txtUpTGST.Text.ToString());
                cmd.Parameters.AddWithValue("@Dis", txtUpDiscount.Text.ToString());
                cmd.Parameters.AddWithValue("@TAmt", txtUpTAmt.Text.ToString());
                cmd.Parameters.AddWithValue("@NAmt", txtUpNAmt.Text.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Updated");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message+"Failed");
            }
            finally
            {
                con.Close();
            }
            showdata();
        }

        private void txtUpTGST_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUpTGST_Leave(object sender, EventArgs e)
        {
            decimal c;
            if (decimal.TryParse(txtUpTGST.Text, out c) && decimal.TryParse(txtUpUPrice.Text, out c))
            {
                txtUpPGst.Text = (decimal.Parse(txtUpTGST.Text) / int.Parse(txtUpNQty.Text)).ToString();
                txtUpPPrice.Text = (decimal.Parse(txtUpUPrice.Text) + decimal.Parse(txtUpPGst.Text)).ToString();
                txtUpTAmt.Text = (decimal.Parse(txtUpPPrice.Text) * int.Parse(txtUpNQty.Text)).ToString();
            }
            else
            {
                MessageBox.Show("wrong value");
                txtUpTGST.Clear();
            }
        }

        private void txtUpSPrice_Leave(object sender, EventArgs e)
        {
            decimal p, s, g;
            if (decimal.TryParse(txtUpPPrice.Text, out p) && decimal.TryParse(txtUpSPrice.Text, out p))
            {
                if (decimal.Parse(txtUpPPrice.Text) <= decimal.Parse(txtUpSPrice.Text))
                {
                    p = decimal.Parse(txtUpPPrice.Text);
                    s = decimal.Parse(txtUpSPrice.Text);
                    g = s - p;
                    decimal gp = (g * 100) / p;
                    txtUpPPercent.Text = gp.ToString();
                }
                else
                {
                    MessageBox.Show("sale price must be greater than purchase price");
                    txtUpSPrice.Clear();
                }
            }
            else
            {
                MessageBox.Show("wrong value");
                txtUpSPrice.Clear();
            }
           
        }

        private void txtUpDiscount_Leave(object sender, EventArgs e)
        {
            decimal c, p, s;
            if (decimal.TryParse(txtUpTAmt.Text, out c) && decimal.TryParse(txtUpDiscount.Text, out c))
            {
                p = decimal.Parse(txtUpTAmt.Text);
                s = decimal.Parse(txtUpDiscount.Text);
                c = p * (s / 100);
                decimal f = p - c;
                txtUpNAmt.Text = f.ToString();
            }
            else
            {
                MessageBox.Show("wrong value");
                txtUpDiscount.Clear();
            }
        }

        private void txtUpNQty_Leave(object sender, EventArgs e)
        {
            //int p, n, t;
            //p = int.Parse(txtUpPQty.Text);
            //n = int.Parse(txtUpNQty.Text);
            //t = (p + n);
            //txtUpTQty.Text = t.ToString();
        }
    }
}
