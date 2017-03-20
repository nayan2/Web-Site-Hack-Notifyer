using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using DbConnectionSample.DataAccess;

namespace web_site_hack_notifyer_sp1
{
    public partial class SetFirstTimePassword : UserControl
    {
        public SetFirstTimePassword()
        {
            InitializeComponent();
            metroTextBox2.PasswordChar = '*';
            metroTextBox1.PasswordChar = '*';
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == "" || metroTextBox2.Text == "")
            {
                MetroMessageBox.Show(this, "You cant not place new password and confirm password field empty.", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (metroTextBox1.Text != metroTextBox2.Text)
                {
                    MetroMessageBox.Show(this, "New password and confirm password field value not match.", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    access_data f = new access_data();
                    if (f.changepassord(metroTextBox2.Text) == true)
                    {
                        MetroMessageBox.Show(this, "Password successfully set.", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.ParentForm.Hide();
                        Main n = new Main(0);
                        n.Show();
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "Something went wrong.please try again after some time.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void metroTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (metroTextBox1.Text == "" || metroTextBox2.Text == "")
                {
                    MetroMessageBox.Show(this, "You cant not place new password and confirm password field empty.", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (metroTextBox1.Text != metroTextBox2.Text)
                    {
                        MetroMessageBox.Show(this, "New password and confirm password field value not match.", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        access_data f = new access_data();
                        if (f.changepassord(metroTextBox2.Text) == true)
                        {
                            MetroMessageBox.Show(this, "Password successfully set.", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.ParentForm.Hide();
                            Main n = new Main(0);
                            n.Show();
                        }
                        else
                        {
                            MetroMessageBox.Show(this, "Something went wrong.please try again after some time.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void Setfirsttimepassword_Load(object sender, EventArgs e)
        {

        }
    }
}
