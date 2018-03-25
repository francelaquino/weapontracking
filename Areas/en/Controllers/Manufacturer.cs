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
    public class ManufacturerController : Controller
    {
        //
        // GET: /en/Firearmtype/
        public ActionResult Index()
        {
            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT ID,GID,MANUFACTURER FROM MANUFACTURER ORDER BY MANUFACTURER ASC";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlManufacturer> record = new List<mdlManufacturer>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlManufacturer list = new mdlManufacturer();
                        list.ID = dr["ID"].ToString();
                        list.GID = dr["GID"].ToString();
                        list.MANUFACTURER = dr["MANUFACTURER"].ToString();
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
            
            return View();
        }

        public ActionResult edit(string id, string gid)
        {
            Populate DropDown = new Populate();
            Db_Connection dbconn = new Db_Connection();

            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT MANUFACTURER FROM MANUFACTURER WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlManufacturer record = new mdlManufacturer();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.ID = id;
                    record.GID = gid;
                    record.MANUFACTURER = dr["MANUFACTURER"].ToString();

                }

            
                return PartialView(record);

            }
            finally
            {
                dbconn.closeConnection();
            }

        }


        [HttpPost]
        public ActionResult savemanufacturer()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtmanufacturer = Request.Form["txtmanufacturer"].ToString();
            string gid = "";
            try
            {

                gid = util.GenerateId();

                strSQL = @"INSERT INTO MANUFACTURER(GID,MANUFACTURER,DATEENCODED,ENCODEDBY,DATEMODIFIED,MODIFIEDBY)
                    VALUES(@GID,@MANUFACTURER,GETDATE(),@ENCODEDBY,GETDATE(),@MODIFIEDBY)";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                cmd.Parameters.Add("MANUFACTURER", SqlDbType.VarChar).Value = txtmanufacturer;
                cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbconn.closeConnection();
                Session["message"] = "Manufacturer type successfully saved...";

                return RedirectToAction("add");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }


        [HttpPost]
        public ActionResult updatemanufacturer()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtid = Request.Form["txtid"].ToString();
            string txtgid = Request.Form["txtgid"].ToString();
            string txtmanufacturer = Request.Form["txtmanufacturer"].ToString();
            try
            {


                strSQL = @"UPDATE MANUFACTURER SET MANUFACTURER=@MANUFACTURER,DATEMODIFIED=GETDATE(),MODIFIEDBY=@MODIFIEDBY 
                WHERE ID=@ID AND GID=@GID";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("MANUFACTURER", SqlDbType.VarChar).Value = txtmanufacturer;
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

        public string isExist(string id, string gid)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";
                strSQL = @"SELECT ID FROM MANUFACTURER WHERE  UPPER(MANUFACTURER)=UPPER(@MANUFACTURER)";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("MANUFACTURER", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != id && id != "")
                    {
                        strResult = "found";
                    }
                    else if (id == "")
                    {
                        strResult = "found";
                    }
                    cmd.Dispose();


                }
                dr.Dispose();
                cmd.Dispose();

                dbconn.closeConnection();

                return strResult;
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

                //strSQL = @"SELECT ID FROM ASSETS WHERE  MANUFACTURER=@MANUFACTURER";
                //SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                //cmd.Parameters.Add("MANUFACTURER", SqlDbType.VarChar).Value = id;
                //SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows)
                //{
                //    strResult = "Found";
                //}
                //dr.Dispose();
                //cmd.Dispose();




                //if (strResult == "")
                //{
                //    strSQL = @"SELECT ID FROM LINENS WHERE  MANUFACTURER=@MANUFACTURER";
                //    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                //    cmd.Parameters.Add("MANUFACTURER", SqlDbType.VarChar).Value = id;
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

                    strSQL = @"DELETE FROM MANUFACTURER WHERE ID=@ID";
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


	}
}
