using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoMartApplication
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (Form1.loginName != null)
            {
                toolStripStatusLabel2.Text = Form1.loginName;
            }

            if (Form1.loginType != null && Form1.loginType == "Seller")
            {
                {
                    categoryToolStripMenuItem.Enabled = false;
                    productToolStripMenuItem.Enabled = false;
                    addUserToolStripMenuItem.Enabled = false;
                }

            }

            // Welcome Message
            if (Form1.loginType == "Admin")
            {
                lblWelcomeTitle.Text = "👑 Welcome Admin " + Form1.loginName;
                lblWelcomeSub.Text = "You can manage categories, products, sellers, and reports.";
            }
            else if (Form1.loginType == "Seller")
            {
                lblWelcomeTitle.Text = "🛒 Welcome Seller " + Form1.loginName;
                lblWelcomeSub.Text = "Start selling products and manage customer orders.";
            }


        }
        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCategory fcat = new FormCategory();
            fcat.Show();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 abt = new AboutBox1();
            abt.Show();
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(dialog == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        private void sellerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAddSeller addSeller = new FormAddSeller();
            addSeller.Show();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAddAdmin addAdmin = new FormAddAdmin();
            addAdmin.Show();
        }

        private void addProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAddProduct addProduct = new FormAddProduct();
            addProduct.Show();
        }

        private void sellerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormSelling selling = new FormSelling();
            selling.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReport report = new FormReport();
            report.Show();
        }
    }
}
