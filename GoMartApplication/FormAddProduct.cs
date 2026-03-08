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
    public partial class FormAddProduct : Form
    {
        DBConnect dbCon = new DBConnect();
        public FormAddProduct()
        {
            InitializeComponent();
        }
        private void FormAddProduct_Load(object sender, EventArgs e)
        {
            BindCategory();
            BindProductList();
            SearchByCategory();
            lblProductId.Visible = false;
            txtProductID.Visible = false;
            btnDeleteProduct.Visible = false;
            btnUpdateProduct.Visible = false;
            btnAddProduct.Visible = true;
            txtProductName.Focus();
            cmbSearch.SelectedIndex = 0;
        }
        private void BindCategory()
        {
            SqlCommand cmd = new SqlCommand("spGetCategory", dbCon.GetCon());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbProductCategory.DataSource = dt;
            cbProductCategory.DisplayMember = "CategoryName";
            cbProductCategory.ValueMember = "CatID";
            dbCon.OpenCon();
        }
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProductName.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Product Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductName.Focus();
                    return;
                }
                if (Convert.ToInt32(txtProductPrice.Text) < 0 || txtProductPrice.Text == string.Empty )
                {
                    MessageBox.Show("Please Enter Valid Product Price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductPrice.Focus();
                    return;
                }
                if (Convert.ToInt32(txtProductQuantity.Text) < 0 || txtProductQuantity.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Valid Product Quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductQuantity.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("spCheckDuplicateProduct", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@ProdName", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatID", cbProductCategory.SelectedValue);
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbCon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Product Name already exists. Please enter a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                        txtProductName.Focus();
                        dbCon.CloseCon();
                        return;
                    }
                    else
                    {
                        cmd = new SqlCommand("spInsertProduct", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@ProdName", txtProductName.Text);
                        cmd.Parameters.AddWithValue("@ProdCatID", cbProductCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@ProdPrice", Convert.ToDecimal(txtProductPrice.Text));
                        cmd.Parameters.AddWithValue("@ProdQty", Convert.ToInt32(txtProductQuantity.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
                        }
                    }
                    dbCon.CloseCon();
                }
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
                dataGridView1.DataSource = dt;
                dbCon.OpenCon();
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtClear()
        {
            txtProductName.Clear();
            txtProductPrice.Clear();
            txtProductQuantity.Clear();
        }
        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProductID.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Product ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductName.Focus();
                    return;
                }
                if (txtProductName.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Product Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductName.Focus();
                    return;
                }
                if (Convert.ToDecimal(txtProductPrice.Text) < 0 || txtProductPrice.Text == string.Empty)
                {
                    MessageBox.Show("Please enter valid Product Price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductPrice.Focus();
                    return;
                }
                if (Convert.ToInt32(txtProductQuantity.Text) < 0 || txtProductQuantity.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Valid Product Quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductQuantity.Focus();
                    return;
                }
                else
                {
                    //SqlCommand cmd = new SqlCommand("spCheckDuplicateProduct", dbCon.GetCon());
                    //cmd.Parameters.AddWithValue("@ProdName", txtProductName.Text);
                    //cmd.Parameters.AddWithValue("@ProdCatID", cbProductCategory.SelectedValue);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //dbCon.OpenCon();
                    //var result = cmd.ExecuteScalar();
                    //if (result != null)
                    //{
                    //    MessageBox.Show("Product Name already exists. Please enter a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    txtClear();
                    //    txtProductName.Focus();
                    //    dbCon.CloseCon();
                    //    return;
                    //}
                    //else
                    //{
                        
                    //}
                    SqlCommand cmd = new SqlCommand("spUpdateProduct", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@ProdID", Convert.ToInt32(txtProductID.Text));
                    cmd.Parameters.AddWithValue("@ProdName", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatID", cbProductCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@ProdPrice", Convert.ToDecimal(txtProductPrice.Text));
                    cmd.Parameters.AddWithValue("@ProdQty", Convert.ToInt32(txtProductQuantity.Text));
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbCon.OpenCon();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Product Updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        BindProductList();
                        lblProductId.Visible = false;
                        txtProductID.Visible = false;
                        btnUpdateProduct.Visible = false;
                        btnDeleteProduct.Visible = false;
                        btnAddProduct.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Product Updated Failed....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtClear();
                    }
                    dbCon.CloseCon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProductID.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Product ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtProductID.Text != string.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Are you sure you want to delete this Product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        SqlCommand cmd = new SqlCommand("spDeleteProduct", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@ProdID", Convert.ToInt32(txtProductID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
                            btnUpdateProduct.Visible = false;
                            btnDeleteProduct.Visible = false;
                            btnAddProduct.Visible = true;
                            txtProductID.Visible = false;
                            lblProductId.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Product Delete Failed....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }
                        dbCon.CloseCon();
                    }
                    else
                    {
                        // User clicked No, cancel deletion
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            try
            {
                btnUpdateProduct.Visible = true;
                btnDeleteProduct.Visible = true;
                txtProductID.Visible = true;
                lblProductId.Visible = true;
                btnAddProduct.Visible = false;

                txtProductID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtProductName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                cbProductCategory.SelectedValue = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                txtProductPrice.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                txtProductQuantity.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void SearchByCategory()
        {
            SqlCommand cmd = new SqlCommand("spGetCategory", dbCon.GetCon());
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // 🔹 Add default option
            DataRow dr = dt.NewRow();
            dr["CatID"] = 0;
            dr["CategoryName"] = "Select Category";
            dt.Rows.InsertAt(dr, 0);

            cmbSearch.DataSource = dt;
            cmbSearch.DisplayMember = "CategoryName";
            cmbSearch.ValueMember = "CatID";
        }
        private void SearchedProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetAllProductList_SearchByCat", dbCon.GetCon());
                cmd.Parameters.AddWithValue("@ProdCatID",Convert.ToInt32(cmbSearch.SelectedValue));
                //cmd.Parameters.AddWithValue("@ProdCatID", cmbSearch.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                //dbCon.OpenCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
                if (cmbSearch.SelectedValue == null || cmbSearch.SelectedValue is DataRowView)
                return;

            int catID = (int)cmbSearch.SelectedValue;

            if (catID == 0)
                BindProductList();
            else
                SearchedProductList();
        }
        private void btnProductRefresh_Click(object sender, EventArgs e)
        {
            cmbSearch.SelectedIndex = 0;
            BindProductList();
        }
    }
}
