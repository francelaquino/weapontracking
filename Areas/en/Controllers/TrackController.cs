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
    public class TrackController : Controller
    {
        //
        // GET: /en/track/
        public ActionResult Location()
        {
            Populate DropDown = new Populate();
            List<SelectListItem> location_combo = new List<SelectListItem>();
            location_combo = DropDown.getLocation();
            ViewData["location"] = location_combo;
            string txtdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["date"] = txtdate;
            return View();
        }


        public ActionResult location_modal(string id)
        {
         

            try
            {

                Db_Connection dbconn = new Db_Connection();

                dbconn.openConnection();

                string strSQL = "";
               
                strSQL = @"SELECT X,Y,LOCATION,IMAGE,MAPTITLE,DESCRIPTION FROM VMAP_LOCATION WHERE LOCATIONID=@ID";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;


                SqlDataReader dr = cmd.ExecuteReader();
                mdlTrack location = new mdlTrack();
                if (dr.HasRows)
                {
                   dr.Read();
                        
                        location.X = dr["X"].ToString();
                        location.Y = dr["Y"].ToString();
                        location.LOCATION = dr["LOCATION"].ToString();
                        location.MAPIMAGE = dr["IMAGE"].ToString();
                        location.MAPTITLE = dr["MAPTITLE"].ToString();
                    location.DESCRIPTION=dr["DESCRIPTION"].ToString();
                    
                }
                dr.Dispose();
                cmd.Dispose();







                mdlMapModel maprecord = new mdlMapModel();


                maprecord.mapheight = "2000";
                maprecord.mapwidth = "2000";





               

                List<mdlMapCategories> categories_list = new List<mdlMapCategories>();
                mdlMapCategories categories = new mdlMapCategories();

                List<mdlMapLocations> locations_list = new List<mdlMapLocations>();


                mdlMapLocations locations = new mdlMapLocations();
                locations.id = id;
                locations.title = location.LOCATION;
                locations.pin = "pin-pulse";
                locations.category = "location";
                locations.description=location.DESCRIPTION;
                locations.x = location.X;
                locations.y = location.Y;
                locations.zoom = "1";
                locations_list.Add(locations);






                


                categories.id = "location";
                categories.title = location.MAPTITLE;
                categories.color = "#4cd3b8";
                categories.show = "true";
                categories_list.Add(categories);



                List<mdlMapLevels> level_list = new List<mdlMapLevels>();
                mdlMapLevels level = new mdlMapLevels();
                level.id = "lower";
                level.title = "Lower floor";
                level.map = "/" + location.MAPIMAGE;
                level.locations = locations_list;
                level_list.Add(level);

                maprecord.levels = level_list;






                maprecord.categories = categories_list;

                ViewData["maprecord"] = JsonConvert.SerializeObject(maprecord);
                ViewData["maptitle"] = location.MAPTITLE;


                dbconn.closeConnection();
            
                return PartialView();

            }
            finally
            {
            }






        }
        public ActionResult location_list(string from, string to, string location)
        {

            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();

                string strSQL = "";
                string strWHERE = "";
                strSQL = @"SELECT TAGS,READTIME,LOCATION,FIREARMTYPE,DEFPIC FROM VRFID_LOGS 
                WHERE READTIME>=@FROM AND READTIME<=DATEADD(day, 1,@TO) AND LOCATIONID=@LOCATION";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("FROM", SqlDbType.VarChar).Value = from;
                cmd.Parameters.Add("TO", SqlDbType.VarChar).Value = to;
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = location;


                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlTrack> record = new List<mdlTrack>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlTrack list = new mdlTrack();
                        list.TAGS = dr["TAGS"].ToString();
                        list.READTIME = util.formatLongDateTime(Convert.ToDateTime(dr["READTIME"].ToString()));
                        list.LOCATION = dr["LOCATION"].ToString();
                        list.LOCATIONID = location;
                        list.FIREARMTYPE = dr["FIREARMTYPE"].ToString();
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
	}
}