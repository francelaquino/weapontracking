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
    public class ZoneController : Controller
    {
        //
        // GET: /en/Zone/
        public ActionResult Index()
        {
            string maptitle = "";
            string mapimage = "";
            Populate DropDown = new Populate();
            string map = "";
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
                    locations.zoom = "1";
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
	}
}