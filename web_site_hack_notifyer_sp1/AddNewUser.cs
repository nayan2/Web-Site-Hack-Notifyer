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
using System.Data.SqlClient;
using DbConnectionSample.DataAccess;
using DbConnectionSample.Entities;
using DbConnectionSample.Framework;
using System.Threading;

namespace web_site_hack_notifyer_sp1
{
    public partial class add_new : MetroFramework.Forms.MetroForm
    {
        DataGridViewRow item;
        DataTable dt = new DataTable();
        DataRow dr;
        public add_new()
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

        private void add_new_Load(object sender, EventArgs e)
        {  
            dt.Columns.Add("Tag Name");
            dt.Columns.Add("Tag Value");
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main n = new Main(1);
            n.Show();
        }
        public void CheckEmpty()
        {
            try
            {
                if (metroTextBox1.Text == "" || metroGrid1.Rows[0].Cells["Tag Name"].Value.ToString() == "" || metroGrid1.Rows[0].Cells["Tag Value"].Value.ToString() == "")
                {
                    DialogResult d = MetroMessageBox.Show(this, "please place the empty field first[url and tag name and tag value]", "WSH notify", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);

                    if (d == DialogResult.Retry)
                    {
                        CheckEmpty();
                    }
                    else
                        return;
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                DialogResult d = MetroMessageBox.Show(this, "please place the empty field first[url and tag name  and tag value] or if you are already filled every field value then click add button first", "WSH notify", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);

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
            try
            {
                if (metroTextBox1.Text == "" || metroGrid1.Rows[0].Cells["Tag Name"].Value.ToString() == "" || metroGrid1.Rows[0].Cells["Tag Value"].Value.ToString() == "")
                {
                    CheckEmpty();
                }
                else
                {
                    DialogResult f = MetroMessageBox.Show(this, "Are you sure to save?", "WSH notify", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (f == DialogResult.OK)
                    {
                        access_data d = new access_data();
                        site s = new site();

                        s.url = metroTextBox1.Text;
                        s.description = textBox1.Text;

                        int site_id=d.InsertNweUrlData(s);
                        int i = metroGrid1.Rows.Count;

                        SqlDbDataAccess dr = new SqlDbDataAccess();
                        for(int j = 0; j < i ; j++)
                        {
                                using (SqlCommand cmd = dr.GetCommand("insert into dbo.site_details (site_id,tag_name,tag_value) values(@site_id,@tag_name,@tag_value);"))
                                {
                                    SqlParameter p=new SqlParameter("@site_id",SqlDbType.Int);
                                    p.Value = site_id;

                                    SqlParameter p1=new SqlParameter("@tag_name",SqlDbType.VarChar,200);
                                    p1.Value=metroGrid1.Rows[j].Cells["Tag Name"].Value.ToString();

                                    SqlParameter p2=new SqlParameter("@tag_value",SqlDbType.VarChar,500);
                                    p2.Value=metroGrid1.Rows[j].Cells["Tag Value"].Value.ToString();

                                    cmd.Parameters.Add(p);
                                    cmd.Parameters.Add(p1);
                                    cmd.Parameters.Add(p2);

                                    cmd.Connection.Open();
                                    cmd.ExecuteNonQuery();
                                    cmd.Connection.Close();

                                }
                        }

                        MetroMessageBox.Show(this, "Data successfully Saved", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        metroTextBox1.Text = "";
                        metroTextBox2.Text = "";
                        textBox1.Text = "";
                        textBox2.Text = "";
                        metroGrid1.DataSource = null;
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                CheckEmpty();
            }
        }

        private void clear()
        {
            metroTextBox2.Text = "";
            textBox2.Text = "";
            
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            string tag_name = metroTextBox2.Text;
            string tag_value = textBox2.Text;

            if(tag_name=="" || tag_value=="")
            {
                MetroMessageBox.Show(this, "Please insert the Tag name and value first", "WSH notify", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                dr = dt.NewRow();
                if (metroTextBox2.Text.Contains("//"))
                {
                    dr["Tag Name"] = metroTextBox2.Text;
                }
                else
                {
                    dr["Tag Name"] = "//" + metroTextBox2.Text;
                }
                dr["Tag Value"] = textBox2.Text;
                dt.Rows.Add(dr);
                metroGrid1.DataSource = dt;
                clear();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Main n = new Main(1);
            n.myTimer.Stop();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            metroGrid1.Rows.RemoveAt(item.Index);
        }

        private void metroGrid1_KeyDown(object sender, KeyEventArgs e)
        {

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

                item = selectedrow;
                this.metroContextMenu1.Show(this.metroGrid1, e.Location);
                metroContextMenu1.Show(Cursor.Position);
            }
        }

        private void add_new_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            new Main(1).Show();
        }

        private void add_new_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            new Main(1).Show();
        }


    }
}
