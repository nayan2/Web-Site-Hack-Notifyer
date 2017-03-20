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
using System.IO;

namespace web_site_hack_notifyer_sp1
{
    public partial class ConnectionLink : MetroFramework.Forms.MetroForm
    {
        public ConnectionLink()
        {
            InitializeComponent();
        }

        private void ConnectionLink_Load(object sender, EventArgs e)
        {
            try
            {
                using (StreamReader st = new StreamReader("connectionstring.txt"))
                {
                    string x = st.ReadToEnd();
                    metroTextBox1.Text = x;
                    st.Close();
                }
            }
            catch(Exception)
            {
                MetroMessageBox.Show(this, "File not found", "Wsh notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroButton10_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText("connectionstring.txt", String.Empty);
                using (StreamWriter r = new StreamWriter("connectionstring.txt"))
                {
                    r.WriteLine(metroTextBox1.Text);
                    r.Close();
                    MetroMessageBox.Show(this, "Connection link successfully changed", "Wsh notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
            catch(Exception)
            {
                MetroMessageBox.Show(this, "File not found", "Wsh notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
