using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using DbConnectionSample.DataAccess;
using DbConnectionSample.Entities;
using DbConnectionSample.Framework;
using System.Threading;

namespace web_site_hack_notifyer_sp1
{
    public partial class Setting : MetroFramework.Forms.MetroForm
    {
        public Setting()
        {
            InitializeComponent();
            this.SizeChanged += new EventHandler(form1_sizeeventhandler);
        }

        private void form1_sizeeventhandler(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Normal;
            }
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.BalloonTipTitle = "WSH notify";
                notifyIcon1.BalloonTipText = "You have just minimized" + Environment.NewLine + "click on the notification icon to restore";
                notifyIcon1.ShowBalloonTip(5000);

                new Main(1).minimize();
            }
        }


        private void setting_Load(object sender, EventArgs e)
        {
            access_data d = new access_data();
            string checkboxvalue=d.CheckUpdateNotificationThroughEmail();
            if (checkboxvalue == "yes")
            {
                metroCheckBox1.Checked = true;
            }
            if (checkboxvalue == "no")
            {
                metroCheckBox1.Checked = false;
            }
            access_data dd = new access_data();
            int jj = dd.GetNoficationInterval();
            metroTextBox4.Text = jj.ToString();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            access_data d=new access_data();
            string currentpassword=d.GetAdminPassword();

            if(metroTextBox1.Text=="" || metroTextBox2.Text=="" || metroTextBox3.Text=="")
            {
                MetroMessageBox.Show(this, "You cant place old password or new password or confirm password filed empty", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                metroTextBox1.Text = "";
                metroTextBox2.Text = "";
                metroTextBox3.Text = "";
            }
            else
            {
                if(metroTextBox2.Text != metroTextBox3.Text)
                {
                    MetroMessageBox.Show(this, "New password and confirm password is not match", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    metroTextBox1.Text = "";
                    metroTextBox2.Text = "";
                    metroTextBox3.Text = "";
                }
                else if (metroTextBox1.Text != currentpassword)
                {
                    MetroMessageBox.Show(this, "Old password not not correct", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    metroTextBox1.Text = "";
                    metroTextBox2.Text = "";
                    metroTextBox3.Text = "";
                }
                else
                {
                    if (d.changepassord(metroTextBox3.Text.ToString()) == true)
                    {
                        MetroMessageBox.Show(this, "Password successfully change", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        metroTextBox1.Text = "";
                        metroTextBox2.Text = "";
                        metroTextBox3.Text = "";
                    }
                }
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main n = new Main(1);
            n.Show();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            access_data d = new access_data();

            if (metroCheckBox1.Checked == true)
            {
                this.Hide();
                EmailConfiguration n = new EmailConfiguration("yes");
                n.Show();
            }
            else
            {
                d.UpdateNotificationThroughEmail("no");
                MetroMessageBox.Show(this, "Information successfully saved", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void metroPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Main n = new Main(1);
            n.myTimer.Stop();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(metroCheckBox1.Checked==true)
            {
                metroButton2.Size = new Size(136, 26);
                metroButton2.Location= new Point(318, 48);

                metroButton2.Text = "Change email setting";
            }
            if (metroCheckBox1.Checked == false)
            {
                metroButton2.Size = new Size(90, 26);
                metroButton2.Location = new Point(364, 48);

                metroButton2.Text = "Apply";
            }
        }

        private void metroTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            if(metroTextBox4.Text == "")
            {
                access_data d=new access_data();
                d.InsertNoficationInterval(0);
            }
            else
            {
                access_data d = new access_data();
                d.InsertNoficationInterval(Convert.ToInt32(metroTextBox4.Text));
            }
            MetroMessageBox.Show(this, "Information successfully updated", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Setting_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            new Main(1).Show();
        }

    }
}
