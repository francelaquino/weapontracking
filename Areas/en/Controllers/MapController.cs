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
    public class MapController : Controller
    {
        //
        // GET: /en/Map/
        public string test(){
            mdlMapModel record = new mdlMapModel();
            record.mapheight = "2000";
            record.mapwidth = "2000";
           
            
            List<mdlMapCategories> categories_list = new List<mdlMapCategories>();
            mdlMapCategories categories = new mdlMapCategories();
            categories.id = "location";
            categories.title = "Locations";
            categories.color = "#4cd3b8";
            categories.show = "true";
            categories_list.Add(categories);

            List<mdlMapLocations> locations_list = new List<mdlMapLocations>();
            mdlMapLocations locations = new mdlMapLocations();
            locations.id = "diningtable";
            locations.title = "Dining Table";
            locations.category = "location";
            locations.x = "0.4746";
            locations.pin = "circular pin-md pin-label";
            locations.fill = "#4d5e6d";
            locations.y = "0.2883";
            locations.zoom = "3";
            locations_list.Add(locations);

            mdlMapLevels level = new mdlMapLevels();
            level.id = "lower";
            level.title = "Lower floor";
            level.map = "/Content/map/1_1eed5a4478d73e34.jpg";
            level.locations = locations_list;


           // record.levels = level;


           



            record.categories=categories_list;

            return JsonConvert.SerializeObject(record);
        }
        public ActionResult definelocation()
        {
            string maptitle = "";
            string mapimage = "";
            Populate DropDown = new Populate();
            string map="";
            if (!string.IsNullOrEmpty(Request.QueryString["map"]))
            {
                map = Request.QueryString["map"].ToString();
            }
            
            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT ID,MAP,LOCATIONS FROM VMAP ORDER BY MAP ASC";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlMap> record = new List<mdlMap>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlMap list = new mdlMap();
                        list.ID = dr["ID"].ToString();
                        list.MAP = dr["MAP"].ToString();
                        list.LOCATIONS = dr["LOCATIONS"].ToString();
                        record.Add(list);
                        if (map == "")
                        {
                            map = list.ID;
                        }
                    }
                }
                dr.Dispose();
                cmd.Dispose();

                strSQL = @"SELECT MAP,IMAGE FROM MAP WHERE ID=@ID";


                 cmd = new SqlCommand(strSQL, dbconn.DbConn);
                 cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = map;
                 dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    maptitle = dr["MAP"].ToString();
                    mapimage = dr["IMAGE"].ToString();
                }
                dr.Dispose();
                cmd.Dispose();




                mdlMapModel maprecord = new mdlMapModel();


                maprecord.mapheight = "2000";
                maprecord.mapwidth = "2000";


              


                strSQL = @"SELECT MAPTITLE,LOCATIONID,ID,MAP,LOCATION,X,Y FROM VMAP_LOCATION WHERE MAP=@MAP ORDER BY LOCATION ASC";


                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("MAP", SqlDbType.VarChar).Value = map;
                dr = cmd.ExecuteReader();

                List<mdlMapCategories> categories_list = new List<mdlMapCategories>();
                mdlMapCategories categories = new mdlMapCategories();

                List<mdlMapLocations> locations_list = new List<mdlMapLocations>();
               
                while (dr.Read())
                {
                    mdlMapLocations locations = new mdlMapLocations();
                    locations.id = dr["LOCATIONID"].ToString();
                    locations.title = dr["LOCATION"].ToString();
                    locations.category = "location";
                    locations.x = dr["X"].ToString();
                    locations.y = dr["Y"].ToString();
                    locations_list.Add(locations);
                }

               
                categories.id = "location";
                categories.title = maptitle;
                categories.color = "#4cd3b8";
                categories.show = "true";
                categories_list.Add(categories);



                List<mdlMapLevels> level_list = new List<mdlMapLevels>();
                mdlMapLevels level = new mdlMapLevels();
                level.id = "lower";
                level.title = "Lower floor";
                level.map = "/" + mapimage;
                level.locations = locations_list;
                level_list.Add(level);

                maprecord.levels = level_list;






                maprecord.categories = categories_list;

                ViewData["maprecord"] = JsonConvert.SerializeObject(maprecord);
               
                List<SelectListItem> location_combo = new List<SelectListItem>();

                dr.Dispose();
                cmd.Dispose();

                strSQL = "SELECT LOCATIONID,LOCATION FROM VMAP_LOCATION WHERE MAP=@MAP ORDER BY LOCATION ASC";
                 cmd = new SqlCommand(strSQL, dbconn.DbConn);
                 cmd.Parameters.Add("MAP", SqlDbType.VarChar).Value = map;
                 dr = cmd.ExecuteReader();
                 location_combo.Add(new SelectListItem
                {
                    Text = "",
                    Value = ""
                });
                 while (dr.Read())
                 {
                     location_combo.Add(new SelectListItem
                     {
                         Text = dr[1].ToString(),
                         Value = dr[0].ToString()

                     });

                 }






                ViewData["location"] = location_combo;
                ViewData["mapid"] = map;


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
        public ActionResult Index()
        {
            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT ID,GID,MAP,IMAGE,DESCRIPTION FROM MAP ORDER BY MAP ASC";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlMap> record = new List<mdlMap>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlMap list = new mdlMap();
                        list.ID = dr["ID"].ToString();
                        list.GID = dr["GID"].ToString();
                        list.MAP = dr["MAP"].ToString();
                        list.DESCRIPTION = dr["DESCRIPTION"].ToString();
                        list.IMAGE = "/"+dr["IMAGE"].ToString();
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

        public ActionResult edit(string id, string gid)
        {
            Populate DropDown = new Populate();
            Db_Connection dbconn = new Db_Connection();

            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT MAP,DESCRIPTION FROM MAP WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlMap record = new mdlMap();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.ID = id;
                    record.GID = gid;
                    record.MAP = dr["MAP"].ToString();
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
        public ActionResult saveaddlocation()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtmapid = Request.Form["txtmapid"].ToString();
            string txtx = Request.Form["txtx"].ToString();
            string txty = Request.Form["txty"].ToString();
            string txtlocation = Request.Form["txtlocation"].ToString();
            try
            {


                strSQL = @"INSERT INTO MAP_LOCATION(MAP,LOCATION,X,Y,DATEENCODED,ENCODEDBY)
                    VALUES(@MAP,@LOCATION,@X,@Y,GETDATE(),@ENCODEDBY)";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("MAP", SqlDbType.VarChar).Value = txtmapid;
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = txtlocation;
                cmd.Parameters.Add("X", SqlDbType.VarChar).Value = txtx;
                cmd.Parameters.Add("Y", SqlDbType.VarChar).Value = txty;
                cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";


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


        public string isLocExist(string id, string gid)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";
                strSQL = @"SELECT ID FROM MAP_LOCATION WHERE   MAP=@MAP AND LOCATION=@LOCATION";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("MAP", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    strResult = "found";

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

        public ActionResult addloc(string id, string gid)
        {
            Populate DropDown = new Populate();
            List<SelectListItem> location_combo = new List<SelectListItem>();
            location_combo = DropDown.getUnAssignedLocation();

            ViewData["location"] = location_combo;
            ViewData["x"] =id;
            ViewData["y"] = gid;
            return PartialView();



        }

        public string isDelete(string id)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";

                //strSQL = @"SELECT ID FROM ASSETS WHERE  ACCESSORIES=@ACCESSORIES";
                //SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                //cmd.Parameters.Add("ACCESSORIES", SqlDbType.VarChar).Value = id;
                //SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows)
                //{
                //    strResult = "Found";
                //}
                //dr.Dispose();
                //cmd.Dispose();




                //if (strResult == "")
                //{
                //    strSQL = @"SELECT ID FROM LINENS WHERE  ACCESSORIES=@ACCESSORIES";
                //    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                //    cmd.Parameters.Add("ACCESSORIES", SqlDbType.VarChar).Value = id;
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

                    strSQL = @"DELETE FROM MAP WHERE ID=@ID";
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


        public string deleteMapLocation(string id, string gid)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";





                strSQL = @"DELETE FROM MAP_LOCATION WHERE MAP=@MAP AND LOCATION=@LOCATION";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("MAP", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = gid;
                cmd.ExecuteNonQuery();

                dbconn.closeConnection();

                return strResult;
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


           [HttpPost]
           public ActionResult savemap(IEnumerable<HttpPostedFileBase> files)
           {
               Utility util = new Utility();
               Function func = new Function();
               Db_Connection dbconn = new Db_Connection();
               SqlCommand cmd = new SqlCommand();
               string strSQL = "";
               dbconn.openConnection();
               string txttitle = Request.Form["txttitle"].ToString();
               string txtdescription = Request.Form["txtdescription"].ToString();

               string gid = "";
               string id = "";
               string image = "";
               try
               {
                   gid = util.GenerateId();


                  


                   strSQL = @"INSERT INTO MAP(GID,MAP,IMAGE,DATEENCODED,ENCODEDBY,DATEMODIFIED,MODIFIEDBY,DESCRIPTION
                    VALUES(@GID,@MAP,@IMAGE,GETDATE(),@ENCODEDBY,GETDATE(),@MODIFIEDBY,@DESCRIPTION)";

                   cmd = new SqlCommand(strSQL, dbconn.DbConn);
                   cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                   cmd.Parameters.Add("MAP", SqlDbType.VarChar).Value = txttitle;
                   cmd.Parameters.Add("IMAGE", SqlDbType.VarChar).Value = "";
                   cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                   cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                   cmd.Parameters.Add("DESCRIPTION", SqlDbType.VarChar).Value = txtdescription;

                   cmd.ExecuteNonQuery();
                   cmd.Dispose();


                   
                 id = func.getCurrentId("MAP");

                 foreach (var file in files)
                 {
                     if (file != null && file.ContentLength > 0)
                     {

                         string extension = Path.GetExtension(file.FileName);
                         file.SaveAs(HttpContext.Server.MapPath("~/Content/map/") + id + "_" + gid + extension);
                         image = "Content/map/" + id + "_" + gid + extension;


                         strSQL = @"UPDATE MAP SET IMAGE=@IMAGE WHERE ID=@ID AND GID=@GID";
                         cmd = new SqlCommand(strSQL, dbconn.DbConn);
                         cmd.Parameters.Add("IMAGE", SqlDbType.VarChar).Value = image;
                         cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                         cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                         cmd.ExecuteNonQuery();
                         cmd.Dispose();

                     }
                 }
               


                   dbconn.closeConnection();




                   Session["message"] = "Map successfully saved...";

                   return RedirectToAction("add");


               }
               finally
               {
                   dbconn.closeConnection();
               }


           }

           [HttpPost]
           public ActionResult updatemap(IEnumerable<HttpPostedFileBase> files)
           {
               Utility util = new Utility();
               Db_Connection dbconn = new Db_Connection();
               SqlCommand cmd = new SqlCommand();
               string strSQL = "";
               dbconn.openConnection();
               string txtid = Request.Form["txtid"].ToString();
               string txtgid = Request.Form["txtgid"].ToString();
               string txttitle = Request.Form["txttitle"].ToString();
               string txtdescription = Request.Form["txtdescription"].ToString();
               string image = "";
               try
               {


                   strSQL = @"UPDATE MAP SET MAP=@MAP,DATEMODIFIED=GETDATE(),MODIFIEDBY=@MODIFIEDBY,DESCRIPTION=@DESCRIPTION
                WHERE ID=@ID AND GID=@GID";
                   cmd = new SqlCommand(strSQL, dbconn.DbConn);

                   cmd.Parameters.Add("MAP", SqlDbType.VarChar).Value = txttitle;
                   cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                   cmd.Parameters.Add("DESCRIPTION", SqlDbType.VarChar).Value = txtdescription;
                   cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = txtid;
                   cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = txtgid;


                   cmd.ExecuteNonQuery();
                   cmd.Dispose();

                   if (files != null)
                   {
                       foreach (var file in files)
                       {
                           if (file != null && file.ContentLength > 0)
                           {

                               string extension = Path.GetExtension(file.FileName);
                               file.SaveAs(HttpContext.Server.MapPath("~/Content/map/") + txtid + "_" + txtgid + extension);
                               image = "Content/map/" + txtid + "_" + txtgid + extension;


                               strSQL = @"UPDATE MAP SET IMAGE=@IMAGE WHERE ID=@ID AND GID=@GID";
                               cmd = new SqlCommand(strSQL, dbconn.DbConn);
                               cmd.Parameters.Add("IMAGE", SqlDbType.VarChar).Value = image;
                               cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = txtid;
                               cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = txtgid;
                               cmd.ExecuteNonQuery();
                               cmd.Dispose();
                           }
                       }
                   }

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
                   strSQL = @"SELECT ID FROM MAP WHERE  UPPER(MAP)=UPPER(@MAP)";
                   SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                   cmd.Parameters.Add("MAP", SqlDbType.VarChar).Value = gid;
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
	}
}