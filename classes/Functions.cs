using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Db_Connections;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace Functions
{
    public class Function
    {
        public void isLogin(string username)
        {
            if (string.IsNullOrEmpty(username))
            {


                HttpContext.Current.Response.Redirect(VirtualPathUtility.ToAbsolute("~/en/login"));

            }
           

        }


        public string getCurrentId(string table)
        {

            string strSQL = "SELECT IDENT_CURRENT('" + table + "')";
            string Id = "1";
            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Id = dr[0].ToString();
                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return Id;

            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public  void sendMail(string email, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            try
            {
                if (email != "")
                {
                    string from = System.Web.HttpContext.Current.Session["email_from"].ToString();
                    string password = System.Web.HttpContext.Current.Session["email_password"].ToString();
                    string smtp = System.Web.HttpContext.Current.Session["email_smtp"].ToString();
                    string port = System.Web.HttpContext.Current.Session["email_port"].ToString();
                    string ssl = System.Web.HttpContext.Current.Session["email_ssl"].ToString();

                    mail.From = new MailAddress(from);
                    mail.To.Add(email);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    //msg.Priority = MailPriority.High;

/*
                    using (SmtpClient client = new SmtpClient())
                    {
                        client.EnableSsl = Convert.ToBoolean(ssl);
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(from, password);
                        client.Host = smtp;
                        client.Port = Convert.ToInt16(port);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        
                        client.Send(mail);

                    }*/

                    body = body + "<p></br></p>";
                    body = body + "<p><b>This is an automated email please do not reply</b></p>";
                    body = body + "<p><i>Trackline Automated Email</i></p>";

                    var client = new SmtpClient
                    {
                        EnableSsl = Convert.ToBoolean(ssl),
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(from, password),
                        Host = smtp,
                        Port = Convert.ToInt16(port),
                        DeliveryMethod = SmtpDeliveryMethod.Network
                        
                    };

                    Thread T1 = new Thread(delegate()
                    {
                        using (var message = new MailMessage(from, email)
                        {
                            Subject = subject,
                            Body = body,
                            IsBodyHtml=true
                        })
                        {
                            {
                                client.Send(message);
                            }
                        }
                    });

                    T1.Start();
                }
            }

            catch(Exception ex)
            {
            }
            finally{
                mail.Dispose();
            }

        }
       
        public void authorized(string page,string username)
        {

            string strSQL = "SELECT "+ page +" FROM SYSUSER_ACCESS WHERE USERNAME=@USERNAME";
            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("USERNAME", SqlDbType.VarChar).Value = username;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr[0].ToString().ToUpper() != "TRUE")
                    {
                        dr.Dispose();
                        cmd.Dispose();
                        dbconn.closeConnection();
                        HttpContext.Current.Response.Redirect(VirtualPathUtility.ToAbsolute("~/en/home/oops"));
                    }
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();
                    dbconn.closeConnection();
                    HttpContext.Current.Response.Redirect(VirtualPathUtility.ToAbsolute("~/en/home/oops"));
                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();


            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public string checkAccess(string page, string username)
        {

            string strSQL = "SELECT " + page + " FROM SYSUSER_ACCESS WHERE USERNAME=@USERNAME";
            Db_Connection dbconn = new Db_Connection();
            try
            {
                string access = "";
                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("USERNAME", SqlDbType.VarChar).Value = username;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr[0].ToString().ToUpper() == "TRUE")
                    {
                        access = "true";
                    }
                    else
                    {
                        access = "false";
                    }
                }
                else
                {
                    access = "false";
                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return access;
            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public void checkAdmin(string username)
        {

            string strSQL = "SELECT ISADMIN FROM SYSUSER WHERE USERNAME=@USERNAME";
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("USERNAME", SqlDbType.VarChar).Value = username;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr[0].ToString().ToLower() == "false")
                    {
                        dr.Dispose();
                        cmd.Dispose();
                        dbconn.closeConnection();
                        HttpContext.Current.Response.Redirect(VirtualPathUtility.ToAbsolute("~/en/home/oops"));
                    }
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();
                    dbconn.closeConnection();
                    HttpContext.Current.Response.Redirect(VirtualPathUtility.ToAbsolute("~/en/home/oops"));
                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

            }
            finally
            {
                dbconn.closeConnection();
            }
        }
        
    }
}