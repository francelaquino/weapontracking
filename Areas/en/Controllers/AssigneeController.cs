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
    public class AssigneeController : Controller
    {
        //
        // GET: /en/Assignee/
        public ActionResult Index()
        {
            

                return View();
        }


        public ActionResult search_list()
        {

            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT ID,GID,RFID,BATCHNO,NAME,IDNO,MOBILENO,ASSIGNEETYPE,FIREARMS FROM VASSIGNEE";



                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);



                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlAssignee> record = new List<mdlAssignee>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlAssignee list = new mdlAssignee();
                        list.ID = dr["ID"].ToString();
                        list.GID = dr["GID"].ToString();
                        list.RFID = dr["RFID"].ToString();
                        list.BATCHNO = dr["BATCHNO"].ToString();
                        list.MOBILENO = dr["MOBILENO"].ToString();
                        list.NAME = dr["NAME"].ToString();
                        list.IDNO = dr["IDNO"].ToString();
                        list.ASSIGNEETYPE = dr["ASSIGNEETYPE"].ToString();
                        list.FIREARMS = dr["FIREARMS"].ToString();
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
        public ActionResult add()
        {
            if (Session["message"] == null) Session["message"] = "";
            Populate DropDown = new Populate();
            List<SelectListItem> assigneetype_combo = new List<SelectListItem>();
            assigneetype_combo=DropDown.getAssigneeType();


            ViewData["assigneetype"] = assigneetype_combo;


            return View();
        }

       

        public string isRFIDExist(string id, string gid)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";
                strSQL = @"SELECT ID FROM ASSIGNEE WHERE  UPPER(RFID)=UPPER(@RFID)";
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

        public string isAssigned(string id)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strResult = "";
                string strSQL = "";
                id = id.Replace(" ", "");
                string[] firearms = id.Split(',');
                int x = 0;

                SqlDataReader dr;
                SqlCommand cmd;
                foreach (string s in firearms)
                {
                    if (s != "")
                    {
                        strSQL = @"SELECT ID FROM ASSIGN_FIREARMS WHERE F_ID=@F_ID";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            x++;

                        }
                        dr.Dispose();
                        cmd.Dispose();
                    }
                }

                if (x > 0)
                {
                    strResult = "Unable to complete the assignment, "+ x.ToString()+" item(s) already assigned!";
                }
                dbconn.closeConnection();

                return strResult;
            }
            finally
            {
                dbconn.closeConnection();
            }

        }


        public string isAssignedandCheckout(string id,string gid)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strResult = "";
                string strSQL = "";
                gid = gid.Replace(" ", "");
                string[] firearms = gid.Split(',');
                int x = 0;
                int i = 0;

                SqlDataReader dr;
                SqlCommand cmd;
                foreach (string s in firearms)
                {
                    if (s != "")
                    {
                        strSQL = @"SELECT STATUS FROM ASSIGN_FIREARMS WHERE F_ID=@F_ID AND A_ID=@A_ID";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                        cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = id;
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            if (dr["STATUS"].ToString().ToUpper() != "CHECKIN")
                            {
                                x++;
                            }
                            

                        }
                        else
                        {
                            i++;
                        }
                        dr.Dispose();
                        cmd.Dispose();
                    }
                }

                if (x > 0)
                {
                    strResult = x.ToString() + " item(s) already checkout";
                }
                if (i > 0)
                {
                    if (strResult == "")
                    {
                        strResult = strResult + i.ToString() + " item(s) are assigned to someone else";
                    }
                    else
                    {
                        strResult = strResult + " and " + i.ToString() + " item(s) are assigned to someone else";

                    }
                }

                if (strResult != "")
                {
                    strResult="Unable to continue, "+strResult+".";
                }
                dbconn.closeConnection();

                return strResult;
            }
            finally
            {
                dbconn.closeConnection();
            }

        }



        public string isAssignedandCheckin(string id, string gid)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strResult = "";
                string strSQL = "";
                gid = gid.Replace(" ", "");
                string[] firearms = gid.Split(',');
                int x = 0;
                int i = 0;

                SqlDataReader dr;
                SqlCommand cmd;
                foreach (string s in firearms)
                {
                    if (s != "")
                    {
                        strSQL = @"SELECT STATUS FROM ASSIGN_FIREARMS WHERE F_ID=@F_ID AND A_ID=@A_ID";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                        cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = id;
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            if (dr["STATUS"].ToString().ToUpper() != "CHECKOUT")
                            {
                                x++;
                            }


                        }
                        else
                        {
                            i++;
                        }
                        dr.Dispose();
                        cmd.Dispose();
                    }
                }

                if (x > 0)
                {
                    strResult = x.ToString() + " item(s) already checkin";
                }
                if (i > 0)
                {
                    if (strResult == "")
                    {
                        strResult = strResult + i.ToString() + " item(s) are assigned to someone else";
                    }
                    else
                    {
                        strResult = strResult + " and " + i.ToString() + " item(s) are assigned to someone else";

                    }
                }

                if (strResult != "")
                {
                    strResult = "Unable to continue, " + strResult + ".";
                }
                dbconn.closeConnection();

                return strResult;
            }
            finally
            {
                dbconn.closeConnection();
            }

        }

        public string isBatchNoExist(string id, string gid)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";
                strSQL = @"SELECT ID FROM ASSIGNEE WHERE  UPPER(BATCHNO)=UPPER(@BATCHNO)";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("BATCHNO", SqlDbType.VarChar).Value = gid;
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



        [HttpPost]
        public ActionResult saveassignee(IEnumerable<HttpPostedFileBase> files)
        {
            Utility util = new Utility();
            Function func = new Function();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtbatchno = Request.Form["txtbatchno"].ToString();
            string txtname = Request.Form["txtname"].ToString();
            string txtidno = Request.Form["txtidno"].ToString();
            string txtassigneetype = Request.Form["txtassigneetype"].ToString();
            string txtmobileno = Request.Form["txtmobileno"].ToString();
            string txtbuildingno = Request.Form["txtbuildingno"].ToString();
            string txtstreetname = Request.Form["txtstreetname"].ToString();
            string txtdistrict = Request.Form["txtdistrict"].ToString();
            string txtcity = Request.Form["txtcity"].ToString();
            string txtpostal = Request.Form["txtpostal"].ToString();
            string txtaddionalno = Request.Form["txtaddionalno"].ToString();
            string txtrfid = Request.Form["txtrfid"].ToString();

            string gid = "";
            string id = "";
            string image = "";
            try
            {
                gid = util.GenerateId();


                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {

                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(HttpContext.Server.MapPath("~/Content/assignee/") + txtbatchno + extension);
                        image = "Content/assignee/" + txtbatchno + extension;

                       
                    }
                }



                strSQL = @"INSERT INTO ASSIGNEE(GID,BATCHNO,NAME,IDNO,ASSIGNEETYPE,MOBILENO,BUILDINGNO,STREETNAME,DISTRICT,CITY,
                    POSTAL,ADDITIONALNO,PICTURE,DATEENCODED,ENCODEDBY,DATEMODIFIED,MODIFIEDBY,RFID)
                    VALUES(@GID,@BATCHNO,@NAME,@IDNO,@ASSIGNEETYPE,@MOBILENO,@BUILDINGNO,@STREETNAME,@DISTRICT,@CITY,
                    @POSTAL,@ADDITIONALNO,@PICTURE,GETDATE(),@ENCODEDBY,GETDATE(),@MODIFIEDBY,@RFID)";

                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                cmd.Parameters.Add("BATCHNO", SqlDbType.VarChar).Value = txtbatchno;
                cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = txtname;
                cmd.Parameters.Add("IDNO", SqlDbType.VarChar).Value = txtidno;
                cmd.Parameters.Add("ASSIGNEETYPE", SqlDbType.VarChar).Value = txtassigneetype;
                cmd.Parameters.Add("MOBILENO", SqlDbType.VarChar).Value = txtmobileno;
                cmd.Parameters.Add("BUILDINGNO", SqlDbType.VarChar).Value = txtbatchno;
                cmd.Parameters.Add("STREETNAME", SqlDbType.VarChar).Value = txtstreetname;
                cmd.Parameters.Add("DISTRICT", SqlDbType.VarChar).Value = txtdistrict;
                cmd.Parameters.Add("CITY", SqlDbType.VarChar).Value = txtcity;
                cmd.Parameters.Add("POSTAL", SqlDbType.VarChar).Value= txtpostal;
                cmd.Parameters.Add("ADDITIONALNO", SqlDbType.VarChar).Value =txtaddionalno;
                cmd.Parameters.Add("PICTURE", SqlDbType.VarChar).Value = image;
                cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = txtrfid;
                
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                

                dbconn.closeConnection();




                Session["message"] = "Assignee successfully saved...";

                return RedirectToAction("add");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }

        public ActionResult edit(string id, string gid)
        {
            if (Session["message"] == null) Session["message"] = "";
            Populate DropDown = new Populate();
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> assigneetype_combo = new List<SelectListItem>();
            assigneetype_combo = DropDown.getAssigneeType();

            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT ID,GID,BATCHNO,NAME,IDNO,RFID,ASSIGNEETYPE,MOBILENO,BUILDINGNO,STREETNAME,DISTRICT,
                CITY,POSTAL,ADDITIONALNO,PICTURE,RFID
                FROM ASSIGNEE WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlAssignee record = new mdlAssignee();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.BATCHNO = dr["BATCHNO"].ToString();
                    record.NAME = dr["NAME"].ToString();
                    record.IDNO = dr["IDNO"].ToString();
                    record.RFID = dr["RFID"].ToString();
                    record.ASSIGNEETYPE = dr["ASSIGNEETYPE"].ToString();
                    record.MOBILENO = dr["MOBILENO"].ToString();
                    record.BUILDINGNO = dr["BUILDINGNO"].ToString();
                    record.STREETNAME = dr["STREETNAME"].ToString();
                    record.DISTRICT = dr["DISTRICT"].ToString();
                    record.CITY = dr["CITY"].ToString();
                    record.POSTAL = dr["POSTAL"].ToString();
                    record.ADDITIONALNO = dr["ADDITIONALNO"].ToString();
                    record.PICTURE = "";
                    if (!string.IsNullOrEmpty(dr["PICTURE"].ToString()))
                    {
                        record.PICTURE ="/"+ dr["PICTURE"].ToString();
                    }

                }


                foreach (SelectListItem item in assigneetype_combo)
                {
                    if (item.Value == record.ASSIGNEETYPE)
                    {
                        item.Selected = true;
                    }
                }




                ViewData["assigneetype"] = assigneetype_combo;
                return View(record);

            }
            finally
            {
                dbconn.closeConnection();
            }






        }

        [HttpPost]
        public ActionResult updateassignee(IEnumerable<HttpPostedFileBase> files)
        {
            Utility util = new Utility();
            Function func = new Function();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtbatchno = Request.Form["txtbatchno"].ToString();
            string txtname = Request.Form["txtname"].ToString();
            string txtidno = Request.Form["txtidno"].ToString();
            string txtassigneetype = Request.Form["txtassigneetype"].ToString();
            string txtmobileno = Request.Form["txtmobileno"].ToString();
            string txtbuildingno = Request.Form["txtbuildingno"].ToString();
            string txtstreetname = Request.Form["txtstreetname"].ToString();
            string txtdistrict = Request.Form["txtdistrict"].ToString();
            string txtcity = Request.Form["txtcity"].ToString();
            string txtpostal = Request.Form["txtpostal"].ToString();
            string txtaddionalno = Request.Form["txtaddionalno"].ToString();
            string txtid = Request.Form["txtid"].ToString();
            string txtgid = Request.Form["txtgid"].ToString();
            string txtrfid = Request.Form["txtrfid"].ToString();
            string oldPicture = "";
            int x = 1;
            string image = "";
            try
            {

                strSQL = @"SELECT PICTURE FROM ASSIGNEE 
                    WHERE ID=@ID AND GID=@GID";

                cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = txtid;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = txtgid;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    oldPicture = dr[0].ToString();
                }



                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {

                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(HttpContext.Server.MapPath("~/Content/assignee/") + txtbatchno + extension);
                        image = "Content/assignee/" + txtbatchno + extension;


                    }
                }

                if (image == "")
                {
                    image = oldPicture;
                }

                dr.Dispose();
                cmd.Dispose();
                strSQL = @"UPDATE ASSIGNEE SET BATCHNO=@BATCHNO,NAME=@NAME,IDNO=@IDNO,ASSIGNEETYPE=@ASSIGNEETYPE,
                    MOBILENO=@MOBILENO,BUILDINGNO=@BUILDINGNO,STREETNAME=@STREETNAME,DISTRICT=@DISTRICT,CITY=@CITY,
                    POSTAL=@POSTAL,ADDITIONALNO=@ADDITIONALNO,PICTURE=@PICTURE,DATEMODIFIED=GETDATE(),MODIFIEDBY=@MODIFIEDBY,RFID=@RFID
                    WHERE ID=@ID AND GID=@GID";

                cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("BATCHNO", SqlDbType.VarChar).Value = txtbatchno;
                cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = txtname;
                cmd.Parameters.Add("IDNO", SqlDbType.VarChar).Value = txtidno;
                cmd.Parameters.Add("ASSIGNEETYPE", SqlDbType.VarChar).Value = txtassigneetype;
                cmd.Parameters.Add("MOBILENO", SqlDbType.VarChar).Value = txtmobileno;
                cmd.Parameters.Add("BUILDINGNO", SqlDbType.VarChar).Value = txtbatchno;
                cmd.Parameters.Add("STREETNAME", SqlDbType.VarChar).Value = txtstreetname;
                cmd.Parameters.Add("DISTRICT", SqlDbType.VarChar).Value = txtdistrict;
                cmd.Parameters.Add("CITY", SqlDbType.VarChar).Value = txtcity;
                cmd.Parameters.Add("POSTAL", SqlDbType.VarChar).Value = txtpostal;
                cmd.Parameters.Add("ADDITIONALNO", SqlDbType.VarChar).Value = txtaddionalno;
                cmd.Parameters.Add("PICTURE", SqlDbType.VarChar).Value = image;
                cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = txtrfid;
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = txtid;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = txtgid;

                cmd.ExecuteNonQuery();
                cmd.Dispose();



                dbconn.closeConnection();




                Session["message"] = "Assignee successfully updated...";

                return RedirectToAction("edit/" + txtid + "/" + txtgid);


            }
            finally
            {
                dbconn.closeConnection();
            }


        }

        public string removePicture(string id)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string picture = "";
                
                strSQL = @"SELECT PICTURE FROM ASSIGNEE  WHERE ID=@ID";
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


                strSQL = @"UPDATE ASSIGNEE SET PICTURE='' WHERE ID=@ID";
                 cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.ExecuteNonQuery();
                cmd.Dispose();


               

                dbconn.closeConnection();


                if (System.IO.File.Exists(HttpContext.Server.MapPath("~/"+picture)))
                {
                    System.IO.File.Delete(HttpContext.Server.MapPath("~/"+picture));
                }

                return "";
            }
            finally
            {
                dbconn.closeConnection();
            }

        }


        public ActionResult assignfirearm()
        {
            if (Session["message"] == null) Session["message"] = "";
            Utility util = new Utility();
            try
            {

                return View();
            }
            finally
            {
            }
        }


        public ActionResult checkinfirearm()
        {
            if (Session["message"] == null) Session["message"] = "";
            Utility util = new Utility();
            try
            {

                return View();
            }
            finally
            {
            }
        }

        public ActionResult checkoutfirearm()
        {
            if (Session["message"] == null) Session["message"] = "";
            Utility util = new Utility();
            try
            {

                return View();
            }
            finally
            {
            }
        }


         [HttpPost]
        public ActionResult getAssignee(string id)
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                string strResult = "";
                strSQL = @"SELECT PICTURE,NAME,ID,ASSIGNEETYPE,FIREARMS FROM VASSIGNEE WHERE  UPPER(BATCHNO)=UPPER(@BATCHNO)";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("BATCHNO", SqlDbType.VarChar).Value = id;
                SqlDataReader dr = cmd.ExecuteReader();

                mdlAssignee Assignee = new mdlAssignee();

                strResult = "/Content/assignee/NoImage.png";
                Assignee.PICTURE = strResult;
                if (dr.HasRows)
                {
                    dr.Read();
                    if (string.IsNullOrEmpty(dr["PICTURE"].ToString()))
                    {
                        
                    }
                    else
                    {
                        strResult ="/"+ dr["PICTURE"].ToString();
                    }

                    Assignee.PICTURE = strResult;
                    Assignee.ID = dr["ID"].ToString();
                    Assignee.NAME = dr["NAME"].ToString();
                    Assignee.ASSIGNEETYPE = dr["ASSIGNEETYPE"].ToString();
                    Assignee.FIREARMS = dr["FIREARMS"].ToString();

                }
                dr.Dispose();
                cmd.Dispose();

                dbconn.closeConnection();

                return Json(Assignee);
            }
            finally
            {
                dbconn.closeConnection();
            }

        }


        public string reScan()
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {
                string IP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();

                dbconn.openConnection();
                string strSQL = "";

                strSQL = @"DELETE FROM RFIDTRANS WHERE IPADD=@IPADD";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("IPADD", SqlDbType.VarChar).Value = IP;
                SqlDataReader dr = cmd.ExecuteReader();
               
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


        [HttpPost]
        public ActionResult saveassignfirearms()
        {
            Utility util = new Utility();
            Function func = new Function();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            Boolean con = true;
            dbconn.openConnection();
            string txtassign_id = Request.Form["txtassign_id"].ToString();
            string txtbatchno = Request.Form["txtbatchno"].ToString();
            string txtfirearms = Request.Form["txtfirearms"].ToString();
            string strResult = "";
            txtfirearms = txtfirearms.Replace(" ", "");
            int x = 0;
            try
            {

                string[] firearms = txtfirearms.Split(',');
                SqlDataReader dr;

                foreach (string s in firearms)
                {
                    if (s != "")
                    {
                         strSQL = @"SELECT ID FROM ASSIGN_FIREARMS WHERE F_ID=@F_ID";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            con = false;
                            strResult = "";
                            x++;

                        }
                        dr.Dispose();
                        cmd.Dispose();
                    }
                }
                cmd.Dispose();
                if (con == true)
                {
                    foreach (string s in firearms)
                    {
                        if (s != "")
                        {



                            strSQL = @"INSERT INTO ASSIGN_FIREARMS(F_ID,A_ID,BATCHNO,DATEENCODED,ENCODEDBY,STATUS)
                            VALUES(@F_ID,@A_ID,@BATCHNO,GETDATE(),@ENCODEDBY,@STATUS)";
                            cmd = new SqlCommand(strSQL, dbconn.DbConn);

                            cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                            cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = txtassign_id;
                            cmd.Parameters.Add("BATCHNO", SqlDbType.VarChar).Value = txtbatchno;
                            cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                            cmd.Parameters.Add("STATUS", SqlDbType.VarChar).Value = "Checkin";
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    strResult = "Firearm(s) successfully assigned...";
                    cmd.Dispose();
                    reScan();
                }
                else
                {
                    strResult = "Unable to complete the assignment, " + x.ToString() + "item(s) already assigned!";

                }



                cmd.Dispose();
                dbconn.closeConnection();

              


                Session["message"] = strResult;

                return RedirectToAction("assignfirearm");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }


        [HttpPost]
        public ActionResult savecheckoutfirearms()
        {
            Utility util = new Utility();
            Function func = new Function();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtassign_id = Request.Form["txtassign_id"].ToString();
            string txtbatchno = Request.Form["txtbatchno"].ToString();
            string txtfirearms = Request.Form["txtfirearms"].ToString();
            string strResult = "";
            txtfirearms = txtfirearms.Replace(" ", "");
            int x = 0;
            try
            {

                string[] firearms = txtfirearms.Split(',');
               
                    foreach (string s in firearms)
                    {
                        if (s != "")
                        {



                            strSQL = @"INSERT INTO FIREARMS_INOUT(F_ID,A_ID,BATCHNO,DATE,STATUS,ENCODEDBY)
                            VALUES(@F_ID,@A_ID,@BATCHNO,GETDATE(),@STATUS,@ENCODEDBY)";
                            cmd = new SqlCommand(strSQL, dbconn.DbConn);

                            cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                            cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = txtassign_id;
                            cmd.Parameters.Add("BATCHNO", SqlDbType.VarChar).Value = txtbatchno;
                            cmd.Parameters.Add("STATUS", SqlDbType.VarChar).Value = "Checkout";
                            cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();


                            strSQL = @"UPDATE ASSIGN_FIREARMS SET STATUS=@STATUS WHERE F_ID=@F_ID AND A_ID=@A_ID";
                            cmd = new SqlCommand(strSQL, dbconn.DbConn);
                            cmd.Parameters.Add("STATUS", SqlDbType.VarChar).Value = "Checkout";
                            cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                            cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = txtassign_id;
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }

                    strResult = "Firearm(s) successfully checkout...";
                    cmd.Dispose();
                    reScan();
               



                cmd.Dispose();
                dbconn.closeConnection();




                Session["message"] = strResult;

                return RedirectToAction("checkoutfirearm");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }

        public ActionResult savecheckinfirearms()
        {
            Utility util = new Utility();
            Function func = new Function();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtassign_id = Request.Form["txtassign_id"].ToString();
            string txtbatchno = Request.Form["txtbatchno"].ToString();
            string txtfirearms = Request.Form["txtfirearms"].ToString();
            string strResult = "";
            txtfirearms = txtfirearms.Replace(" ", "");
            int x = 0;
            try
            {

                string[] firearms = txtfirearms.Split(',');

                foreach (string s in firearms)
                {
                    if (s != "")
                    {



                        strSQL = @"INSERT INTO FIREARMS_INOUT(F_ID,A_ID,BATCHNO,DATE,STATUS,ENCODEDBY)
                            VALUES(@F_ID,@A_ID,@BATCHNO,GETDATE(),@STATUS,@ENCODEDBY)";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);

                        cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                        cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = txtassign_id;
                        cmd.Parameters.Add("BATCHNO", SqlDbType.VarChar).Value = txtbatchno;
                        cmd.Parameters.Add("STATUS", SqlDbType.VarChar).Value = "Checkin";
                        cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();


                        strSQL = @"UPDATE ASSIGN_FIREARMS SET STATUS=@STATUS WHERE F_ID=@F_ID AND A_ID=@A_ID";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("STATUS", SqlDbType.VarChar).Value = "Checkin";
                        cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                        cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = txtassign_id;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }

                strResult = "Firearm(s) successfully checkin...";
                cmd.Dispose();
                reScan();




                cmd.Dispose();
                dbconn.closeConnection();




                Session["message"] = strResult;

                return RedirectToAction("checkinfirearm");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }



        [HttpPost]
        public ActionResult removeassignfirearms()
        {
            Utility util = new Utility();
            Function func = new Function();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtassign_id = Request.Form["txtassign_id"].ToString();
            string txtassign_gid = Request.Form["txtassign_gid"].ToString();
            string txtfirearms = Request.Form["txtfirearms"].ToString();
            string strResult = "";
            txtfirearms = txtfirearms.Replace(" ", "");
            try
            {

                string[] firearms = txtfirearms.Split(',');

                foreach (string s in firearms)
                {
                    if (s != "")
                    {
                        strSQL = @"DELETE FROM ASSIGN_FIREARMS WHERE A_ID=@A_ID AND F_ID=@F_ID";
                        cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = txtassign_id;
                        cmd.Parameters.Add("F_ID", SqlDbType.VarChar).Value = s;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                
                    strResult = "Assign firearm(s) successfully removed...";

                cmd.Dispose();
                dbconn.closeConnection();




                Session["message"] = strResult;

                return RedirectToAction("removeassignment/" + txtassign_id + "/" + txtassign_gid);


            }
            finally
            {
                dbconn.closeConnection();
            }


        }



        public ActionResult removefirearm(string id, string gid)
        {
            if (Session["message"] == null) Session["message"] = "";
            Populate DropDown = new Populate();
            Db_Connection dbconn = new Db_Connection();

            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT ID,GID,BATCHNO,NAME,PICTURE,ASSIGNEETYPE,FIREARMS
                FROM VASSIGNEE WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                ViewData["ID"] = id;
                ViewData["GID"]= gid;
                ViewData["PICTURE"] = "/Content/assignee/NoImage.png";
                if (dr.HasRows)
                {
                    dr.Read();
                    ViewData["BATCHNO"] = dr["BATCHNO"].ToString();
                    ViewData["FIREARMS"] = dr["FIREARMS"].ToString();
                    ViewData["ASSIGNEETYPE"] = dr["ASSIGNEETYPE"].ToString();
                    ViewData["NAME"] = dr["NAME"].ToString();
                    ViewData["PICTURE"] = "/Content/assignee/NoImage.png";
                    if (!string.IsNullOrEmpty(dr["PICTURE"].ToString()))
                    {
                        ViewData["PICTURE"] = "/" + dr["PICTURE"].ToString();
                    }

                }



                dr.Dispose();
                cmd.Dispose();
                
                strSQL = @"SELECT ID,F_ID,DEFPIC,FIREARMTYPE,STATUS FROM VASSIGN_FIREARMS WHERE A_ID=@A_ID";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = id;
                dr = cmd.ExecuteReader();
                List<mdlFirearm> record = new List<mdlFirearm>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlFirearm list = new mdlFirearm();

                        list.ID = dr["ID"].ToString();
                        list.GID = dr["F_ID"].ToString();
                        list.STATUS = dr["STATUS"].ToString();
                        list.FIREARMTYPE = dr["FIREARMTYPE"].ToString();
                        if (!string.IsNullOrEmpty(dr["DEFPIC"].ToString()))
                        {
                            list.DEFPIC = "/" + dr["DEFPIC"].ToString();
                        }
                        else
                        {
                            list.DEFPIC = "/Content/assignee/NoImage.png";
                        }

                        record.Add(list);
                    }
                }
                dr.Dispose();
                dbconn.openConnection();

                return View(record);

            }
            finally
            {
                dbconn.closeConnection();
            }






        }

        public ActionResult profile(string id,string gid)
        {
            Populate DropDown = new Populate();
            Db_Connection dbconn = new Db_Connection();

            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT ID,GID,BATCHNO,NAME,IDNO,RFID,ASSIGNEETYPE,MOBILENO,PICTURE,RFID,FIREARMS,CHECKINS,CHECKOUTS
                FROM VASSIGNEE WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlAssignee record = new mdlAssignee();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.BATCHNO = dr["BATCHNO"].ToString();
                    record.NAME = dr["NAME"].ToString();
                    record.IDNO = dr["IDNO"].ToString();
                    record.RFID = dr["RFID"].ToString();
                    record.FIREARMS = dr["FIREARMS"].ToString();
                    record.CHECKINS = dr["CHECKINS"].ToString();
                    record.CHECKOUTS = dr["CHECKOUTS"].ToString();
                    record.ASSIGNEETYPE = dr["ASSIGNEETYPE"].ToString();
                    record.MOBILENO = dr["MOBILENO"].ToString();
                    record.PICTURE = "";
                    if (!string.IsNullOrEmpty(dr["PICTURE"].ToString()))
                    {
                        record.PICTURE = "/" + dr["PICTURE"].ToString();
                    }

                }


               
                return View(record);

            }
            finally
            {
                dbconn.closeConnection();
            }


        }


        public ActionResult profile_firearms(string id, string gid)
        {
            Populate DropDown = new Populate();
            Db_Connection dbconn = new Db_Connection();

            try
            {
                dbconn.openConnection();



                string strSQL = @"SELECT ID,GID,BATCHNO,NAME,IDNO,RFID,ASSIGNEETYPE,MOBILENO,PICTURE,RFID,FIREARMS,CHECKINS,CHECKOUTS
                FROM VASSIGNEE WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlAssignee record = new mdlAssignee();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.BATCHNO = dr["BATCHNO"].ToString();
                    record.NAME = dr["NAME"].ToString();
                    record.IDNO = dr["IDNO"].ToString();
                    record.RFID = dr["RFID"].ToString();
                    record.FIREARMS = dr["FIREARMS"].ToString();
                    record.CHECKINS = dr["CHECKINS"].ToString();
                    record.CHECKOUTS = dr["CHECKOUTS"].ToString();
                    record.ASSIGNEETYPE = dr["ASSIGNEETYPE"].ToString();
                    record.MOBILENO = dr["MOBILENO"].ToString();
                    record.PICTURE = "";
                    if (!string.IsNullOrEmpty(dr["PICTURE"].ToString()))
                    {
                        record.PICTURE = "/" + dr["PICTURE"].ToString();
                    }

                }



                return View(record);

            }
            finally
            {
                dbconn.closeConnection();
            }


        }

        public ActionResult profile_inout(string id, string gid)
        {
            Populate DropDown = new Populate();
            Db_Connection dbconn = new Db_Connection();

            try
            {
                dbconn.openConnection();

                ViewData["date"] = DateTime.Now.ToString("dd-MMM-yy");


                string strSQL = @"SELECT ID,GID,BATCHNO,NAME,IDNO,RFID,ASSIGNEETYPE,MOBILENO,PICTURE,RFID,FIREARMS,CHECKINS,CHECKOUTS
                FROM VASSIGNEE WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlAssignee record = new mdlAssignee();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.BATCHNO = dr["BATCHNO"].ToString();
                    record.NAME = dr["NAME"].ToString();
                    record.IDNO = dr["IDNO"].ToString();
                    record.RFID = dr["RFID"].ToString();
                    record.FIREARMS = dr["FIREARMS"].ToString();
                    record.CHECKINS = dr["CHECKINS"].ToString();
                    record.CHECKOUTS = dr["CHECKOUTS"].ToString();
                    record.ASSIGNEETYPE = dr["ASSIGNEETYPE"].ToString();
                    record.MOBILENO = dr["MOBILENO"].ToString();
                    record.PICTURE = "";
                    if (!string.IsNullOrEmpty(dr["PICTURE"].ToString()))
                    {
                        record.PICTURE = "/" + dr["PICTURE"].ToString();
                    }

                }



                return View(record);

            }
            finally
            {
                dbconn.closeConnection();
            }


        }

        public ActionResult profile_firearms_list(string id)
        {

            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT DEFPIC,FIREARMTYPE,STATUS FROM VASSIGN_FIREARMS WHERE A_ID=@A_ID";



                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = id;


                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlFirearm> record = new List<mdlFirearm>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlFirearm list = new mdlFirearm();
                        list.FIREARMTYPE = dr["FIREARMTYPE"].ToString();
                        list.STATUS = dr["STATUS"].ToString();
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


        public ActionResult profile_inout_list(string id,string date)
        {

            Db_Connection dbconn = new Db_Connection();
            Utility util = new Utility();
            try
            {
                dbconn.openConnection();
                string strSQL = "";
                strSQL = @"SELECT DATE,DEFPIC,FIREARMTYPE,STATUS FROM VFIREARMS_INOUT WHERE A_ID=@A_ID AND CAST(DATE AS DATE)=CAST(@DATE AS DATE)";



                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("A_ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("DATE", SqlDbType.VarChar).Value = date;


                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlFirearm> record = new List<mdlFirearm>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlFirearm list = new mdlFirearm();
                        list.FIREARMTYPE = dr["FIREARMTYPE"].ToString();
                        list.STATUS = dr["STATUS"].ToString();
                        list.DATE = Convert.ToDateTime(dr["DATE"].ToString()).ToShortTimeString();
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

