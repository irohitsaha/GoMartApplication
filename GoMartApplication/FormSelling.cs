using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace GoMartApplication
{
    public partial class FormSelling : Form
    {
        DBConnect dbCon = new DBConnect();
        public FormSelling()
        {
            InitializeComponent();
        }

        private void FormSelling_Load(object sender, EventArgs e)
        {
            BindCategory();
            lblDate.Text = DateTime.Now.ToShortDateString();
            try
            {
                SqlCommand cmd = new SqlCommand("spGetBillList", dbCon.GetCon());
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BindCategory()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetCategory", dbCon.GetCon());
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add default option
                DataRow dr = dt.NewRow();
                dr["CatID"] = 0;
                dr["CategoryName"] = "Select Category";
                dt.Rows.InsertAt(dr, 0);

                cmbCategory.DataSource = dt;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CatID";

                cmbCategory.SelectedIndex = 0;   // default selection
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BindProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList", dbCon.GetCon());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2_Product.DataSource = dt;
                //dbCon.OpenCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        double GrandTotal = 0.0;
        int n = 0;
        private void SearchedProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList_SearchByCat", dbCon.GetCon());
                cmd.Parameters.AddWithValue("@ProdCatID", Convert.ToInt32(cmbCategory.SelectedValue));
                //cmd.Parameters.AddWithValue("@ProdCatID", cmbSearch.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2_Product.DataSource = dt;
                //dbCon.OpenCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            try
            {
               
                    int catID = Convert.ToInt32(cmbCategory.SelectedValue);

                    if (catID == 0)
                    {
                        // Show all products
                        BindProductList();
                    }
                    else
                    {
                        // Show products by category
                        SearchedProductList();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddOreder_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPrice.Text == "" || txtQuantity.Text == "")
                {
                    MessageBox.Show("Enter valid Qty or Prince", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    double total = Convert.ToDouble(txtPrice.Text) * Convert.ToInt32(txtQuantity.Text);
                    DataGridViewRow addRow = new DataGridViewRow();
                    addRow.CreateCells(dataGridView1_Order);
                    addRow.Cells[0].Value = ++n;
                    addRow.Cells[1].Value = txtPName.Text;
                    addRow.Cells[2].Value = txtPrice.Text;
                    addRow.Cells[3].Value = txtQuantity.Text;
                    addRow.Cells[4].Value = total;
                    dataGridView1_Order.Rows.Add(addRow);
                    GrandTotal += total;
                    lblGrandTotal.Text = "Rs." + GrandTotal;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_Product_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2_Product.CurrentRow != null)
                {
                    txtSellingID.Text = dataGridView2_Product.CurrentRow.Cells[0].Value.ToString();
                    txtPName.Text = dataGridView2_Product.CurrentRow.Cells[1].Value.ToString();
                    txtPrice.Text = dataGridView2_Product.CurrentRow.Cells[4].Value.ToString();

                    txtQuantity.Clear();
                    txtQuantity.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtBill.Text=="")
                {
                    MessageBox.Show("Enter Bill Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("spInsertBill", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@Bill_ID", txtBill.Text);
                    cmd.Parameters.AddWithValue("@SellerID", Form1.loginName);
                    cmd.Parameters.AddWithValue("@SellDate", lblDate.Text);
                    cmd.Parameters.AddWithValue("@TotalAmt", Convert.ToDouble(txtQuantity.Text));
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbCon.OpenCon();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        BindBillList();
                        MessageBox.Show("Bill Added Successfully...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clrtext();
                    }
                    dbCon.CloseCon();
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void BindBillList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetBillList", dbCon.GetCon());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dbCon.OpenCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void clrtext()
        {
            txtBill.Clear();
            dataGridView1_Order.DataSource = null;
            txtPrice.Clear();
            txtSellingID.Clear();
            txtPName.Clear();
            txtQuantity.Clear();
            lblGrandTotal.Text = "0.0";
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset category dropdown
                cmbCategory.SelectedIndex = 0;

                // Reload all products
                BindProductList();

                // Clear text fields
                txtSellingID.Clear();
                txtPName.Clear();
                txtPrice.Clear();
                txtQuantity.Clear();

                // Clear order grid
                dataGridView1_Order.Rows.Clear();

                // Reset totals
                GrandTotal = 0;
                lblGrandTotal.Text = "0.0";

                // Focus again
                txtQuantity.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            BindBillList();
        }
    }
}
