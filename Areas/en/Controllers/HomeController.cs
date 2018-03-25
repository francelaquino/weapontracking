using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PopulateDropDown;
using Db_Connections;
using System.Data.SqlClient;
using System.Data;
using Utilities;
using FMS.Models;

namespace FMS.Areas.en.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /en/Home/
        public ActionResult Index()
        {
            string txtdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["date"] = txtdate;
            return View();
        }

        public ActionResult weaponCount(string date)
        {
            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                string checkoutToday = "0", checkinToday="0",actualCheckout="0";
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT COUNT(ID) FROM FIREARMS_INOUT WHERE CONVERT(VARCHAR(10),[DATE],110)=@DATE AND UPPER(STATUS)='CHECKOUT'";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("DATE", SqlDbType.VarChar).Value = Convert.ToDateTime(date).ToString("MM-dd-yyyy");
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                checkoutToday = dr[0].ToString();
                dr.Dispose();
                cmd.Dispose();

                strSQL = @"SELECT COUNT(ID) FROM FIREARMS_INOUT WHERE CONVERT(VARCHAR(10),[DATE],110)=@DATE AND UPPER(STATUS)='CHECKIN'";


                 cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("DATE", SqlDbType.VarChar).Value = Convert.ToDateTime(date).ToString("MM-dd-yyyy");
                 dr = cmd.ExecuteReader();
                dr.Read();
                checkinToday = dr[0].ToString();
                dr.Dispose();
                cmd.Dispose();

                strSQL = @"SELECT COUNT(ID) FROM ASSIGN_FIREARMS WHERE UPPER(STATUS)='CHECKOUT'";


                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                dr = cmd.ExecuteReader();
                dr.Read();
                actualCheckout = dr[0].ToString();
                dr.Dispose();
                cmd.Dispose();



                dbconn.closeConnection();

                ViewData["checkoutToday"] = checkoutToday;
                ViewData["checkinToday"] = checkinToday;
                ViewData["actualCheckout"] = actualCheckout;
                return PartialView();
            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public ActionResult countLocation()
        {
            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT LOCATION,CNT FROM vrfid_locationcheckout";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlTrack> record = new List<mdlTrack>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlTrack list = new mdlTrack();
                        list.LOCATION = dr["LOCATION"].ToString();
                        list.COUNT = dr["CNT"].ToString();
                        record.Add(list);
                    }
                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();
                return View(record);
            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public ActionResult countAssignee()
        {
            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT BATCHNO,NAME,COUNT(BATCHNO) AS CNT FROM VASSIGN_FIREARMS WHERE UPPER(STATUS)='CHECKIN' GROUP BY BATCHNO,NAME";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlAssignee> record = new List<mdlAssignee>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlAssignee list = new mdlAssignee();
                        list.NAME = dr["BATCHNO"].ToString() + " - " + dr["NAME"].ToString();
                        list.CHECKOUTS = dr["CNT"].ToString();
                        record.Add(list);
                    }
                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();
                return View(record);
            }
            finally
            {
                dbconn.closeConnection();
            }
        }
	}
}