﻿using System;
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
    public class ConditionController : Controller
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
                strSQL = @"SELECT ID,GID,CONDITION FROM CONDITION ORDER BY CONDITION ASC";


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                List<mdlCondition> record = new List<mdlCondition>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mdlCondition list = new mdlCondition();
                        list.ID = dr["ID"].ToString();
                        list.GID = dr["GID"].ToString();
                        list.CONDITION = dr["CONDITION"].ToString();
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



                string strSQL = @"SELECT CONDITION FROM CONDITION WHERE ID=@ID AND GID=@GID";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                SqlDataReader dr = cmd.ExecuteReader();
                mdlCondition record = new mdlCondition();
                record.ID = id;
                record.GID = gid;
                if (dr.HasRows)
                {
                    dr.Read();
                    record.ID = id;
                    record.GID = gid;
                    record.CONDITION = dr["CONDITION"].ToString();

                }

            
                return PartialView(record);

            }
            finally
            {
                dbconn.closeConnection();
            }

        }


        [HttpPost]
        public ActionResult savecondition()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtcondition = Request.Form["txtcondition"].ToString();
            string gid = "";
            try
            {

                gid = util.GenerateId();

                strSQL = @"INSERT INTO CONDITION(GID,CONDITION,DATEENCODED,ENCODEDBY,DATEMODIFIED,MODIFIEDBY)
                    VALUES(@GID,@CONDITION,GETDATE(),@ENCODEDBY,GETDATE(),@MODIFIEDBY)";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;
                cmd.Parameters.Add("CONDITION", SqlDbType.VarChar).Value = txtcondition;
                cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbconn.closeConnection();
                Session["message"] = "Condition type successfully saved...";

                return RedirectToAction("add");


            }
            finally
            {
                dbconn.closeConnection();
            }


        }


        [HttpPost]
        public ActionResult updatecondition()
        {
            Utility util = new Utility();
            Db_Connection dbconn = new Db_Connection();
            SqlCommand cmd = new SqlCommand();
            string strSQL = "";
            dbconn.openConnection();
            string txtid = Request.Form["txtid"].ToString();
            string txtgid = Request.Form["txtgid"].ToString();
            string txtcondition = Request.Form["txtcondition"].ToString();
            try
            {


                strSQL = @"UPDATE CONDITION SET CONDITION=@CONDITION,DATEMODIFIED=GETDATE(),MODIFIEDBY=@MODIFIEDBY 
                WHERE ID=@ID AND GID=@GID";
                cmd = new SqlCommand(strSQL, dbconn.DbConn);

                cmd.Parameters.Add("CONDITION", SqlDbType.VarChar).Value = txtcondition;
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
                strSQL = @"SELECT ID FROM CONDITION WHERE  UPPER(CONDITION)=UPPER(@CONDITION)";
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("CONDITION", SqlDbType.VarChar).Value = gid;
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

                //strSQL = @"SELECT ID FROM ASSETS WHERE  CONDITION=@CONDITION";
                //SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                //cmd.Parameters.Add("CONDITION", SqlDbType.VarChar).Value = id;
                //SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows)
                //{
                //    strResult = "Found";
                //}
                //dr.Dispose();
                //cmd.Dispose();




                //if (strResult == "")
                //{
                //    strSQL = @"SELECT ID FROM LINENS WHERE  CONDITION=@CONDITION";
                //    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                //    cmd.Parameters.Add("CONDITION", SqlDbType.VarChar).Value = id;
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

                    strSQL = @"DELETE FROM CONDITION WHERE ID=@ID";
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
