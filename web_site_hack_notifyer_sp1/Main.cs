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
using System.Data.SqlClient;
using System.Web;
using HtmlAgilityPack;
using System.IO;
using System.Threading;
using System.Timers;



namespace web_site_hack_notifyer_sp1
{
    public partial class Main : MetroFramework.Forms.MetroForm
    {
        int firstTimeForm1;
        //System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        string url;
        public int row, row1, UrlBoxRow;

        public int UrlId, id, delete_id;
        int aa, EditTagNameAndTagValue;

        string b, c, d, e, f = "", line, valueatlast;
        string b1, c1, d1, e1, f1 = "";

        public System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        public Main(int x)
        {
            firstTimeForm1 = x;
            InitializeComponent();

            richTextBox1.ScrollToCaret();
            metroProgressBar1.Hide();
            this.Size = new Size(724, 492);

            metroButton8.TabStop = false;
            metroButton8.FlatStyle = FlatStyle.Flat;
            metroButton8.FlatAppearance.BorderSize = 0;
            metroButton8.FlatAppearance.BorderColor = Color.White;
            this.SizeChanged += new EventHandler(form1_sizeeventhandler);  
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        public void UseThreadForCheckingUrlTagAndValue1(string url)
        {
            string line = "====================================";
             HtmlWeb web = new HtmlWeb();
             HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

                            try
                            {
                                doc = web.Load("http://" + url);
                                SqlDbDataAccess dr = new SqlDbDataAccess();

                                using (SqlCommand cmd = dr.GetCommand("select dbo.site_details.tag_name,dbo.site_details.tag_value from dbo.site_details where dbo.site_details.site_id=(select dbo.site.id from dbo.site where dbo.site.url=@url);"))
                                {
                                    SqlParameter p = new SqlParameter("@url", SqlDbType.VarChar, 50);
                                    p.Value = url;

                                    cmd.Parameters.Add(p);

                                    cmd.Connection.Open();

                                    using (var reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            string tagname = reader["tag_name"].ToString();
                                            string tagvalue = reader["tag_value"].ToString();

                                            try
                                            {
                                                b1 = "URL:" + url + "\r\n\n" + "Tag Name:" + tagname + "\r\n\n" + "Tag Value:" + tagvalue + "\r\n\n";

                                                var link = doc.DocumentNode.SelectSingleNode(tagname);
                                                string ParsedtagValue = link.InnerText.ToString();

                                                if (ParsedtagValue == "")
                                                {
                                                    c1 = "CONDITION:" + "Tag not found" + "\n" + line + "\n\n";
                                                }
                                                else if (ParsedtagValue == tagvalue)
                                                {
                                                    d1 = "CONDITION:" + "GOOD" + "\n" + line + "\n\n";
                                                }
                                                else
                                                {
                                                    e1 = "CONDITION:" + "HACKED" + "\n" + line + "\n\n";
                                                }
                                            }
                                            catch (System.NullReferenceException)
                                            {
                                                f1 = "CONDITION:" + "Tag not found" + "\n" + line + "\n\n";
                                            }

                                            this.Invoke(new MethodInvoker(delegate()
                                            {
                                                richTextBox1.AppendText(b1 + c1 + d1 + e1 + f1 );
                                            }));

                                            b1 = "";
                                            c1 = "";
                                            d1 = "";
                                            e1 = "";
                                            f1 = "";
                                            line = "";
                                        }
                                    }
                                }
                            }
                            catch (System.Net.WebException)
                            {
                                MessageBox.Show(url + " might be temporarily down or it may have moved permanently to a new web address");
                                return;
                            }
        }



        private void DynamicThreading()
        {
            access_data ad = new access_data();
            DataTable t = new DataTable();
            t = ad.ReturnWebSiteUrl();

            for (int i = 0; i < metroGrid1.Rows.Count;i++ )
            {
                string x = t.Rows[i]["url"].ToString();
                Thread th = new Thread(() => { UseThreadForCheckingUrlTagAndValue(x); });
                th.IsBackground = true;
                th.Start();
            }
        }

        private string GetAllTagData()
        {
            DynamicThreading();
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("result.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    line = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                MetroMessageBox.Show(this, "File not found", "Wsh notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            File.WriteAllText("result.txt", String.Empty);
            valueatlast = line;
            return valueatlast;
        }

        private void UseThreadForCheckingUrlTagAndValue(string url)
        {
                            HtmlWeb web = new HtmlWeb();
                            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

                            try
                            {
                                doc = web.Load("http://" + url);
                                SqlDbDataAccess dr = new SqlDbDataAccess();

                                using (SqlCommand cmd = dr.GetCommand("select dbo.site_details.tag_name,dbo.site_details.tag_value from dbo.site_details where dbo.site_details.site_id=(select dbo.site.id from dbo.site where dbo.site.url=@url);"))
                                {
                                    SqlParameter p = new SqlParameter("@url", SqlDbType.VarChar, 50);
                                    p.Value = url;

                                    cmd.Parameters.Add(p);

                                    cmd.Connection.Open();

                                    using (var reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            string tagname = reader["tag_name"].ToString();
                                            string tagvalue = reader["tag_value"].ToString();

                                            try
                                            {
                                                b = "URL:" + url+Environment.NewLine+"Tag Name:" + tagname + System.Environment.NewLine + "Tag Value:" + tagvalue + System.Environment.NewLine;

                                                var link = doc.DocumentNode.SelectSingleNode(tagname);
                                                string ParsedtagValue = link.InnerText.ToString();

                                                if (ParsedtagValue == "")
                                                {
                                                    c = "CONDITION:" + "Tag not found" + System.Environment.NewLine;
                                                }
                                                else if (ParsedtagValue == tagvalue)
                                                {
                                                    d = "CONDITION:" + "GOOD" + System.Environment.NewLine;
                                                }
                                                else
                                                {
                                                    e = "CONDITION:" + "HACKED" + System.Environment.NewLine;
                                                }
                                            }
                                            catch (System.NullReferenceException)
                                            {
                                                f = "CONDITION:" + "Tag not found" + System.Environment.NewLine;
                                            }
                                            using (System.IO.StreamWriter n = new StreamWriter("result.txt", true))
                                            {
                                                n.WriteLine(b + c + d + e + f);
                                                n.Close();
                                            }
                                            b = "";
                                            c = "";
                                            d = "";
                                            e = "";
                                            f = "";
                                        }
                                    }
                                }
                            }
                            catch (System.Net.WebException)
                            {
                                MessageBox.Show(url + " might be temporarily down or it may have moved permanently to a new web address");
                                return;
                            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            metroTextBox1.Enabled = metroTextBox2.Enabled = metroTextBox3.Enabled = textBox1.Enabled = false;
            metroButton2.Enabled = metroButton4.Enabled = metroButton5.Enabled = false;
            metroGrid2.Enabled = false;
            richTextBox1.Clear();
            metroLabel6.Text = "";
            access_data ad = new access_data();
            metroGrid1.DataSource = ad.ReturnWebSiteUrl();

            UrlBoxRow = this.metroGrid1.RowCount;
            if (UrlBoxRow == 0)
            {
                textBox1.Enabled = false;
                metroTextBox1.Enabled = metroTextBox2.Enabled = metroTextBox3.Enabled = false;
                metroButton4.Enabled = metroButton5.Enabled = metroButton2.Enabled = metroButton10.Enabled = false;
                metroGrid2.Enabled = false;
            }

            DataGridViewButtonColumn bcol = new DataGridViewButtonColumn();
            bcol.HeaderText = "Click Me";
            bcol.Text = "Check";
            bcol.Name = "btnClickMe";
            bcol.UseColumnTextForButtonValue = true;
            metroGrid1.Columns.Add(bcol);

            string value = ad.CheckUpdateNotificationThroughEmail();
            if (value == "yes" && firstTimeForm1 == 0)
            {
                if (CheckForInternetConnection() == true)
                {
                    string email = ad.getemail();
                    if (ad.getemail() != "empty" && UrlBoxRow >= 1)
                    {
                        string x = ad.SendMail(GetAllTagData());
                        if(x!=String.Empty)
                        {
                            MetroMessageBox.Show(this,x,"Wsh notify",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MetroMessageBox.Show(this, "No internet connection available,please check your internet connection or troubleshoot your internet connection", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void minimize()
        {
            if (UrlBoxRow >= 1)
            {
                if (CheckForInternetConnection() == true)
                {
                    access_data dd = new access_data();
                    int jj = dd.GetNoficationInterval();
                    if (jj != 0)
                    {
                        myTimer.Tick += new EventHandler(myEvent);
                        myTimer.Interval = jj * 60 * 1000;
                        myTimer.Start();
                    }
                }
                else
                {
                    MessageBox.Show("No internet connection available,please check your internet connection or troubleshoot your internet connection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty url list.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void form1_sizeeventhandler(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                    this.Hide();
                    notifyIcon1.Visible = true;
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.BalloonTipTitle = "WSH notify";
                    notifyIcon1.BalloonTipText = "You have just minimized"+Environment.NewLine+"click on the notification icon to restore";
                    notifyIcon1.ShowBalloonTip(5000);

                    minimize();
            }
        }

        private void myEvent(object source,EventArgs e)
        {
            //Call the notification panel to display message
            Notification_panel pp = new Notification_panel();
            pp.message = GetAllTagData();
            pp.Show();
            //MessageBox.Show(GetAllTagData(),"WSH notify",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
        }

        private void metroGrid1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(metroGrid1.Rows.Count >= 1)
            {
                metroTextBox1.Enabled = metroTextBox2.Enabled = metroTextBox3.Enabled = textBox1.Enabled = true;
                metroButton2.Enabled = metroButton4.Enabled = metroButton5.Enabled = true;
                metroGrid2.Enabled = true;

                DataGridViewRow selectedrow = null;
                if (metroGrid1.SelectedRows.Count > 0)
                {
                    selectedrow = metroGrid1.SelectedRows[0];
                }
                if (selectedrow == null)
                    return;

                string x = selectedrow.Cells["URL"].Value.ToString();

                if (x == "")
                {
                    return;
                }
                else
                {
                    metroTextBox2.Text = "";
                    metroTextBox3.Text = "";
                    access_data d = new access_data();
                    d.get_info_by_url(x);
                    UrlId = d.GetIdByUsingUrl(x);

                    metroTextBox1.Text = d.a;
                    textBox1.Text = d.dd;
                    metroGrid2.DataSource = d.ReturnWebTagListAndName(UrlId);
                    aa = 0;
                }

            }
            
        }

        private static bool CheckForInternetConnection()   //for checking internet connection
        {
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {

        }

        private void CheckEmpty()
        {
            if (metroTextBox1.Text == "")
            {

                DialogResult d = MetroMessageBox.Show(this, "please select a url from web site list or if you already selected then you cant left [url and tag name and tag value] field empty", "WSH notify", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);

                if (d == DialogResult.Retry)
                {
                    CheckEmpty();
                }
                else
                    return;
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text == "")
            {
                CheckEmpty();
            }
            else
            {
                if(aa == 0)
                {
                    MetroMessageBox.Show(this, "please edit text first from [url or tag name or tag value or description]", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                site n = new site();

                n.url = metroTextBox1.Text;
                n.description = textBox1.Text;
                access_data ad = new access_data();



                DialogResult ff = MetroMessageBox.Show(this, "Are you really wants to change the information?", "WSH notify", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if(ff == DialogResult.OK)
                {
                    if (ad.Edit_web_info(n,UrlId) == true)
                    {
                        MetroMessageBox.Show(this, "information successfully update", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        access_data addd = new access_data();
                        metroGrid1.DataSource = addd.ReturnWebSiteUrl();
                    }
                    else
                    {
                        DialogResult dd = MetroMessageBox.Show(this, "something went wrong!", "WSH notify", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

                        if (dd == DialogResult.Cancel)
                        {
                            Application.Exit();
                        }
                    }
                }
                }
        }
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            aa = 1;
        }

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            aa = 1;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            aa = 1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            aa = 1;
        }

        private void metroGrid2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow selectedrow = null;
                if (metroGrid2.SelectedRows.Count > 0)
                {
                    selectedrow = metroGrid2.SelectedRows[0];
                }
                if (selectedrow == null)
                    return;

                string x = selectedrow.Cells["Tag Name"].Value.ToString();

                string y = selectedrow.Cells["Tag Value"].Value.ToString();

               if(x=="" && y=="")
               {
                   return;
               }
               else
               {
                   access_data d = new access_data();
                   id = d.GetSite_detailsIdByUsingTag_name(x);
                   metroTextBox2.Text = x;
                   metroTextBox3.Text = y;
                   EditTagNameAndTagValue = 0;
               }

        }

        private void metroGrid2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            access_data d = new access_data();
            string tag_name = "";
            if (metroTextBox2.Text.Contains("//"))
            {
                tag_name = metroTextBox2.Text;
            }
            else
            {
                tag_name = "//" + metroTextBox2.Text;
            }
            string tag_value = metroTextBox3.Text;
            if(tag_name=="" && tag_value=="")
            {
                MetroMessageBox.Show(this, "please insert tag name and tag value first", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
               if (d.CheckDataAvailableOrNot(tag_name,tag_value)==true)
                {
                    MetroMessageBox.Show(this, "Tag name andd value is already available", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(d.InsertNew_site_details(UrlId, tag_name, tag_value) == true)
                {
                    MetroMessageBox.Show(this, "Data successfully inserted", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    metroTextBox2.Text = metroTextBox3.Text = "";
                }
            }
            metroGrid2.DataSource = d.ReturnWebTagListAndName(UrlId);
        }

        private void metroTextBox2_TextChanged_1(object sender, EventArgs e)
        {
            EditTagNameAndTagValue = 1;
        }

        private void metroTextBox3_TextChanged(object sender, EventArgs e)
        {
            EditTagNameAndTagValue = 1;
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            access_data d = new access_data();
            string tag_name="";
            if(metroTextBox2.Text.Contains("//"))
            {
               tag_name= metroTextBox2.Text;
            }
            else
            {
                tag_name = "//"+metroTextBox2.Text;
            }
            string tag_value = metroTextBox3.Text;
            if (tag_name == "" && tag_value == "")
            {
                MetroMessageBox.Show(this, "please Select something from below table", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (EditTagNameAndTagValue == 0)
                {
                    MetroMessageBox.Show(this, "please edit tag name or value first", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (d.Update_site_details(id, tag_name, metroTextBox3.Text) == true)
                    {
                        MetroMessageBox.Show(this, "Information successfully updated", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        metroTextBox2.Text = metroTextBox3.Text = "";
                    }
                }
            }
            metroGrid2.DataSource = d.ReturnWebTagListAndName(UrlId);
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            access_data d = new access_data();
            if(d.DeleteRow(delete_id)==true)
            {
                MetroMessageBox.Show(this, "Information successfully deleted", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            metroGrid2.DataSource = d.ReturnWebTagListAndName(UrlId);
        }

        private void metroGrid2_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.Button==MouseButtons.Right)
            {
                DataGridViewRow selectedrow = null;
                if (metroGrid2.SelectedRows.Count > 0)
                {
                    selectedrow = metroGrid2.SelectedRows[0];
                }
                if (selectedrow == null)
                    return;
                string x = selectedrow.Cells["Tag Name"].Value.ToString();
                access_data d = new access_data();
                delete_id = d.GetSite_detailsIdByUsingTag_name(x);
                this.contextMenuStrip1.Show(this.metroGrid2,e.Location);
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void metroButton3_Click_1(object sender, EventArgs e)
        {
        }

        private void deleteRecordToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            access_data d = new access_data();
            if (d.DeleteUrl(url) == true)
            {
                MetroMessageBox.Show(this, "Information successfully deleted", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                metroGrid2.DataSource = null;
            }

            metroGrid1.DataSource = d.ReturnWebSiteUrl();

            UrlBoxRow = this.metroGrid1.RowCount;
            if (UrlBoxRow < 1)
            {
                textBox1.Enabled = false;
                metroTextBox1.Enabled = metroTextBox2.Enabled = metroTextBox3.Enabled = false;
                metroButton4.Enabled = metroButton5.Enabled = metroButton2.Enabled = false;
                metroGrid2.Enabled = false;
            }

            textBox1.Text = "";
            metroTextBox1.Text = "";
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

                url = selectedrow.Cells["URL"].Value.ToString();
                this.metroContextMenu1.Show(this.metroGrid1, e.Location);
                metroContextMenu1.Show(Cursor.Position);
            }
        }

        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                if(metroGrid1.Rows.Count > 0)
                {
                    if(CheckForInternetConnection() == true)
                    {
                         this.Hide();
                         check_in i = new check_in(UrlId);
                         i.Show();
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "No internet connection available,please check your internet connection or troubleshoot your internet connection", "WSH notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MetroMessageBox.Show(this, "No url available.plase add a new url first.", "Wsh notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            DialogResult d = MetroMessageBox.Show(this, "Do you wants to log out?", "WSH notify", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if(d == DialogResult.OK)
            {
                this.Hide();
                this.Dispose();
                Login n = new Login();
                n.Show();
            }
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Setting s = new Setting();
            s.Show();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            myTimer.Stop();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void metroContextMenu1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void metroButton3_Click_2(object sender, EventArgs e)
        {
            this.Hide();
            add_new n = new add_new();
            n.Show();
        }

        private void metroButton10_Click(object sender, EventArgs e)
        {
            this.Size = new Size(1006, 492);
            metroProgressBar1.Show();

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
                if(backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;    
                }
                else
                {
                    access_data ad = new access_data();
                    DataTable t = new DataTable();
                    t = ad.ReturnWebSiteUrl();
                    int count = metroGrid1.Rows.Count;
                    int value = 100 / count;
                    int xx = value;

                    for (int i = 0; i < metroGrid1.Rows.Count; i++)
                    {
                        
                        string x = t.Rows[i]["url"].ToString();
                        Thread th = new Thread(() => { UseThreadForCheckingUrlTagAndValue1(x); });
                        th.IsBackground = true;
                        th.Start();

                        Thread.Sleep(2000);

                        this.Invoke(new MethodInvoker(delegate()
                        {
                            metroLabel6.Text = "Checking URL:" + x + " Completed:" + value + " %";
                        }));

                        backgroundWorker1.ReportProgress(value);

                        value = value + xx;
                    }
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        richTextBox1.AppendText("=========================================\n" +
                                                "=========================================\n" +
                                                "=========================================\n");
                    }));

                    this.Invoke(new MethodInvoker(delegate()
                    {
                        metroLabel6.Text = "Completed...........";
                    }));

                }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            metroProgressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void metroButton11_Click(object sender, EventArgs e)
        {
            metroLabel6.Text = "";
            richTextBox1.Clear();
            backgroundWorker1.CancelAsync();
            metroProgressBar1.Hide();
            this.Size = new Size(724, 492);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MetroMessageBox.Show(this, "Do you wants to exit", "WSH notify", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                e.Cancel = false;
                this.Activate();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(0);
        }

    }
}
