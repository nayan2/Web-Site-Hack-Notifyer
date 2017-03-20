using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace DbConnectionSample.Framework
{
    public class SqlDbDataAccess
    {
       string ConnectiosnString;
       private void getconnection()
       {
           try
           {
               using (StreamReader sr = new StreamReader("connectionstring.txt"))
               {
                   string x = sr.ReadToEnd();
                   if(x == "")
                   {
                       MessageBox.Show("please set up the databse link first","Wsh notify",MessageBoxButtons.OK,MessageBoxIcon.Error);
                   }
                   else
                   {
                       ConnectiosnString = x;
                   }
                   sr.Close();
               }
           }
           catch(Exception)
           {
               MessageBox.Show("File not found", "Wsh notify", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
        }
        public SqlCommand GetCommand(String query)
        {
            getconnection();
            var connection = new SqlConnection(ConnectiosnString);
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = connection;
            return cmd;
        }
   }

}
