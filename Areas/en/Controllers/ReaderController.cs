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
    public class ReaderController : Controller
    {
        //
        // GET: /en/Reader/
        public ActionResult Index()
        {
            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT ID,GID,READERMODEL,READERNAME,PORT,IP,TIME,LOCATION
                         FROM VREADER ORDER BY READERNAME ASC";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlReaders> record = new List<mdlReaders>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlReaders list = new mdlReaders();
                        list.ID = dr["ID"].ToString();
                        list.GID = dr["GID"].ToString();
                        list.READERNAME = dr["READERNAME"].ToString();
                        list.READERMODEL = dr["READERMODEL"].ToString();
                        list.LOCATION = dr["LOCATION"].ToString();
                        list.PORT = dr["PORT"].ToString();
                        list.IP = dr["IP"].ToString();
                        list.TIME = dr["TIME"].ToString();
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

        public ActionResult add()
        {
            if (Session["message"] == null) Session["message"] = "";
            Populate DropDown = new Populate();
            List<SelectListItem> location_combo = new List<SelectListItem>();
            List<SelectListItem> interval_combo = new List<SelectListItem>();
            List<SelectListItem> rfidmodel_combo = new List<SelectListItem>();
            interval_combo = DropDown.getTimeInterval();
            rfidmodel_combo = DropDown.getReaderModel();
            location_combo = DropDown.getLocation();
            ViewData["location"] = location_combo;
            ViewData["interval"] = interval_combo;
            ViewData["rfidmodel"] = rfidmodel_combo;

            return View();
        }

        [HttpPost]
        public ActionResult savereader()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtreader = Request.Form["txtreader"].ToString();
            string txtport = Request.Form["txtport"].ToString();
            string txtip = Request.Form["txtip"].ToString();
            string txtinterval = Request.Form["txtinterval"].ToString();
            string txtmodel = Request.Form["txtmodel"].ToString();
            string txtlocation = Request.Form["txtlocation"].ToString();
            string gid = "";
            try
            {

                gid = util.GenerateId();

                strSQL = @"INSERT INTO READER(GID,READERMODEL,READERNAME,PORT,IP,TIME,LOCATION,ENCODEDBY,DATEENCODED,MODIFIEDBY,DATEMODIFIED)
                            VALUES(CONVERT(varchar(10), right(newid(),10)),@READERMODEL,@READERNAME,@PORT,@IP,@TIME,@LOCATION,@ENCODEDBY,GETDATE(),@MODIFIEDBY,GETDATE())";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("READERMODEL", SqlDbType.VarChar).Value = txtmodel;
                cmd.Parameters.Add("READERNAME", SqlDbType.VarChar).Value = txtreader;
                cmd.Parameters.Add("PORT", SqlDbType.VarChar).Value = txtport;
                cmd.Parameters.Add("IP", SqlDbType.VarChar).Value = txtip;
                cmd.Parameters.Add("TIME", SqlDbType.VarChar).Value = txtinterval;
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = txtlocation;
                cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbconn.closeConnection();
                Session["message"] = "Reader successfully saved...";

                return RedirectToAction("add");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }
        public ActionResult edit(string id, string gid)
        {
            Populate DropDown = new Populate();
            List<SelectListItem> location_combo = new List<SelectListItem>();
            List<SelectListItem> interval_combo = new List<SelectListItem>();
            List<SelectListItem> rfidmodel_combo = new List<SelectListItem>();
            interval_combo = DropDown.getTimeInterval();
            rfidmodel_combo = DropDown.getReaderModel();
            location_combo = DropDown.getLocation();

            Db_Connection dbconn = new Db_Connection();

            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT READERMODEL,READERNAME,PORT,IP,TIME,LOCATION FROM VREADER WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlReaders record = new mdlReaders();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.ID = id;
                    record.GID = gid;
                    record.READERNAME = dr["READERNAME"].ToString();
                    record.LOCATION = dr["LOCATION"].ToString();
                    record.PORT = dr["PORT"].ToString();
                    record.IP = dr["IP"].ToString();
                    record.READERMODEL = dr["READERMODEL"].ToString();
                    record.TIME = dr["TIME"].ToString();

                }

                foreach (SelectListItem item in location_combo)
                {
                    if (item.Text.ToUpper() == record.LOCATION.ToUpper())
                    {
                        item.Selected = true;
                    }
                }
                foreach (SelectListItem item in interval_combo)
                {
                    if (item.Value == record.TIME)
                    {
                        item.Selected = true;
                    }
                }
                foreach (SelectListItem item in rfidmodel_combo)
                {
                    if (item.Text.ToUpper() == record.READERMODEL.ToUpper())
                    {
                        item.Selected = true;
                    }
                }

                ViewData["location"] = location_combo;
                ViewData["interval"] = interval_combo;
                ViewData["rfidmodel"] = rfidmodel_combo;


                return PartialView(record);

            }
            finally
            {
                dbconn.closeConnection();
            }

        }

        public string isDelete(string id)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";

                //strSQL = @"SELECT ID FROM ASSETS WHERE  LOCATION=@LOCATION";
                //SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                //cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = id;
                //SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows)
                //{
                //    strResult = "Found";
                //}
                //dr.Dispose();
                //cmd.Dispose();




                //if (strResult == "")
                //{
                //    strSQL = @"SELECT ID FROM LINENS WHERE  LOCATION=@LOCATION";
                //    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                //    cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = id;
                //    dr = cmd.ExecuteReader();
                //    if (dr.HasRows)
                //    {
                //        strResult = "Found";
                //    }
                //    dr.Dispose();
                //    cmd.Dispose();

                //}





                if (strResult == "")
                {

                    strSQL = @"DELETE FROM READER WHERE ID=@ID";
                    SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                    cmd.ExecuteNonQuery();
                }

                dbconn.closeConnection();

                return strResult;
            }
            finally
            {
                dbconn.closeConnection();
            }

        }



        [HttpPost]
        public ActionResult updatereader()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtid = Request.Form["txtid"].ToString();
            string txtgid = Request.Form["txtgid"].ToString();
            string txtreader = Request.Form["txtreader"].ToString();
            string txtport = Request.Form["txtport"].ToString();
            string txtip = Request.Form["txtip"].ToString();
            string txtinterval = Request.Form["txtinterval"].ToString();
            string txtmodel = Request.Form["txtmodel"].ToString();
            string txtlocation = Request.Form["txtlocation"].ToString();
            try
            {


                strSQL = @"UPDATE READER SET READERNAME=@READERNAME,PORT=@PORT,IP=@IP,TIME=@TIME,READERMODEL=@READERMODEL,
                    LOCATION=@LOCATION,MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE()
                            WHERE ID=@ID AND GID=@GID";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);


                cmd.Parameters.Add("READERNAME", SqlDbType.VarChar).Value = txtreader;
                cmd.Parameters.Add("PORT", SqlDbType.VarChar).Value = txtport;
                cmd.Parameters.Add("IP", SqlDbType.VarChar).Value = txtip;
                cmd.Parameters.Add("TIME", SqlDbType.VarChar).Value = txtinterval;
                cmd.Parameters.Add("READERMODEL", SqlDbType.VarChar).Value = txtmodel;
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = txtlocation;
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = txtid;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = txtgid;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbconn.closeConnection();

                return RedirectToAction("index");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }

	}
}