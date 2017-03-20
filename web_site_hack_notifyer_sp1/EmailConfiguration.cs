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
using System.Text.RegularExpressions;
using System.Threading;
using System.Data.SqlClient;

namespace web_site_hack_notifyer_sp1
{
    public partial class EmailConfiguration : MetroFramework.Forms.MetroForm
    {
        string e_to;

        string nofify;
        public EmailConfiguration(string nofify)
        {

            this.nofify = nofify;
            InitializeComponent();
            metroTextBox2.UseSystemPasswordChar = true;
            metroComboBox1.Items.Add("true");
            metroComboBox1.Items.Add("false");
            metroComboBox2.Items.Add("true");
            metroComboBox2.Items.Add("false");
            this.SizeChanged += new EventHandler(form1_sizeeventhandler);
            metroGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        public bool IsEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            { return false; }
            try
            {
                Regex _regex = new Regex("^((([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])" +
                        "+(\\.([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)" +
                        "((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|" +
                        "[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\u" +
                        "FDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|" +
                        "(([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|" +
                        "[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|[\\u00A0-\\uD7FF\\uF900" +
                        "-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFF" +
                        "EF])))\\.?$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
                return _regex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }


        private void EmailConfiguration_Load(object sender, EventArgs e)
        {
            access_data d = new access_data();
            user_details u = new user_details();

            d.GetEmailConfigInfo(u);

            metroTextBox1.Text = u.e_from;
            metroTextBox2.Text = u.e_password;
            metroTextBox3.Text = Convert.ToString(u.port);
            metroComboBox1.SelectedIndex = metroComboBox1.FindStringExact(u.enablessi);
            metroComboBox2.SelectedIndex = metroComboBox2.FindStringExact(u.usedefaultcredentials);
            metroGrid1.DataSource = d.ReturnEmailToCc();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            string EnableSsl = this.metroComboBox1.GetItemText(this.metroComboBox1.SelectedItem);
            string UseDefaultCredentials = this.metroComboBox2.GetItemText(this.metroComboBox2.SelectedItem);


            if (metroTextBox1.Text == "" || metroTextBox2.Text == "" || metroTextBox3.Text == "" || EnableSsl == "" || UseDefaultCredentials == "" || metroGrid1.Rows.Count < 1)
            {
                MetroMessageBox.Show(this,"You cant place any above field ampty.[Multiple receiving email address acceptable but at least one receiving email address must be given]","WSH notify",MessageBoxButtons.OK,MessageBoxIcon.Error);
                metroTextBox1.Text = metroTextBox2.Text = metroTextBox3.Text = EnableSsl = UseDefaultCredentials = "";
            }
            else
            {
                if (IsEmail(metroTextBox1.Text) == false || IsEmail(metroTextBox4.Text) == false)
                {
                    MetroMessageBox.Show(this, "Invalid Email Address!", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    metroTextBox1.Text = metroTextBox2.Text = metroTextBox3.Text = EnableSsl = UseDefaultCredentials = "";
                }
                else
                {
                    SqlDbDataAccess dr = new SqlDbDataAccess();
                    access_data d = new access_data();
                    user_details u = new user_details();
                    u.e_from = metroTextBox1.Text;
                    u.e_password = metroTextBox2.Text;
                    u.port = Convert.ToInt32(metroTextBox3.Text);
                    u.enablessi = EnableSsl;
                    u.usedefaultcredentials = UseDefaultCredentials;
                    d.InsertEmailConfiguration(u);

                    d.InsertNewToCC(metroTextBox4.Text);
                    metroGrid1.DataSource = d.ReturnEmailToCc();

                    d.UpdateNotificationThroughEmail(nofify);
                    MetroMessageBox.Show(this, "Information successfully updated", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    metroTextBox4.Text = "";
                }
            }
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Setting nn = new Setting();
            nn.Show();
        }

        private void metroTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void metroLabel5_Click(object sender, EventArgs e)
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

        private void metroButton3_Click(object sender, EventArgs e)
        {
            string email = metroTextBox4.Text;
            if(email == "")
            {
                MetroMessageBox.Show(this,"Please add a email first in to box","WSH notify",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                if(IsEmail(metroTextBox4.Text))
                {
                    access_data d = new access_data();

                    d.InsertNewToCC(metroTextBox4.Text);
                    metroGrid1.DataSource = d.ReturnEmailToCc();
                    /////////////////////////
                    MetroMessageBox.Show(this, "Email successfully addeded", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MetroMessageBox.Show(this, "Invalid email address", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    metroTextBox4.Text = "";
                }
            }
        }

        private void metroGrid1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridViewRow selectedrow = null;
                if (metroGrid1.SelectedRows.Count > 0)
                {
                    selectedrow = metroGrid1.SelectedRows[0];
                }
                if (selectedrow == null)
                    return;

                e_to = selectedrow.Cells["To"].Value.ToString();
                this.metroContextMenu1.Show(this.metroGrid1, e.Location);
                metroContextMenu1.Show(Cursor.Position);
            }
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            access_data d = new access_data();
            if (d.DeleteTo(e_to) == true)
            {
                MetroMessageBox.Show(this, "Information successfully deleted", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                metroGrid1.DataSource = null;
            }

            metroGrid1.DataSource = d.ReturnEmailToCc();
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

        private void metroTextBox3_Click(object sender, EventArgs e)
        {

        }

        private void EmailConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void EmailConfiguration_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            new Main(1).Show();
        }

        private void metroContextMenu1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
