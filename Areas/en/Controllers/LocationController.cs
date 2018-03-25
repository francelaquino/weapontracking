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
    public class LocationController : Controller
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
                strSQL = @"SELECT ID,GID,LOCATION,DESCRIPTION FROM LOCATION ORDER BY LOCATION ASC";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlLocation> record = new List<mdlLocation>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlLocation list = new mdlLocation();
                        list.ID = dr["ID"].ToString();
                        list.GID = dr["GID"].ToString();
                        list.DESCRIPTION = dr["DESCRIPTION"].ToString();
                        list.LOCATION = dr["LOCATION"].ToString();
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



                string strSQL = @"SELECT LOCATION,DESCRIPTION FROM LOCATION WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlLocation record = new mdlLocation();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.ID = id;
                    record.GID = gid;
                    record.LOCATION = dr["LOCATION"].ToString();
                    record.DESCRIPTION = dr["DESCRIPTION"].ToString();

                }


                return PartialView(record);

            }
            finally
            {
                dbconn.closeConnection();
            }

        }


        [HttpPost]
        public ActionResult savelocation()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtlocation = Request.Form["txtlocation"].ToString();
            string txtdescription = Request.Form["txtdescription"].ToString();
            string gid = "";
            try
            {

                gid = util.GenerateId();

                strSQL = @"INSERT INTO LOCATION(GID,LOCATION,DATEENCODED,ENCODEDBY,DATEMODIFIED,MODIFIEDBY,DESCRIPTION)
                    VALUES(@GID,@LOCATION,GETDATE(),@ENCODEDBY,GETDATE(),@MODIFIEDBY,@DESCRIPTION)";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = txtlocation;
                cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("DESCRIPTION", SqlDbType.VarChar).Value = txtdescription;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbconn.closeConnection();
                Session["message"] = "Location successfully saved...";

                return RedirectToAction("add");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }


        [HttpPost]
        public ActionResult updatelocation()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtid = Request.Form["txtid"].ToString();
            string txtgid = Request.Form["txtgid"].ToString();
            string txtlocation = Request.Form["txtlocation"].ToString();
            string txtdescription = Request.Form["txtdescription"].ToString();
            try
            {


                strSQL = @"UPDATE LOCATION SET LOCATION=@LOCATION,DATEMODIFIED=GETDATE(),MODIFIEDBY=@MODIFIEDBY ,DESCRIPTION=@DESCRIPTION
                WHERE ID=@ID AND GID=@GID";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = txtlocation;
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("DESCRIPTION", SqlDbType.VarChar).Value = txtdescription;
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
                strSQL = @"SELECT ID FROM LOCATION WHERE  UPPER(LOCATION)=UPPER(@LOCATION)";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = gid;
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

                    strSQL = @"DELETE FROM LOCATION WHERE ID=@ID";
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
