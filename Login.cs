using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automated_MCQ_Grading_UI
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            const string uname = "admin";
            const string pword = "admin";

            if(txtBxUserName.Text == uname && txtBxPassword.Text==pword )
            {
                this.Hide();
                frmHome ob = (frmHome)Application.OpenForms["frmHome"];
                if (ob != null)
                {
                    ob.Show();
                }
                else
                {
                    ob = new frmHome();
                    ob.Show();
                }
                
            }
            else
            {
                MessageBox.Show("Invalid Credentials!", "Login error");
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
