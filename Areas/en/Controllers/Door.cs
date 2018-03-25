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
    public class DoorController : Controller
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
                strSQL = @"SELECT ID,GID,DOOR,LOCATION,DESCRIPTION
                         FROM VDOOR ORDER BY DOOR ASC";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlDoor> record = new List<mdlDoor>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlDoor list = new mdlDoor();
                        list.ID = dr["ID"].ToString();
                        list.GID = dr["GID"].ToString();
                        list.DOOR = dr["DOOR"].ToString();
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
            List<SelectListItem> location_combo = new List<SelectListItem>();
            location_combo = DropDown.getLocation();
            ViewData["location"] = location_combo;

            return View();
        }

        [HttpPost]
        public ActionResult savedoor()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtdoor = Request.Form["txtdoor"].ToString();
            string txtdescription = Request.Form["txtdescription"].ToString();
            string txtlocation = Request.Form["txtlocation"].ToString();
            string gid = "";
            try
            {


                strSQL = @"INSERT INTO DOOR(GID,DOOR,DESCRIPTION,LOCATION,ENCODEDBY,DATEENCODED,MODIFIEDBY,DATEMODIFIED)
                            VALUES(CONVERT(varchar(10), right(newid(),10)),@DOOR,@DESCRIPTION,@LOCATION,@ENCODEDBY,GETDATE(),@MODIFIEDBY,GETDATE())";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("DOOR", SqlDbType.VarChar).Value = txtdoor;
                cmd.Parameters.Add("DESCRIPTION", SqlDbType.VarChar).Value = txtdescription;
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = txtlocation;
                cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbconn.closeConnection();
                Session["message"] = "Door successfully saved...";

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
            location_combo = DropDown.getLocation();

            Db_Connection dbconn = new Db_Connection();

            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT DOOR,LOCATION,DESCRIPTION FROM VDOOR WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlDoor record = new mdlDoor();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.ID = id;
                    record.GID = gid;
                    record.DOOR = dr["DOOR"].ToString();
                    record.LOCATION = dr["LOCATION"].ToString();
                    record.DESCRIPTION = dr["DESCRIPTION"].ToString();

                }

                foreach (SelectListItem item in location_combo)
                {
                    if (item.Text.ToUpper() == record.LOCATION.ToUpper())
                    {
                        item.Selected = true;
                    }
                }


                ViewData["location"] = location_combo;


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

                    strSQL = @"DELETE FROM DOOR WHERE ID=@ID";
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
        public ActionResult updatedoor()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtid = Request.Form["txtid"].ToString();
            string txtgid = Request.Form["txtgid"].ToString();
            string txtdoor = Request.Form["txtdoor"].ToString();
            string txtdescription = Request.Form["txtdescription"].ToString();
            string txtlocation = Request.Form["txtlocation"].ToString();
            try
            {


                strSQL = @"UPDATE DOOR SET DOOR=@DOOR,DESCRIPTION=@DESCRIPTION,
                    LOCATION=@LOCATION,MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE()
                            WHERE ID=@ID AND GID=@GID";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);


                cmd.Parameters.Add("DOOR", SqlDbType.VarChar).Value = txtdoor;
                cmd.Parameters.Add("DESCRIPTION", SqlDbType.VarChar).Value = txtdescription;
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