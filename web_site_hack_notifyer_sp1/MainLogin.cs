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
    public partial class MainLogin : UserControl
    {
        public MainLogin()
        {
            InitializeComponent();
            metroTextBox2.UseSystemPasswordChar = true;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            access_data d = new access_data();
            string selected = this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem);
            if (selected == "" || metroTextBox2.Text == "")
            {
                MetroMessageBox.Show(this, "Please select username and password first", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (selected == "admin" && metroTextBox2.Text == d.GetAdminPassword())
                {
                    this.ParentForm.Hide();
                    Main n = new Main(0);
                    n.Show();
                }
                else
                {
                    MetroMessageBox.Show(this, "Invalid username and password Or check your database connection link[Tools->Connection option]!", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void metroTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                access_data d = new access_data();
                string selected = this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem);
                if (selected == "" || metroTextBox2.Text == "")
                {
                    MetroMessageBox.Show(this, "Please select username and password first", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (selected == "admin" && metroTextBox2.Text == d.GetAdminPassword())
                    {
                        this.ParentForm.Hide();
                        Main n = new Main(0);
                        n.Show();
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "Invalid username and password!", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Mainlogin_Load(object sender, EventArgs e)
        {

        }

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(metroCheckBox1.Checked == true)
            {
                metroTextBox2.UseSystemPasswordChar = false;
                this.metroTextBox2.Text = this.metroTextBox2.Text;
            }
            else
            {
                metroTextBox2.UseSystemPasswordChar = true;
                this.metroTextBox2.Text = this.metroTextBox2.Text;
            }
        }
    }
}
