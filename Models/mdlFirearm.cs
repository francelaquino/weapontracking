using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    public class mdlFirearm
    {
        public string ID { get; set; }
        public string GID { get; set; }
        public string RFID { get; set; }

        public string CALIBER { get; set; }
        public string MODEL { get; set; }
        public string SERIALNO { get; set; }
        public string BARCODE { get; set; }
        public string MANUFACTURER { get; set; }
        public string CONDITION { get; set; }
        public string FINISH { get; set; }
        public string CAPACITY { get; set; }
        public string LENGTH { get; set; }
        public string ADDINFO { get; set; }
        public string STORAGE { get; set; }
        public string RACK { get; set; }
        public string SUPPLIER { get; set; }
        public string FIREARMTYPE { get; set; }
        public string DATE { get; set; }
        
        public string PATH { get; set; }
        public string IMG_ID { get; set; }
        public string FILENAME { get; set; }
        public string DEFPIC { get; set; }
        public string STATUS { get; set; }
        public string NAME { get; set; }
        
        
        public string DATEENCODED { get; set; }
        public string ENCODEDBY { get; set; }
        public string DATEMODIFIED { get; set; }
        public string MODIFIEDBY { get; set; }
    }
}