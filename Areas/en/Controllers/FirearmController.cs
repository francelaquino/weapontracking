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
using System.Net;
using System.Net.Sockets;
namespace FMS.Areas.en.Controllers
{
    public class FirearmController : Controller
    {



        public ActionResult Index()
        {
            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT ID,GID,STORAGE FROM STORAGE ORDER BY STORAGE ASC";


               
                return View();
            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public ActionResult search_list()
        {

            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT ID,GID,RFID,MODEL,FIREARMTYPE,CALIBER,CONDITION,DEFPIC,STATUS FROM VFIREARMS";



                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);



                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlFirearm> record = new List<mdlFirearm>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlFirearm list = new mdlFirearm();
                        list.ID = dr["ID"].ToString();
                        list.GID = dr["GID"].ToString();
                        list.RFID = dr["RFID"].ToString();
                        list.MODEL = dr["MODEL"].ToString();
                        list.FIREARMTYPE = dr["FIREARMTYPE"].ToString();
                        list.CALIBER = dr["CALIBER"].ToString();
                        list.STATUS = dr["STATUS"].ToString();
                        list.CONDITION = dr["CONDITION"].ToString();
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

        public ActionResult checkFirearms()
        {
            Populate DropDown = new Populate();
            List<SelectListItem> status_combo = new List<SelectListItem>();
            List<SelectListItem> firearmtype_combo = new List<SelectListItem>();
            status_combo = DropDown.getStatusEN();
            firearmtype_combo = DropDown.getFirearmType();
            ViewData["status"] = status_combo;
            ViewData["firearmtype"] = firearmtype_combo;
            return View();

        }
        public ActionResult checkFirearms_list(string batchno,string status, string firearmtype)
        {

            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();

                string strSQL = "";
                string strWHERE = "";
                strSQL = @"SELECT F_ID,NAME,FIREARMTYPE,BATCHNO,DEFPIC,STATUS FROM VASSIGN_FIREARMS ";
                if (batchno != "")
                {
                    strWHERE = strWHERE + "UPPER(BATCHNO)=UPPER(@BATCHNO) AND ";
                }
                if (status != "")
                {
                    strWHERE = strWHERE + "STATUS=@STATUS AND ";
                }
                if (firearmtype != "")
                {
                    strWHERE = strWHERE + "FIREARMTYPEID=@FIREARMTYPE AND ";
                }
               
                if (strWHERE != "")
                {

                    strSQL = strSQL + " WHERE "+strWHERE;
                    strSQL = strSQL.Substring(0, strSQL.Length - 4);
                }
                


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                if (batchno != "")
                {
                    cmd.Parameters.Add("BATCHNO", SqlDbType.VarChar).Value = batchno;

                }
                if (status != "")
                {
                    cmd.Parameters.Add("STATUS", SqlDbType.VarChar).Value = status;

                }
                if (firearmtype != "")
                {
                    cmd.Parameters.Add("FIREARMTYPE", SqlDbType.VarChar).Value = firearmtype;

                }

                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlFirearm> record = new List<mdlFirearm>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlFirearm list = new mdlFirearm();
                        list.NAME = dr["BATCHNO"].ToString() + " - " + dr["NAME"].ToString();
                        list.FIREARMTYPE = dr["FIREARMTYPE"].ToString();
                        list.STATUS = dr["STATUS"].ToString();
                        list.ID = dr["F_ID"].ToString();
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

        
        public ActionResult picture_list(string id,string gid)
        {

            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT ID,F_ID,F_GID,PATH,FILENAME,DEFPIC FROM FIREARMS_PICTURES WHERE F_ID=@ID AND F_GID=@GID ORDER BY ID";



                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;


                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlFirearm> record = new List<mdlFirearm>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlFirearm list = new mdlFirearm();
                        list.IMG_ID = dr["ID"].ToString();
                        list.ID = dr["F_ID"].ToString();
                        list.GID = dr["F_GID"].ToString();
                        list.DEFPIC = dr["DEFPIC"].ToString();
                        list.PATH = "/"+dr["PATH"].ToString();
                        list.FILENAME = dr["FILENAME"].ToString();
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

        public string isRFIDExist(string id, string gid)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";
                strSQL = @"SELECT ID FROM FIREARMS WHERE  UPPER(RFID)=UPPER(@RFID)";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = gid;
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



        public ActionResult add()
        {
            if (Session["message"] == null) Session["message"] = "";
            Populate DropDown = new Populate();
            List<SelectListItem> firearmtype_combo = new List<SelectListItem>();
            List<SelectListItem> storage_combo = new List<SelectListItem>();
            List<SelectListItem> manufacturer_combo = new List<SelectListItem>();
            List<SelectListItem> supplier_combo = new List<SelectListItem>();
            List<SelectListItem> condition_combo = new List<SelectListItem>();
            firearmtype_combo = DropDown.getFirearmType();
            manufacturer_combo = DropDown.getManufacturer();
            supplier_combo = DropDown.getSupplier();
            condition_combo = DropDown.getCondition();
            storage_combo = DropDown.getStorage();

            ViewData["firearmtype"] = firearmtype_combo;
            ViewData["manufacturer"] = manufacturer_combo;
            ViewData["supplier"] = supplier_combo;
            ViewData["storage"] = storage_combo;
            ViewData["condition"] = condition_combo;

            return View();
        }

        public ActionResult edit(string id,string gid)
        {
            if (Session["message"] == null) Session["message"] = "";
            Populate DropDown = new Populate();
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> firearmtype_combo = new List<SelectListItem>();
            List<SelectListItem> storage_combo = new List<SelectListItem>();
            List<SelectListItem> manufacturer_combo = new List<SelectListItem>();
            List<SelectListItem> supplier_combo = new List<SelectListItem>();
            List<SelectListItem> condition_combo = new List<SelectListItem>();
            firearmtype_combo = DropDown.getFirearmType();
            manufacturer_combo = DropDown.getManufacturer();
            supplier_combo = DropDown.getSupplier();
            condition_combo = DropDown.getCondition();
            storage_combo = DropDown.getStorage();

            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT ID,GID,RFID,FIREARMTYPE,CALIBER,MODEL,SERIALNO,BARCODE,MANUFACTURER,CONDITION,FINISH,
                CAPACITY,LENGTH,ADDINFO,STORAGE,RACK,SUPPLIER,FINISH,CAPACITY,LENGTH,ADDINFO
                FROM FIREARMS WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlFirearm record = new mdlFirearm();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.RFID = dr["RFID"].ToString();
                    record.FIREARMTYPE = dr["FIREARMTYPE"].ToString();
                    record.CALIBER = dr["CALIBER"].ToString();
                    record.MODEL = dr["MODEL"].ToString();
                    record.SERIALNO = dr["SERIALNO"].ToString();
                    record.BARCODE = dr["BARCODE"].ToString();
                    record.MANUFACTURER = dr["MANUFACTURER"].ToString();
                    record.CONDITION = dr["CONDITION"].ToString();
                    record.FINISH = dr["FINISH"].ToString();
                    record.CAPACITY = dr["CAPACITY"].ToString();
                    record.LENGTH = dr["LENGTH"].ToString();
                    record.ADDINFO = dr["ADDINFO"].ToString();
                    record.STORAGE = dr["STORAGE"].ToString();
                    record.RACK = dr["RACK"].ToString();
                    record.FINISH = dr["FINISH"].ToString();
                    record.CAPACITY = dr["CAPACITY"].ToString();
                    record.LENGTH = dr["LENGTH"].ToString();
                    record.ADDINFO = dr["ADDINFO"].ToString();
                    record.SUPPLIER = dr["SUPPLIER"].ToString();

                }


                foreach (SelectListItem item in firearmtype_combo)
                {
                    if (item.Value == record.FIREARMTYPE)
                    {
                        item.Selected = true;
                    }
                }

                foreach (SelectListItem item in manufacturer_combo)
                {
                    if (item.Value == record.MANUFACTURER)
                    {
                        item.Selected = true;
                    }
                }

                foreach (SelectListItem item in supplier_combo)
                {
                    if (item.Value == record.SUPPLIER)
                    {
                        item.Selected = true;
                    }
                }

                foreach (SelectListItem item in condition_combo)
                {
                    if (item.Value == record.CONDITION)
                    {
                        item.Selected = true;
                    }
                }

                foreach (SelectListItem item in storage_combo)
                {
                    if (item.Value == record.STORAGE)
                    {
                        item.Selected = true;
                    }
                }

              

                ViewData["firearmtype"] = firearmtype_combo;
                ViewData["manufacturer"] = manufacturer_combo;
                ViewData["supplier"] = supplier_combo;
                ViewData["storage"] = storage_combo;
                ViewData["condition"] = condition_combo;
                return View(record);

            }
            finally
            {
                dbconn.closeConnection();
            }





          
        }


        public ActionResult details(string id)
        {
            Db_Connection dbconn = new Db_Connection();

            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT NAME,IDNO,ID,GID,RFID,FIREARMTYPE,CALIBER,MODEL,SERIALNO,BARCODE,MANUFACTURER,CONDITION,FINISH,
                CAPACITY,LENGTH,ADDINFO,STORAGE,RACK,SUPPLIER,FINISH,CAPACITY,LENGTH,ADDINFO
                FROM VFIREARMS WHERE ID=@ID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlFirearm record = new mdlFirearm();
                record.ID = id;
               
                if (dr.HasRows)
                {
                    dr.Read();
                    record.RFID = dr["RFID"].ToString();
                    record.GID = dr["GID"].ToString();
                    record.FIREARMTYPE = dr["FIREARMTYPE"].ToString();
                    record.CALIBER = dr["CALIBER"].ToString();
                    record.MODEL = dr["MODEL"].ToString();
                    record.SERIALNO = dr["SERIALNO"].ToString();
                    record.BARCODE = dr["BARCODE"].ToString();
                    record.MANUFACTURER = dr["MANUFACTURER"].ToString();
                    record.CONDITION = dr["CONDITION"].ToString();
                    record.FINISH = dr["FINISH"].ToString();
                    record.CAPACITY = dr["CAPACITY"].ToString();
                    record.LENGTH = dr["LENGTH"].ToString();
                    record.ADDINFO = dr["ADDINFO"].ToString();
                    record.STORAGE = dr["STORAGE"].ToString();
                    record.RACK = dr["RACK"].ToString();
                    record.FINISH = dr["FINISH"].ToString();
                    record.CAPACITY = dr["CAPACITY"].ToString();
                    record.LENGTH = dr["LENGTH"].ToString();
                    record.ADDINFO = dr["ADDINFO"].ToString();
                    record.SUPPLIER = dr["SUPPLIER"].ToString();
                    if (string.IsNullOrEmpty(dr["IDNO"].ToString()))
                    {
                        record.NAME = "Unassign Weapon";
                    }
                    else
                    {
                        record.NAME = dr["IDNO"].ToString() + " - " + dr["NAME"].ToString();
                    }

                }


                return PartialView(record);

            }
            finally
            {
                dbconn.closeConnection();
            }






        }

        [HttpPost]
        public ActionResult savefirearm(IEnumerable<HttpPostedFileBase> files)
        {
            Utility util = new Utility();
            Function func = new Function();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtfirearmtype = Request.Form["txtfirearmtype"].ToString();
            string txtmanufacturer = Request.Form["txtmanufacturer"].ToString();
            string txtcaliber = Request.Form["txtcaliber"].ToString();
            string txtrfid = Request.Form["txtrfid"].ToString();
            string txtmodel = Request.Form["txtmodel"].ToString();
            string txtserialno = Request.Form["txtserialno"].ToString();
            string txtbarcode = Request.Form["txtbarcode"].ToString();
            string txtsupplier = Request.Form["txtsupplier"].ToString();
            string txtcondition = Request.Form["txtcondition"].ToString();
            string txtfinish = Request.Form["txtfinish"].ToString();
            string txtcapacity = Request.Form["txtcapacity"].ToString();
            string txtlength = Request.Form["txtlength"].ToString();
            string txtaddinfo = Request.Form["txtaddinfo"].ToString();
            string txtstorage = Request.Form["txtstorage"].ToString();
            string txtrack = Request.Form["txtrack"].ToString();

            string gid = "";
            string id = "";
            int cnt = 0;
            int x = 1;
            string image = "";
            try
            {
                gid = util.GenerateId();
               

                strSQL = @"INSERT INTO FIREARMS(GID,RFID,FIREARMTYPE,CALIBER,MODEL,SERIALNO,BARCODE,MANUFACTURER,CONDITION,DATEENCODED,ENCODEDBY,DATEMODIFIED,MODIFIEDBY,
                    FINISH,CAPACITY,LENGTH,ADDINFO,STORAGE,RACK,SUPPLIER)
                    VALUES(@GID,@RFID,@FIREARMTYPE,@CALIBER,@MODEL,@SERIALNO,@BARCODE,@MANUFACTURER,@CONDITION,GETDATE(),@ENCODEDBY,GETDATE(),@MODIFIEDBY,
                    @FINISH,@CAPACITY,@LENGTH,@ADDINFO,@STORAGE,@RACK,@SUPPLIER)";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = txtrfid;
                cmd.Parameters.Add("FIREARMTYPE", SqlDbType.VarChar).Value = txtfirearmtype;
                cmd.Parameters.Add("CALIBER", SqlDbType.VarChar).Value = txtcaliber;
                cmd.Parameters.Add("MODEL", SqlDbType.VarChar).Value = txtmodel;
                cmd.Parameters.Add("SERIALNO", SqlDbType.VarChar).Value = txtserialno;
                cmd.Parameters.Add("BARCODE", SqlDbType.VarChar).Value = txtbarcode;
                cmd.Parameters.Add("MANUFACTURER", SqlDbType.VarChar).Value = txtmanufacturer;
                cmd.Parameters.Add("CONDITION", SqlDbType.VarChar).Value = txtcondition;
                cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("FINISH", SqlDbType.VarChar).Value = txtfinish;
                cmd.Parameters.Add("CAPACITY", SqlDbType.VarChar).Value = txtcapacity;
                cmd.Parameters.Add("LENGTH", SqlDbType.VarChar).Value = txtlength;
                cmd.Parameters.Add("ADDINFO", SqlDbType.VarChar).Value = txtaddinfo;
                cmd.Parameters.Add("STORAGE", SqlDbType.VarChar).Value = txtstorage;
                cmd.Parameters.Add("RACK", SqlDbType.VarChar).Value = txtrack;
                cmd.Parameters.Add("SUPPLIER", SqlDbType.VarChar).Value = txtsupplier;

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                 id = func.getCurrentId("FIREARMS");

                 foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {

                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(HttpContext.Server.MapPath("~/Content/firearms/") + x + "_" + id + extension);
                        image = "Content/firearms/" + x + "_" + id + extension;

                        strSQL = @"INSERT FIREARMS_PICTURES(ID,F_ID,F_GID,FILENAME,PATH,DATEENCODED,ENCODEDBY)
                        VALUES(@ID,@F_ID,@F_GID,@FILENAME,@PATH,GETDATE(),@ENCODEDBY)";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = x.ToString();
                        cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = id;
                        cmd.Parameters.Add("F_GID", SqlDbType.VarChar).Value = gid;
                        cmd.Parameters.Add("FILENAME", SqlDbType.VarChar).Value = file.FileName;
                        cmd.Parameters.Add("PATH", SqlDbType.VarChar).Value = image;
                        cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        if (cnt == 0 )
                        {
                            strSQL = @"UPDATE FIREARMS SET DEFPIC=@DEFPIC WHERE ID=@ID AND GID=@GID";
                            cmd = new SqlCommand(strSQL, dbconn.DbConn);
                            cmd.Parameters.Add("DEFPIC", SqlDbType.VarChar).Value = image;
                            cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                            cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            strSQL = @"UPDATE FIREARMS_PICTURES SET DEFPIC='Default' WHERE ID=@ID AND F_ID=@F_ID";
                            cmd = new SqlCommand(strSQL, dbconn.DbConn);
                            cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = x.ToString();
                            cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = id;
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            cnt = 1;

                        }
                        x++;


                        x++;
                    }
                }

                dbconn.closeConnection();




                Session["message"] = "Firearm successfully saved...";

                return RedirectToAction("add");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }


        [HttpPost]
        public ActionResult updatefirearm(IEnumerable<HttpPostedFileBase> files)
        {
            Utility util = new Utility();
            Function func = new Function();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtfirearmtype = Request.Form["txtfirearmtype"].ToString();
            string txtmanufacturer = Request.Form["txtmanufacturer"].ToString();
            string txtcaliber = Request.Form["txtcaliber"].ToString();
            string txtrfid = Request.Form["txtrfid"].ToString();
            string txtmodel = Request.Form["txtmodel"].ToString();
            string txtserialno = Request.Form["txtserialno"].ToString();
            string txtbarcode = Request.Form["txtbarcode"].ToString();
            string txtsupplier = Request.Form["txtsupplier"].ToString();
            string txtcondition = Request.Form["txtcondition"].ToString();
            string txtfinish = Request.Form["txtfinish"].ToString();
            string txtcapacity = Request.Form["txtcapacity"].ToString();
            string txtlength = Request.Form["txtlength"].ToString();
            string txtaddinfo = Request.Form["txtaddinfo"].ToString();
            string txtstorage = Request.Form["txtstorage"].ToString();
            string txtrack = Request.Form["txtrack"].ToString();
            string txtid = Request.Form["txtid"].ToString();
            string txtgid = Request.Form["txtgid"].ToString();
            string withDefPic = "";
            int cnt = 0;
            int x = 1;
            string image = "";
            try
            {
                 strSQL = @"SELECT MAX(ID) FROM FIREARMS_PICTURES 
                    WHERE F_ID=@ID AND F_GID=@GID";

                cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = txtid;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = txtgid;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (!string.IsNullOrEmpty(dr[0].ToString()))
                    {
                        if (Convert.ToInt16(dr[0].ToString()) > 0)
                        {
                            x = Convert.ToInt16(dr[0].ToString()) + 1;
                        }
                    }
                }

                cmd.Dispose();
                dr.Dispose();

                strSQL = @"SELECT DEFPIC FROM FIREARMS
                    WHERE ID=@ID AND GID=@GID";

                cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = txtid;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = txtgid;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (!string.IsNullOrEmpty(dr["DEFPIC"].ToString()))
                    {
                        withDefPic = "T";
                    }
                }

                cmd.Dispose();
                dr.Dispose();


                strSQL = @"UPDATE FIREARMS SET RFID=@RFID,FIREARMTYPE=@FIREARMTYPE,CALIBER=@CALIBER,MODEL=@MODEL,SERIALNO=@SERIALNO,
                    BARCODE=@BARCODE,MANUFACTURER=@MANUFACTURER,CONDITION=@CONDITION,DATEMODIFIED=GETDATE(),MODIFIEDBY=@MODIFIEDBY,
                    FINISH=@FINISH,CAPACITY=@CAPACITY,LENGTH=@LENGTH,ADDINFO=@ADDINFO,STORAGE=@STORAGE,RACK=@RACK,SUPPLIER=@SUPPLIER
                    WHERE ID=@ID AND GID=@GID";

                cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = txtrfid;
                cmd.Parameters.Add("FIREARMTYPE", SqlDbType.VarChar).Value = txtfirearmtype;
                cmd.Parameters.Add("CALIBER", SqlDbType.VarChar).Value = txtcaliber;
                cmd.Parameters.Add("MODEL", SqlDbType.VarChar).Value = txtmodel;
                cmd.Parameters.Add("SERIALNO", SqlDbType.VarChar).Value = txtserialno;
                cmd.Parameters.Add("BARCODE", SqlDbType.VarChar).Value = txtbarcode;
                cmd.Parameters.Add("MANUFACTURER", SqlDbType.VarChar).Value = txtmanufacturer;
                cmd.Parameters.Add("CONDITION", SqlDbType.VarChar).Value = txtcondition;
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("FINISH", SqlDbType.VarChar).Value = txtfinish;
                cmd.Parameters.Add("CAPACITY", SqlDbType.VarChar).Value = txtcapacity;
                cmd.Parameters.Add("LENGTH", SqlDbType.VarChar).Value = txtlength;
                cmd.Parameters.Add("ADDINFO", SqlDbType.VarChar).Value = txtaddinfo;
                cmd.Parameters.Add("STORAGE", SqlDbType.VarChar).Value = txtstorage;
                cmd.Parameters.Add("RACK", SqlDbType.VarChar).Value = txtrack;
                cmd.Parameters.Add("SUPPLIER", SqlDbType.VarChar).Value = txtsupplier;
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = txtid;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = txtgid;

                cmd.ExecuteNonQuery();
                cmd.Dispose();


                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {

                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(HttpContext.Server.MapPath("~/Content/firearms/") + x + "_" + txtid + extension);
                        image = "Content/firearms/" + x + "_" + txtid + extension;

                        strSQL = @"INSERT FIREARMS_PICTURES(ID,F_ID,F_GID,FILENAME,PATH,DATEENCODED,ENCODEDBY)
                        VALUES(@ID,@F_ID,@F_GID,@FILENAME,@PATH,GETDATE(),@ENCODEDBY)";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = x.ToString();
                        cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = txtid;
                        cmd.Parameters.Add("F_GID", SqlDbType.VarChar).Value = txtgid;
                        cmd.Parameters.Add("FILENAME", SqlDbType.VarChar).Value = file.FileName;
                        cmd.Parameters.Add("PATH", SqlDbType.VarChar).Value = image;
                        cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                       
                        if (cnt == 0 && withDefPic!="T")
                        {
                            strSQL = @"UPDATE FIREARMS SET DEFPIC=@DEFPIC WHERE ID=@ID AND GID=@GID";
                            cmd = new SqlCommand(strSQL, dbconn.DbConn);
                            cmd.Parameters.Add("DEFPIC", SqlDbType.VarChar).Value = image;
                            cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = txtid;
                            cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = txtgid;
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();

                            strSQL = @"UPDATE FIREARMS_PICTURES SET DEFPIC='Default' WHERE ID=@ID AND F_ID=@F_ID";
                            cmd = new SqlCommand(strSQL, dbconn.DbConn);
                            cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = x.ToString();
                            cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = txtid;
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            cnt = 1;

                        }
                        x++;
                    }
                }

                dbconn.closeConnection();




                Session["message"] = "Firearm successfully updated...";

                return RedirectToAction("edit/"+txtid+"/"+txtgid);


            }
            finally
            {
                dbconn.closeConnection();
            }


        }

        public string removePicture(string id,string gid)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string picture = "";
                string defPic = "";

                strSQL = @"SELECT PATH FROM FIREARMS_PICTURES  WHERE ID=@ID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    picture = dr[0].ToString();
                }

                cmd.Dispose();
                dr.Dispose();



                strSQL = @"DELETE FROM FIREARMS_PICTURES WHERE ID=@ID";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
               


                if (System.IO.File.Exists(HttpContext.Server.MapPath("~/" + picture)))
                {
                    System.IO.File.Delete(HttpContext.Server.MapPath("~/" + picture));
                }



                strSQL = @"SELECT ID,PATH FROM FIREARMS_PICTURES  WHERE F_ID=@F_ID ORDER BY ID";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = gid;
                dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"UPDATE FIREARMS SET DEFPIC='' WHERE ID=@ID";
                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = gid;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    dr.Read();
                    string path = dr["PATH"].ToString();
                    string imgid = dr["ID"].ToString();

                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"SELECT ID FROM FIREARMS_PICTURES  WHERE F_ID=@F_ID AND UPPER(DEFPIC)='DEFAULT'";
                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = gid;
                    dr = cmd.ExecuteReader();
                    if (!dr.HasRows)
                    {

                        dr.Dispose();
                        cmd.Dispose();

                        strSQL = @"UPDATE  FIREARMS SET DEFPIC=@DEFPIC WHERE ID=@ID";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("DEFPIC", SqlDbType.VarChar).Value = path;
                        cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = gid;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();

                        strSQL = @"UPDATE  FIREARMS_PICTURES SET DEFPIC='Default' WHERE ID=@ID";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("DEFPIC", SqlDbType.VarChar).Value = path;
                        cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = imgid;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }

                cmd.Dispose();
                dr.Dispose();
                dbconn.closeConnection();


                return "";
            }
            finally
            {
                dbconn.closeConnection();
            }

        }

        public string checkFirearm(string id)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";
                strSQL = @"SELECT PATH FROM FIREARMS_PICTURES WHERE RFID=@RFID ORDER BY ID ASC";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = id;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    strResult ="/"+ dr["PATH"].ToString();
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

        public ActionResult checkout()
        {
            if (Session["message"] == null) Session["message"] = "";
            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {


                //string IP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();

              //  ViewData["test"] = IP;



                return View();
            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        

        public ActionResult  getScanFirearms()
        {
            Db_Connection dbconn = new Db_Connection();
            string IP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
            
            dbconn.openConnection();
            List<mdlRFID> items = new List<mdlRFID>();
            string strSQL = @"SELECT F_ID,RFID,ITEM,DEFPIC FROM [dbo].[vrfidtrans] WHERE IPADD=@IPADD";

            SqlCommand cmd = new SqlCommand(strSQL,dbconn.DbConn);
                    cmd.Parameters.Add("IPADD", SqlDbType.VarChar).Value = IP;
                    SqlDataReader  dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        mdlRFID list = new mdlRFID();
                        list.GID = dr["F_ID"].ToString();
                        list.RFID = dr["RFID"].ToString();
                        list.ITEM = dr["ITEM"].ToString();
                        list.DEFPIC = "/" + dr["DEFPIC"].ToString();
                        items.Add(list);


                    }
                    dr.Dispose();
                    cmd.Dispose();
                    dbconn.closeConnection();
            return PartialView (items);


        }

	}
}
