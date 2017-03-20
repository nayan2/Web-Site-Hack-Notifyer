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
using HtmlAgilityPack;
using System.Web;
using System.Data.SqlClient;
using System.Threading;

namespace web_site_hack_notifyer_sp1
{
    public partial class check_in : MetroFramework.Forms.MetroForm
    {
        int id;

        public check_in(int id)
        {
            this.id = id;
            InitializeComponent();
            this.SizeChanged += new EventHandler(form1_sizeeventhandler);
        }

        private void form1_sizeeventhandler(object sender, EventArgs e)
        {
            while (this.WindowState == FormWindowState.Normal)
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

        public void CheckTagValue()
        {
            access_data aa = new access_data();
            try
            {
                aa.get_info_by_url(aa.GetUrlByUsingId(id));

                metroGrid1.DataSource = aa.CheckEveryTagValue(aa.GetUrlByUsingId(id));
                HtmlWeb web = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = web.Load("http://" + aa.a);

                DataGridViewImageColumn delbut = new DataGridViewImageColumn();
                delbut.HeaderText = "Result";
                delbut.Width = 20;
                delbut.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                metroGrid1.Columns.Add(delbut);

                int i = metroGrid1.Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    try
                    {
                        string tagname = metroGrid1.Rows[j].Cells["Tag Name"].Value.ToString();
                        string tagvalue = metroGrid1.Rows[j].Cells["Tag Value"].Value.ToString();

                        var link = doc.DocumentNode.SelectSingleNode(tagname);
                        string ParsedValue = link.InnerText.ToString();

                        if (ParsedValue == "")
                        {
                            metroGrid1.Rows[j].Cells[2].Value = Image.FromFile(Environment.CurrentDirectory + "/notavailable.png");
                        }
                        else if (ParsedValue == tagvalue)
                        {
                            metroGrid1.Rows[j].Cells[2].Value = Image.FromFile(Environment.CurrentDirectory + "/correct.png");
                        }
                        else
                        {
                            metroGrid1.Rows[j].Cells[2].Value = Image.FromFile(Environment.CurrentDirectory + "/invalid.png");
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        metroGrid1.Rows[j].Cells[2].Value = Image.FromFile(Environment.CurrentDirectory + "/notavailable.png");
                    }
                }
            }
            catch (System.Net.WebException)
            {
                MetroMessageBox.Show(this, "The webpage at " + aa.a + " might be temporarily down or it may have moved permanently to a new web address", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void check_in_Load(object sender, EventArgs e)
        {
            Main n = new Main(1);
            
            access_data a=new access_data();

            if (a.GetUrlByUsingId(id)=="empty")
            {
                MetroMessageBox.Show(this,"No url available.plase add a new url first.","Wsh notify",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                metroTextBox1.Text = a.GetUrlByUsingId(id);
                CheckTagValue();
            }
        }

        private void metroButton2_Click_2(object sender, EventArgs e)
        {
            this.Hide();
            Main n = new Main(1);
            n.Show();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Main n = new Main(1);
            n.myTimer.Stop();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void check_in_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            new Main(1).Show();
        }

        private void check_in_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            new Main(1).Show();
        }
    }
}
