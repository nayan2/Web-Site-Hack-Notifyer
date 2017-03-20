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
using System.IO;
using System.Web;
using HtmlAgilityPack;
using System.Data.SqlClient;

namespace web_site_hack_notifyer_sp1
{
    public partial class Login : MetroFramework.Forms.MetroForm
    {
        public Login()
        {
            InitializeComponent();

            access_data d = new access_data();
            string password = d.GetAdminPassword();

            if (password == "empty")
            {
                metroPanel1.Controls.Add(new SetFirstTimePassword());
            }
            else
            {
                metroPanel1.Controls.Add(new MainLogin());
            }

        }

        private void login_Load(object sender, EventArgs e)
        {

        }
        private void metroButton2_Click(object sender, EventArgs e)
        {

        }

        private void login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
            Application.Exit();
        }

        private void metroButton2_Click_1(object sender, EventArgs e)
        {

        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void connectionOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionLink l = new ConnectionLink();
            l.ShowDialog();
        }
    }
}
