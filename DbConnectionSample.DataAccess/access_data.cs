using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DbConnectionSample.Entities;
using DbConnectionSample.Framework;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using MetroFramework;

namespace DbConnectionSample.DataAccess
{
    public class access_data
    {
        public string a, b, c, dd = null;

        public void InsertNoficationInterval(int time)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("update dbo.user_details set regularintervaltogetupdate=@regularintervaltogetupdate where username='admin';");

            SqlParameter p = new SqlParameter("@regularintervaltogetupdate",SqlDbType.Int);
            p.Value = time;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public int GetNoficationInterval()
        {
            int x;
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("select dbo.user_details.regularintervaltogetupdate from dbo.user_details where dbo.user_details.username='admin';");

            try
            {
                cmd.Connection.Open();
                SqlDataReader dx = cmd.ExecuteReader();
                dx.Read();

                x = dx.GetInt32(0);

                dx.Close();
                cmd.Connection.Close();
            }
            catch(System.Data.SqlTypes.SqlNullValueException)
            {
                x = 0;
            }
            return x;
        }

        public void InsertNewToCC(string email)
        {
            if (CheckDtataExistOrNot(email).Rows.Count <= 0 )
            {
                SqlDbDataAccess dr = new SqlDbDataAccess();
                SqlCommand cmd = dr.GetCommand("insert into dbo.email_cc (username,e_to) values(@username,@e_to);");

                SqlParameter p = new SqlParameter("@username", SqlDbType.VarChar, 5);
                p.Value = "admin";

                SqlParameter p1 = new SqlParameter("@e_to", SqlDbType.VarChar, 50);
                p1.Value = email;

                cmd.Parameters.Add(p);
                cmd.Parameters.Add(p1);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            else
            {
                SqlDbDataAccess dr = new SqlDbDataAccess();
                SqlCommand cmd = dr.GetCommand("update dbo.email_cc set e_to=@e_to1 where dbo.email_cc.e_to=@e_to2;");

                SqlParameter p = new SqlParameter("@e_to1", SqlDbType.VarChar, 50);
                p.Value = email;

                SqlParameter p1 = new SqlParameter("@e_to2", SqlDbType.VarChar, 50);
                p1.Value = email;

                cmd.Parameters.Add(p);
                cmd.Parameters.Add(p1);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }

        public void GetEmailConfigInfo(user_details u)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.user_details.e_from,dbo.user_details.e_password,dbo.user_details.port,dbo.user_details.enablessi,dbo.user_details.usedefaultcredentials from dbo.user_details where dbo.user_details.username='admin';");

            try
            {
                cmd.Connection.Open();
                SqlDataReader dx = cmd.ExecuteReader();
                dx.Read();

                u.e_from = dx.GetString(0);
                u.e_password = dx.GetString(1);
                u.port = dx.GetInt32(2);
                u.enablessi = dx.GetString(3);
                u.usedefaultcredentials = dx.GetString(4);

                dx.Close();
                cmd.Connection.Close();
            }
            catch (System.Data.SqlTypes.SqlNullValueException)
            {
                u.e_from = "";
                u.e_password = "";
                u.port = 0;
                u.enablessi = "";
                u.usedefaultcredentials = "";
            }
        }

        public bool InsertEmailConfiguration(user_details u)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("update dbo.user_details set e_from=@e_from,e_password=@e_password,port=@port,enablessi=@enablessi,usedefaultcredentials=@usedefaultcredentials where dbo.user_details.username='admin';");


            SqlParameter p = new SqlParameter("@e_from", SqlDbType.VarChar, 50);
            p.Value = u.e_from;

            SqlParameter p1 = new SqlParameter("@e_password",SqlDbType.VarChar,20);
            p1.Value = u.e_password;

            SqlParameter p2 = new SqlParameter("@port",SqlDbType.Int);
            p2.Value = u.port;

            SqlParameter p3 = new SqlParameter("@enablessi",SqlDbType.VarChar,4);
            p3.Value = u.enablessi;

            SqlParameter p4 = new SqlParameter("@usedefaultcredentials",SqlDbType.VarChar,6);
            p4.Value = u.usedefaultcredentials;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            return true;
        }
        public string GetAdminPassword()
        {
            string i = "";
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.user_details.password from dbo.user_details where dbo.user_details.username='admin';");

            try
            {
                cmd.Connection.Open();
                SqlDataReader d = cmd.ExecuteReader();
                d.Read();

                i = d.GetString(0);

                d.Close();
                cmd.Connection.Close();
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("Please set your proper database connection link.Go TO Tools->Connection option.","WSH notift",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            catch (System.Data.SqlTypes.SqlNullValueException)
            {
                return "empty";
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return "empty";
            }
            return i;
        }

        public bool changepassord(string password)
        {
            try
            {
                SqlDbDataAccess dr = new SqlDbDataAccess();
                SqlCommand cmd = dr.GetCommand("update dbo.user_details set password=@password where dbo.user_details.username='admin';");

                SqlParameter p = new SqlParameter("@password", SqlDbType.VarChar, 100);
                p.Value = password;


                cmd.Parameters.Add(p);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return true;
            }
            catch(System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Please set your proper database connection link.Go TO Tools->Connection option.", "WSH notift", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }


        public string CheckUpdateNotificationThroughEmail()
        {
            string i = "";
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.user_details.notifythroughemail from dbo.user_details where dbo.user_details.username='admin';");

            try
            {
                cmd.Connection.Open();
                SqlDataReader d = cmd.ExecuteReader();
                d.Read();

                i = d.GetString(0);

                d.Close();
                cmd.Connection.Close();
            }
            catch (System.Data.SqlTypes.SqlNullValueException)
            {
                return "no";
            }
            return i;
        }

        public void UpdateNotificationThroughEmail(string value)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("update dbo.user_details set notifythroughemail=@notifythroughemail where dbo.user_details.username='admin';");

            SqlParameter p = new SqlParameter("@notifythroughemail", SqlDbType.VarChar, 4);
            p.Value = value;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public string SendMail(string body)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            string y = "";
            access_data d = new access_data();
            user_details u = new user_details();
            d.GetEmailConfigInfo(u);

            using (SqlCommand cmd = dr.GetCommand("select dbo.email_cc.e_to from dbo.email_cc;"))
            {
                cmd.Connection.Open();
                using(var reader=cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        string to = reader["e_to"].ToString();

                        MailMessage mail = new MailMessage(u.e_from, to, "report from WHN", body);
                        SmtpClient clint = new SmtpClient();
                        //for determine email smtp...
                        string x = u.e_from;
                        int startIndex = x.IndexOf('@');
                        int endIndex = x.LastIndexOf('.');
                        int length = endIndex - startIndex;
                        string xx = x.Substring(startIndex + 1, length - 1);

                        if (xx == "gmail" || xx == "Gmail")
                        {
                            clint.Host = "smtp.gmail.com";
                            clint.Port = u.port;
                            clint.EnableSsl = Convert.ToBoolean(u.enablessi);
                        }
                        if (xx == "Hotmail" || xx == "hotmail" || xx == "live" || xx == "Live")
                        {
                            clint.Host = "smtp.live.com";
                            clint.Port = u.port;
                            clint.EnableSsl = Convert.ToBoolean(u.enablessi);
                        }
                        if (xx == "yahoo" || xx == "Yahoo")
                        {
                            clint.Host = "smtp.mail.yahoo.com";
                            clint.Port = u.port;
                            clint.EnableSsl = Convert.ToBoolean(u.enablessi);
                        }
                        clint.Credentials = new System.Net.NetworkCredential(u.e_from, u.e_password);
                        clint.DeliveryMethod = SmtpDeliveryMethod.Network;
                        clint.UseDefaultCredentials = Convert.ToBoolean(u.usedefaultcredentials);
                        try
                        {
                            clint.Send(mail);
                        }
                        catch (System.Net.Mail.SmtpException)
                        {
                            y = "Your internet connection is not secure to send email";
                            return y;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public string getemail()
        {
            string email = "";
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.user_details.e_from from dbo.user_details where dbo.user_details.username='admin';");

            try
            {
                cmd.Connection.Open();
                SqlDataReader d = cmd.ExecuteReader();
                d.Read();

                email = d.GetString(0);

                d.Close();
                cmd.Connection.Close();
            }
            catch (System.Data.SqlTypes.SqlNullValueException)
            {
                return "empty";
            }

            return email;
        }

        public DataTable CheckEveryTagValue(string url)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.site_details.tag_name as 'Tag Name',dbo.site_details.tag_value as 'Tag Value' from dbo.site_details where dbo.site_details.site_id=(select dbo.site.id from dbo.site where dbo.site.url=@url);");

            SqlParameter p = new SqlParameter("@url",SqlDbType.VarChar,50);
            p.Value = url;

            cmd.Parameters.Add(p);

            DataTable tbl = new DataTable();

            using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
            {
                cmd.Connection.Open();
                dt.Fill(tbl);
                cmd.Connection.Close();
            }
            return tbl;

        }

        public DataTable ReturnWebSiteUrl()
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.site.url as URL from dbo.site;");

            DataTable tbl = new DataTable();

            using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
            {
                cmd.Connection.Open();
                dt.Fill(tbl);
                cmd.Connection.Close();
            }
            return tbl;
        }

        public DataTable CheckDtataExistOrNot(string email)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.email_cc.e_to from dbo.email_cc where dbo.email_cc.e_to=@e_to;");

            SqlParameter p = new SqlParameter("@e_to",SqlDbType.VarChar,50);
            p.Value = email;

            cmd.Parameters.Add(p);

            DataTable tbl = new DataTable();

            using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
            {
                cmd.Connection.Open();
                dt.Fill(tbl);
                cmd.Connection.Close();
            }
            return tbl;
        }

        public DataTable ReturnEmailToCc()
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.email_cc.e_to As 'To' from dbo.email_cc where dbo.email_cc.username='admin';");

            DataTable tbl = new DataTable();

            using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
            {
                cmd.Connection.Open();
                dt.Fill(tbl);
                cmd.Connection.Close();
            }
            return tbl;
        }

        public bool DeleteUrl(string url)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("begin transaction delete from dbo.site_details where dbo.site_details.site_id=(select dbo.site.id from dbo.site where dbo.site.url=@url); delete from dbo.site where dbo.site.url=@url1;commit;");

            SqlParameter p = new SqlParameter("@url",SqlDbType.VarChar,50);
            p.Value = url;

            SqlParameter p1 = new SqlParameter("@url1",SqlDbType.VarChar,50);
            p1.Value = url;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            return true;
        }

        public bool DeleteTo(string email)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("delete from dbo.email_cc where dbo.email_cc.e_to=@e_to");

            SqlParameter p = new SqlParameter("@e_to", SqlDbType.VarChar, 50);
            p.Value = email;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            return true;
        }

        public bool InsertNewUrlMutipleTagNameAndValue()
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();

            return true;
        }

        public DataTable ReturnWebTagListAndName(int id)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.site_details.tag_name as 'Tag Name',dbo.site_details.tag_value as 'Tag Value' from dbo.site_details where dbo.site_details.site_id=@id");

            SqlParameter p = new SqlParameter("@id", SqlDbType.Int);
            p.Value = id;

            cmd.Parameters.Add(p);

            DataTable tbl = new DataTable();

            using (SqlDataAdapter dt = new SqlDataAdapter(cmd))
            {
                cmd.Connection.Open();
                dt.Fill(tbl);
                cmd.Connection.Close();
            }
            return tbl;
        }

        public string GetUrlByUsingId(int id)
        {
            string i = string.Empty;
            try
            {
                SqlDbDataAccess da = new SqlDbDataAccess();
                SqlCommand cmd = da.GetCommand("select dbo.site.url from dbo.site where dbo.site.id=@id;");

                SqlParameter p = new SqlParameter("@id", SqlDbType.Int);
                p.Value = id;

                cmd.Parameters.Add(p);

                cmd.Connection.Open();

                SqlDataReader d = cmd.ExecuteReader();

                d.Read();

                i = d.GetString(0);

            }
            catch (System.InvalidOperationException)
            {
                i = "empty";
            }
            return i;
        }

        public int GetIdByUsingUrl(string url)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("select dbo.site.id from dbo.site where dbo.site.url=@url;");

            SqlParameter p = new SqlParameter("@url", SqlDbType.VarChar, 50);
            p.Value = url;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();

            SqlDataReader d = cmd.ExecuteReader();

            d.Read();

            int i = d.GetInt32(0);

            return i;
        }

        public int GetSite_detailsIdByUsingTag_name(string tag_name)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();
            SqlCommand cmd = da.GetCommand("select dbo.site_details.id from dbo.site_details where dbo.site_details.tag_name=@tag_name;");

            SqlParameter p = new SqlParameter("@tag_name", SqlDbType.VarChar, 50);
            p.Value = tag_name;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();

            SqlDataReader d = cmd.ExecuteReader();

            d.Read();

            int i = d.GetInt32(0);

            return i;
        }



        public void get_info_by_url(string url)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.site.url,dbo.site_details.tag_name,dbo.site_details.tag_value,dbo.site.description from dbo.site,dbo.site_details where dbo.site.id=dbo.site_details.site_id and dbo.site.url=@url;");

            SqlParameter p = new SqlParameter("@url", SqlDbType.VarChar, 50);
            p.Value = url;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();

            SqlDataReader d = cmd.ExecuteReader();

            d.Read();

            try
            {
                a = d.GetString(0);
                b = d.GetString(1);
                c = d.GetString(2);
                dd = d.GetString(3);
            }
            catch (System.Data.SqlTypes.SqlNullValueException)
            {
                a = "";
                b = "";
                c = "";
                dd = "";
            }

            cmd.Connection.Close();

        }
        public bool DeleteRow(int id)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("delete from dbo.site_details where dbo.site_details.id=@id;");

            SqlParameter p = new SqlParameter("@id", SqlDbType.Int);
            p.Value = id;

            cmd.Parameters.Add(p);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            return true;
        }
        public bool CheckDataAvailableOrNot(string tag_name,string tag_value)
        {
            int value;
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("select dbo.site_details.id from dbo.site_details where dbo.site_details.tag_name=@tag_name and dbo.site_details.tag_value=@tag_value;");

            SqlParameter  p=new SqlParameter("@tag_name",SqlDbType.VarChar,200);
            p.Value = tag_name;

            SqlParameter p1 = new SqlParameter("@tag_value",SqlDbType.VarChar,500);
            p1.Value = tag_value;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);

            cmd.Connection.Open();

            SqlDataReader d = cmd.ExecuteReader();

            d.Read();

            try
            {
                value=d.GetInt32(0);
            }
            catch (System.Data.SqlTypes.SqlNullValueException)
            {
                value = 0;
            }
            catch(System.InvalidOperationException)
            {
                value = 0;
            }
            if(value !=0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool Update_site_details(int id,string tag_name,string tag_value)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("update dbo.site_details set tag_name=@tag_name,tag_value=@tag_value where dbo.site_details.id=@id;");

            SqlParameter p = new SqlParameter("@tag_name", SqlDbType.VarChar, 200);
            p.Value = tag_name;

            SqlParameter p1 = new SqlParameter("@tag_value", SqlDbType.VarChar, 500);
            p1.Value = tag_value;

            SqlParameter p2 = new SqlParameter("@id", SqlDbType.Int);
            p2.Value = id;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            return true;
        }

        public bool InsertNew_site_details(int site_id,string tag_name, string tag_value)
        {
            SqlDbDataAccess dr = new SqlDbDataAccess();
            SqlCommand cmd = dr.GetCommand("insert into dbo.site_details (site_id,tag_name,tag_value) values(@site_id,@tag_name,@tag_value);");

            SqlParameter p = new SqlParameter("@site_id",SqlDbType.Int);
            p.Value = site_id;

            SqlParameter p1 = new SqlParameter("@tag_name",SqlDbType.VarChar,200);
            p1.Value = tag_name;

            SqlParameter p2 = new SqlParameter("@tag_value",SqlDbType.VarChar,500);
            p2.Value = tag_value;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            return true;
        }

        public bool Edit_web_info(site e,int id)
        {
            SqlDbDataAccess da = new SqlDbDataAccess();

            SqlCommand cmd = da.GetCommand("update dbo.site set url=@url,description=@description where dbo.site.id=@id;");

            SqlParameter p = new SqlParameter("@url",SqlDbType.VarChar,50);
            p.Value = e.url;

            SqlParameter p1 = new SqlParameter("@description",SqlDbType.VarChar,500);
            p1.Value = e.description;

            SqlParameter p2 = new SqlParameter("@id",SqlDbType.Int);
            p2.Value = id;

            cmd.Parameters.Add(p);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);;

            cmd.Connection.Open();

            cmd.ExecuteNonQuery();

            cmd.Connection.Close();

            return true;
        }

        public int InsertNweUrlData(site i)
        {
            int xy;
            SqlDbDataAccess dr = new SqlDbDataAccess();
            using (SqlCommand cmd = dr.GetCommand("insert into dbo.site (url,description) values (@url,@description);"))
            {
                SqlParameter p = new SqlParameter("@url",SqlDbType.VarChar,50);
                p.Value = i.url;

                SqlParameter p1 = new SqlParameter("@description",SqlDbType.VarChar,500);
                p1.Value = i.description;

                cmd.Parameters.Add(p);
                cmd.Parameters.Add(p1);

                cmd.Connection.Open();

                cmd.ExecuteNonQuery();

                cmd.Connection.Close();
            }
            using (SqlCommand cmd = dr.GetCommand("select dbo.site.id from dbo.site where dbo.site.url=@url;"))
            {
                SqlParameter p2 = new SqlParameter("@url", SqlDbType.VarChar, 50);
                p2.Value = i.url;

                cmd.Parameters.Add(p2);

                cmd.Connection.Open();
                SqlDataReader d = cmd.ExecuteReader();
                d.Read();
                xy = d.GetInt32(0);
                cmd.Connection.Close();
                d.Close();
            }

            return xy;
        }
    }
}
