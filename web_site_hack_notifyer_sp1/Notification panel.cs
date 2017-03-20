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

namespace web_site_hack_notifyer_sp1
{
    public partial class Notification_panel : MetroFramework.Forms.MetroForm
    {
        public string message { get; set; }
        public Notification_panel()
        {
            InitializeComponent();
            richTextBox1.ScrollToCaret();
            this.richTextBox1.Text = "";
            this.ControlBox = false;
        }

        private void Notification_panel_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(message);
            this.WindowState = FormWindowState.Normal;
            this.Focus();
            this.Activate();
            richTextBox1.AppendText(message);

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.richTextBox1.Text = "";
        }

        private void Notification_panel_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            this.richTextBox1.Text = "";
        }
    }
}
