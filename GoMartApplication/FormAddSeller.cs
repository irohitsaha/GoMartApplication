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
    public partial class FormAddSeller : Form
    {
        DBConnect dbCon = new DBConnect();
        public FormAddSeller()
        {
            InitializeComponent();
        }
        private void FormAddSeller_Load(object sender, EventArgs e)
        {
            lblSellerId.Visible = false;
            txtSellerID.Visible = false;
            btnDeleteSeller.Visible = false;
            btnUpdateSeller.Visible = false;
            btnAddSeller.Visible = true;
            bindSeller();
            txtSellerName.Focus();
        }
        private void btnAddSeller_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSellerName.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Seller Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSellerName.Focus();
                    return;
                }
                if (txtSellerPassword.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSellerPassword.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SELECT SellerName FROM tblSeller WHERE SellerName = @SellerName", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                    dbCon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Seller Name already exists. Please enter a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                        txtSellerName.Focus();
                        dbCon.CloseCon();
                        return;
                    }
                    else
                    {
                        cmd = new SqlCommand("spSellerInsert", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                        cmd.Parameters.AddWithValue("@SellerAge", Convert.ToInt32(txtSellerAge.Text));
                        cmd.Parameters.AddWithValue("@SellerPhone", txtSellerPh.Text);
                        cmd.Parameters.AddWithValue("@SellerPass", txtSellerPassword.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Seller added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindSeller();
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
        private void btnUpdateSeller_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSellerID.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Seller ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSellerName.Focus();
                    return;
                }
                if (txtSellerName.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Seller Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSellerName.Focus();
                    return;
                }
                else if (txtSellerPassword.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSellerPassword.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SELECT SellerName FROM tblSeller WHERE SellerName = @SellerName", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                    dbCon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Seller Name already exists. Please enter a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                        txtSellerName.Focus();
                        dbCon.CloseCon();
                        return;
                    }
                    else
                    {
                        cmd = new SqlCommand("spSellerUpadte", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@SellerID",Convert.ToInt32(txtSellerID.Text));
                        cmd.Parameters.AddWithValue("@SellerName", txtSellerName.Text);
                        cmd.Parameters.AddWithValue("@SellerAge", Convert.ToInt32(txtSellerAge.Text));
                        cmd.Parameters.AddWithValue("@SellerPhone", txtSellerPh.Text);
                        cmd.Parameters.AddWithValue("@SellerPass", txtSellerPassword.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        dbCon.CloseCon();
                        if (i > 0)
                        {
                            MessageBox.Show("Seller Updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindSeller();
                            btnUpdateSeller.Visible = false;
                            btnDeleteSeller.Visible = false;
                            btnAddSeller.Visible = true;
                            txtSellerID.Visible = false;
                            lblSellerId.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Seller Updated Failed....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClear();
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
        private void btnDeleteSeller_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSellerID.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Seller ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtSellerID.Text != string.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Are you sure you want to delete this Seller?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        SqlCommand cmd = new SqlCommand("spSellerDelete", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@SellerID", Convert.ToInt32(txtSellerID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Seller Deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindSeller();
                            btnUpdateSeller.Visible = false;
                            btnDeleteSeller.Visible = false;
                            btnAddSeller.Visible = true;
                            txtSellerID.Visible = false;
                            lblSellerId.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Seller Delete Failed....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void bindSeller()
        {
            SqlCommand cmd = new SqlCommand("select * from tblSeller ", dbCon.GetCon());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbCon.OpenCon();
        }
        
        private void txtClear()
        {
            txtSellerName.Clear();
            txtSellerAge.Clear();
            txtSellerPh.Clear();    
            txtSellerPassword.Clear();
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            btnUpdateSeller.Visible = true;
            btnDeleteSeller.Visible = true;
            txtSellerID.Visible = true;
            lblSellerId.Visible = true;
            btnAddSeller.Visible = false;

            txtSellerID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtSellerName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtSellerAge.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txtSellerPh.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtSellerPassword.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }
    }
}
