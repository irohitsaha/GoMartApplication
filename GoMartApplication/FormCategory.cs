using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoMartApplication
{
    public partial class FormCategory : Form
    {
        DBConnect dbCon = new DBConnect();
        public FormCategory()
        {
            InitializeComponent();
        }
        private void FormCategory_Load(object sender, EventArgs e)
        {
            btnUpdateCat.Visible = false;
            btnDeleteCat.Visible = false;
            txtCatID.Visible = false;
            lblCatId.Visible = false;
            bindCategory();
        }

        private void btnAddCat_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCatName.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Category Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCatName.Focus();
                    return;
                }
                if (rtxtCatDescription.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Category Description.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rtxtCatDescription.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("select CategoryName from tblCategory where CategoryName = @CategoryName", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@CategoryName", txtCatName.Text);
                    dbCon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Category Name already exists. Please enter a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                        txtCatName.Focus();
                        dbCon.CloseCon();
                        return;
                    }
                    else
                    {
                        cmd = new SqlCommand("spCatInsert", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@CategoryName", txtCatName.Text);
                        cmd.Parameters.AddWithValue("@CategoryDesc", rtxtCatDescription.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Category added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindCategory();
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
        private void txtClear()
        {
            txtCatName.Clear();
            rtxtCatDescription.Clear();
        }

        private void bindCategory()
        {
            SqlCommand cmd = new SqlCommand("select CatID as CategoryID,CategoryName, CategoryDesc as CategoryDescription from tblCategory ", dbCon.GetCon());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbCon.OpenCon();
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            btnUpdateCat.Visible = true;
            btnDeleteCat.Visible = true;
            txtCatID.Visible = true;
            lblCatId.Visible = true;
            btnAddCat.Visible = false;

            txtCatID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtCatName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            rtxtCatDescription.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        }
        private void btnUpdateCat_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCatID.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Category ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCatName.Focus();
                    return;
                }
                if (txtCatName.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Category Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCatName.Focus();
                    return;
                }
                else if (rtxtCatDescription.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Category Description.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rtxtCatDescription.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("select CategoryName from tblCategory where CategoryName = @CategoryName", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@CategoryName", txtCatName.Text);
                    dbCon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Category Name already exists. Please enter a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                        txtCatName.Focus();
                        dbCon.CloseCon();
                        return;
                    }
                    else
                    {
                        cmd = new SqlCommand("spCatUpdate", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(txtCatID.Text));
                        cmd.Parameters.AddWithValue("@CategoryName", txtCatName.Text);
                        cmd.Parameters.AddWithValue("@CategoryDesc", rtxtCatDescription.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        dbCon.CloseCon();
                        if (i > 0)
                        {
                            MessageBox.Show("Category Updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindCategory();
                            btnUpdateCat.Visible = false;
                            btnDeleteCat.Visible = false;
                            btnAddCat.Visible = true;
                            txtCatID.Visible = false;
                            lblCatId.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Category Updated Failed....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
                        }
                    }
                    dbCon.CloseCon();
                }
            }
            catch (Exception ex)
            { 
            MessageBox.Show( ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDeleteCat_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCatID.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Category ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(txtCatID.Text != string.Empty)
                {
                    if(DialogResult.Yes == MessageBox.Show("Are you sure you want to delete this category?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        SqlCommand cmd = new SqlCommand("spCatDelete", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@CatID", Convert.ToInt32(txtCatID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Category Deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindCategory();
                            btnUpdateCat.Visible = false;
                            btnDeleteCat.Visible = false;
                            btnAddCat.Visible = true;
                            txtCatID.Visible = false;
                            lblCatId.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Category Delete Failed....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show( ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
