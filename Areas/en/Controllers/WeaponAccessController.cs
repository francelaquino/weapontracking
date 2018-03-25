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
using System.IO;
using Functions;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace FMS.Areas.en.Controllers
{
    public class WeaponAccessController : Controller
    {
        //
        // GET: /en/WeaponAccess/
        public ActionResult AccessDoor()
        {
            Populate DropDown = new Populate();
            List<SelectListItem> location_combo = new List<SelectListItem>();
            location_combo = DropDown.getLocation();
            ViewData["location"] = location_combo;
            return View();
        }

        public JsonResult getDoorLocation(string id)
        {

            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            Function func = new Function();
            try
            {
                dbconn.openConnection();
                string strSQL = "";


                strSQL = @"SELECT ID,DOOR FROM DOOR WHERE LOCATION=@LOCATION ORDER BY DOOR ASC";


                List<mdlDoor> model_list = new List<mdlDoor>();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = id;
                SqlDataReader  dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlDoor list = new mdlDoor();
                        list.ID = dr["ID"].ToString();
                        list.DOOR = dr["DOOR"].ToString();
                        model_list.Add(list);
                    }
                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();



                return Json(model_list.ToList());
            }
            finally
            {
                dbconn.closeConnection();
            }





        }

        public ActionResult accessdoor_list(string door)
        {

            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();

                string strSQL = "";
                strSQL = @"SELECT ID,F_ID,FIREARMTYPE,ACCESS,LOCATION,DOOR,DEFPIC FROM VWEAPON_DOOR_ACCESS WHERE DOORID=@DOOR";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("DOOR", SqlDbType.VarChar).Value = door;


                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlTrack> record = new List<mdlTrack>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlTrack list = new mdlTrack();
                        list.FIREARMTYPE = dr["FIREARMTYPE"].ToString();
                        list.F_ID = dr["F_ID"].ToString();
                        list.ACCESS = dr["ACCESS"].ToString();
                        list.ID = dr["ID"].ToString();
                        list.LOCATION = dr["LOCATION"].ToString();
                        list.DOOR = dr["DOOR"].ToString();
                        list.DEFPIC = dr["DEFPIC"].ToString();
                        list.DEFPIC = "/Content/firearms/NoImage.png";
                        if (!string.IsNullOrEmpty(dr["DEFPIC"].ToString()))
                        {
                            list.DEFPIC = "/" + dr["DEFPIC"].ToString();
                        }
                        record.Add(list);
                    }
                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();
                return PartialView(record);
            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public ActionResult AccessDoorAdd()
        {
            if (Session["message"] == null) Session["message"] = "";
            Populate DropDown = new Populate();

            try
            {
                List<SelectListItem> door_combo = new List<SelectListItem>();
                door_combo = DropDown.getDoor();
                



                ViewData["door"] = door_combo;
                return View();

            }
            finally
            {
            }

        }


        public ActionResult AccessDoorEdit(string id)
        {
            Populate DropDown = new Populate();
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> allowdeny_combo = new List<SelectListItem>();
            allowdeny_combo = DropDown.getAllowDeny();
            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT LOCATION,DOOR,ACCESS FROM VWEAPON_DOOR_ACCESS WHERE ID=@ID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlTrack record = new mdlTrack();
                record.ID = id;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.ID = id;
                    record.LOCATION = dr["LOCATION"].ToString();
                    record.DOOR = dr["DOOR"].ToString();
                    record.ACCESS = dr["ACCESS"].ToString();

                }


                foreach (SelectListItem item in allowdeny_combo)
                {
                    if (item.Text.ToUpper() == record.ACCESS.ToUpper())
                    {
                        item.Selected = true;
                    }
                }

                ViewData["access"] = allowdeny_combo;
                return PartialView(record);

            }
            finally
            {
                dbconn.closeConnection();
            }

        }

        [HttpPost]
        public void updateweaponaccessdoor()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtid = Request.Form["txtid"].ToString();
            string txtaccess = Request.Form["txtaccess"].ToString();
            try
            {


                strSQL = @"UPDATE WEAPON_DOOR_ACCESS SET ACCESS=@ACCESS,DATEMODIFIED=GETDATE() WHERE ID=@ID";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);


                cmd.Parameters.Add("ACCESS", SqlDbType.VarChar).Value = txtaccess;
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = txtid;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbconn.closeConnection();



            }
            finally
            {
                dbconn.closeConnection();
            }


        }

        public string isWeaponAccessDoor(string id)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";





                    strSQL = @"DELETE FROM WEAPON_DOOR_ACCESS WHERE ID=@ID";
                    SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                    cmd.ExecuteNonQuery();

                dbconn.closeConnection();

                return strResult;
            }
            finally
            {
                dbconn.closeConnection();
            }

        }

        [HttpPost]
        public ActionResult saveweaponaccessdoor()
        {
            Utility util = new Utility();
            Function func = new Function();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtdoor = Request.Form["txtdoor"].ToString();
            string txtcontrol = Request.Form["txtcontrol"].ToString();
            string txtfirearms = Request.Form["txtfirearms"].ToString();
            string strResult = "";
            txtfirearms = txtfirearms.Replace(" ", "");
            string gid = "";
            try
            {

                string[] firearms = txtfirearms.Split(',');
                SqlDataReader dr;

                foreach (string s in firearms)
                {
                    if (s != "")
                    {
                        strSQL = @"SELECT ID FROM WEAPON_DOOR_ACCESS WHERE DOOR=@DOOR AND F_ID=@F_ID ";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("DOOR", SqlDbType.VarChar).Value = txtdoor;
                        cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Dispose();
                            cmd.Dispose();
                            strSQL = @"UPDATE WEAPON_DOOR_ACCESS SET ACCESS=@ACCESS,MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE() WHERE DOOR=@DOOR AND F_ID=@F_ID ";
                            cmd = new SqlCommand(strSQL, dbconn.DbConn);
                            cmd.Parameters.Add("ACCESS", SqlDbType.VarChar).Value = txtcontrol;
                            cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                            cmd.Parameters.Add("DOOR", SqlDbType.VarChar).Value = txtdoor;
                            cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            dr.Dispose();
                            cmd.Dispose();
                            gid = util.GenerateId();


                            strSQL = @"INSERT INTO WEAPON_DOOR_ACCESS(GID,F_ID,DOOR,ACCESS,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED)
                            VALUES(@GID,@F_ID,@DOOR,@ACCESS,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE())";
                            cmd = new SqlCommand(strSQL, dbconn.DbConn);
                            cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                            cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                            cmd.Parameters.Add("DOOR", SqlDbType.VarChar).Value = txtdoor;
                            cmd.Parameters.Add("ACCESS", SqlDbType.VarChar).Value = txtcontrol;
                            cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                            cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                cmd.Dispose();
                dbconn.closeConnection();

                    strResult="Weapon access door successfully added...";

                Session["message"] = strResult;

                return RedirectToAction("accessdooradd");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }

	}
}