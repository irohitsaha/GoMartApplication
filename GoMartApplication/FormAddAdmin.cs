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
    public partial class FormAddAdmin : Form
    {
        DBConnect dbCon = new DBConnect();
        public FormAddAdmin()
        {
            InitializeComponent();
        }

        private void btnAddAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdminName.Text == string.Empty || txtAdminUserID.Text == string.Empty || txtAdminPassword.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Valid Admin Name, UserId, Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClear();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("select AdminID from tblAdmin where AdminID=@ID", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@ID", txtAdminUserID.Text);
                    dbCon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Admin UserID already exists. Please enter a different UserID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                        txtAdminUserID.Focus();
                        dbCon.CloseCon();
                        return;
                    }
                    else
                    {
                        cmd = new SqlCommand("spAddAdmin", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@AdminID", txtAdminUserID.Text);
                        cmd.Parameters.AddWithValue("@Password", txtAdminPassword.Text);
                        cmd.Parameters.AddWithValue("@FullName", txtAdminName.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Admin added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindAdmin();
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
        private void FormAddAdmin_Load(object sender, EventArgs e)
        {
            lblAdminId.Visible = false;
            txtAdminID.Visible = false;
            btnDeleteAdmin.Visible = false;
            btnUpdateAdmin.Visible = false;
            btnAddAdmin.Visible = true;
            bindAdmin();
            txtAdminName.Focus();
        }
        private void txtClear()
        {
            txtAdminName.Clear();
            txtAdminUserID.Clear();
            txtAdminPassword.Clear();
            txtAdminName.Focus();
        }

        private void bindAdmin()
        {
            SqlCommand cmd = new SqlCommand("select * from tblAdmin ", dbCon.GetCon());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbCon.OpenCon();
        }

        private void btnUpdateAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdminName.Text == string.Empty || txtAdminUserID.Text == string.Empty || txtAdminPassword.Text == string.Empty || txtAdminID.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Valid Admin Name, UserId, Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClear();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("spUpdateAdmin", dbCon.GetCon());
                    dbCon.OpenCon();
                    cmd.Parameters.AddWithValue("@AdminID", txtAdminID.Text);
                    cmd.Parameters.AddWithValue("@Password", txtAdminPassword.Text);
                    cmd.Parameters.AddWithValue("@FullName", txtAdminName.Text);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int i = cmd.ExecuteNonQuery();
                    dbCon.CloseCon();
                    if (i > 0)
                    {
                        MessageBox.Show("Admin Updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        bindAdmin();
                        btnUpdateAdmin.Visible = false;
                        btnDeleteAdmin.Visible = false;
                        btnAddAdmin.Visible = true;
                        txtAdminID.Visible = false;
                        lblAdminId.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Admin Updated Failed....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnDeleteAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdminID.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Admin Record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtAdminID.Text != string.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Are you sure you want to delete this Admin?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        SqlCommand cmd = new SqlCommand("spDeleteAdmin", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@AdminID", txtAdminID.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Admin Deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            bindAdmin();
                            btnUpdateAdmin.Visible = false;
                            btnDeleteAdmin.Visible = false;
                            btnAddAdmin.Visible = true;
                            txtAdminID.Visible = false;
                            lblAdminId.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Admin Delete Failed....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                btnUpdateAdmin.Visible = true;
                btnDeleteAdmin.Visible = true;
                txtAdminID.Visible = true;
                lblAdminId.Visible = true;
                btnAddAdmin.Visible = false;

                txtAdminID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtAdminUserID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                txtAdminPassword.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                txtAdminName.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
