using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using Db_Connections;
using System.Data.SqlClient;
using System.Data;

namespace PopulateDropDown
{
    public class Populate
    {
        public List<SelectListItem> getFirearmType()
        {
            string strSQL = "SELECT ID,FIREARMTYPE FROM FIREARMTYPE ORDER BY FIREARMTYPE ASC";
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = ""
                });
                while (dr.Read())
                {
                    dropdown_combo.Add(new SelectListItem
                    {
                        Text = dr[1].ToString(),
                        Value = dr[0].ToString()

                    });

                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return dropdown_combo;

            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public List<SelectListItem> getAllowDeny()
        {
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            try
            {

                dropdown_combo.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = ""
                });
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "Allow",
                    Value = "Allow"
                });
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "Deny",
                    Value = "Deny"
                });
                

                return dropdown_combo;

            }
            finally
            {
            }
        }

        


        public List<SelectListItem> getAssigneeType()
        {
            string strSQL = "SELECT ID,ASSIGNEETYPE FROM ASSIGNEETYPE ORDER BY ASSIGNEETYPE ASC";
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = ""
                });
                while (dr.Read())
                {
                    dropdown_combo.Add(new SelectListItem
                    {
                        Text = dr[1].ToString(),
                        Value = dr[0].ToString()

                    });

                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return dropdown_combo;

            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public List<SelectListItem> getStorage()
        {
            string strSQL = "SELECT ID,STORAGE FROM STORAGE ORDER BY STORAGE ASC";
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = ""
                });
                while (dr.Read())
                {
                    dropdown_combo.Add(new SelectListItem
                    {
                        Text = dr[1].ToString(),
                        Value = dr[0].ToString()

                    });

                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return dropdown_combo;

            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public List<SelectListItem> getManufacturer()
        {
            string strSQL = "SELECT ID,MANUFACTURER FROM MANUFACTURER ORDER BY MANUFACTURER ASC";
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = ""
                });
                while (dr.Read())
                {
                    dropdown_combo.Add(new SelectListItem
                    {
                        Text = dr[1].ToString(),
                        Value = dr[0].ToString()

                    });

                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return dropdown_combo;

            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public List<SelectListItem> getCondition()
        {
            string strSQL = "SELECT ID,CONDITION FROM CONDITION ORDER BY CONDITION ASC";
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = ""
                });
                while (dr.Read())
                {
                    dropdown_combo.Add(new SelectListItem
                    {
                        Text = dr[1].ToString(),
                        Value = dr[0].ToString()

                    });

                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return dropdown_combo;

            }
            finally
            {
                dbconn.closeConnection();
            }
        }


        public List<SelectListItem> getLocation()
        {
            string strSQL = "SELECT ID,LOCATION FROM LOCATION ORDER BY LOCATION ASC";
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = ""
                });
                while (dr.Read())
                {
                    dropdown_combo.Add(new SelectListItem
                    {
                        Text = dr[1].ToString(),
                        Value = dr[0].ToString()

                    });

                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return dropdown_combo;

            }
            finally
            {
                dbconn.closeConnection();
            }
        }


        public List<SelectListItem> getDoor()
        {
            string strSQL = "SELECT ID,DOOR FROM DOOR ORDER BY DOOR ASC";
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = ""
                });
                while (dr.Read())
                {
                    dropdown_combo.Add(new SelectListItem
                    {
                        Text = dr[1].ToString(),
                        Value = dr[0].ToString()

                    });

                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return dropdown_combo;

            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public List<SelectListItem> getUnAssignedLocation()
        {
            string strSQL = "SELECT ID,LOCATION FROM LOCATION WHERE ID NOT IN (SELECT LOCATION FROM MAP_LOCATION) ORDER BY LOCATION ASC";
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = ""
                });
                while (dr.Read())
                {
                    dropdown_combo.Add(new SelectListItem
                    {
                        Text = dr[1].ToString(),
                        Value = dr[0].ToString()

                    });

                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return dropdown_combo;

            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public List<SelectListItem> getSupplier()
        {
            string strSQL = "SELECT ID,SUPPLIER FROM SUPPLIER ORDER BY SUPPLIER ASC";
            Db_Connection dbconn = new Db_Connection();
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            try
            {

                dbconn.openConnection();
                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();
                dropdown_combo.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = ""
                });
                while (dr.Read())
                {
                    dropdown_combo.Add(new SelectListItem
                    {
                        Text = dr[1].ToString(),
                        Value = dr[0].ToString()

                    });

                }
                dr.Dispose();
                cmd.Dispose();
                dbconn.closeConnection();

                return dropdown_combo;

            }
            finally
            {
                dbconn.closeConnection();
            }
        }

        public List<SelectListItem> getStatusEN()
        {
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();

            dropdown_combo.Add(new SelectListItem
            {
                Text = "--Select--",
                Value = ""
            });
            dropdown_combo.Add(new SelectListItem
            {
                Text = "Checkin",
                Value = "Checkin"
            });
            dropdown_combo.Add(new SelectListItem
            {
                Text = "Checkout",
                Value = "Checkout"
            });


            return dropdown_combo;

        }

        public List<SelectListItem> getTimeInterval()
        {
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            dropdown_combo.Add(new SelectListItem { Text = "--Select--", Value = "" });
            dropdown_combo.Add(new SelectListItem { Text = "1", Value = "1" });
            dropdown_combo.Add(new SelectListItem { Text = "2", Value = "2" });
            dropdown_combo.Add(new SelectListItem { Text = "3", Value = "3" });
            dropdown_combo.Add(new SelectListItem { Text = "4", Value = "4" });
            dropdown_combo.Add(new SelectListItem { Text = "5", Value = "5" });
            dropdown_combo.Add(new SelectListItem { Text = "6", Value = "6" });
            dropdown_combo.Add(new SelectListItem { Text = "7", Value = "7" });
            dropdown_combo.Add(new SelectListItem { Text = "8", Value = "8" });
            dropdown_combo.Add(new SelectListItem { Text = "9", Value = "9" });
            dropdown_combo.Add(new SelectListItem { Text = "10", Value = "10" });
            return dropdown_combo;
        }

        public List<SelectListItem> getReaderModel()
        {
            List<SelectListItem> dropdown_combo = new List<SelectListItem>();
            dropdown_combo.Add(new SelectListItem { Text = "--Select--", Value = "" });
            dropdown_combo.Add(new SelectListItem { Text = "UHF RFID Reader", Value = "UHF RFID Reader" });
            return dropdown_combo;
        }
        
    }
}