using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace GoMartApplication
{
    public partial class Form1 : Form
    {
        DBConnect dbCon = new DBConnect();

        public static string loginName, loginType;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbRole.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRole.SelectedIndex > 0)
                {
                    if(txtUserName.Text == string.Empty)
                    {
                        MessageBox.Show("Please enter valid username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtUserName.Focus();
                        return;
                    }
                    if (txtPassword.Text == string.Empty)
                    {
                        MessageBox.Show("Please enter valid password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPassword.Focus();
                        return;
                    }
                    if (cmbRole.SelectedIndex > 0 && txtUserName.Text != string.Empty && txtPassword.Text != string.Empty)
                    {
                        //Login Code
                        if(cmbRole.Text== "Admin")
                        {
                            SqlCommand cmd = new SqlCommand("select top 1 AdminID, Password, FullName from tblAdmin where AdminID =@AdminID and Password =@Password", dbCon.GetCon());

                            cmd.Parameters.AddWithValue("@AdminID", txtUserName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password",txtPassword.Text.Trim());

                            dbCon.OpenCon();

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if(dt.Rows.Count > 0)
                            {
                                MessageBox.Show("Login Success Welcome to Home Page", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                loginName = txtUserName.Text.Trim();
                                loginType = cmbRole.Text;
                                clrValue();
                                this.Hide();
                                FormMain fm = new FormMain();
                                fm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Login Failed Please check the UserName or Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else if(cmbRole.Text == "Seller")
                        {
                            SqlCommand cmd = new SqlCommand("select top 1 SellerName,SellerPass from tblSeller where SellerName=@SellerName and SellerPass=@SellerPass\r\n", dbCon.GetCon());

                            cmd.Parameters.AddWithValue("@SellerName", txtUserName.Text.Trim());
                            cmd.Parameters.AddWithValue("@SellerPass", txtPassword.Text.Trim());

                            dbCon.OpenCon();

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("Login Success Welcome to Home Page", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                loginName = txtUserName.Text.Trim();
                                loginType = cmbRole.Text;
                                clrValue();
                                this.Hide();
                                FormMain fm = new FormMain();
                                fm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Login Failed Please check the UserName or Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter valid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clrValue();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a role to login.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clrValue();
                }
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void clrValue()
        {
            cmbRole.SelectedIndex = 0;
            txtUserName.Clear();
            txtPassword.Clear();
        }
    }
}
