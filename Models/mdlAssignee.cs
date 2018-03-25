using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    public class mdlAssignee
    {
        public string ID { get; set; }
        public string GID { get; set; }
        public string BATCHNO { get; set; }
        public string NAME { get; set; }
        public string IDNO { get; set; }
        public string ASSIGNEETYPE { get; set; }
        public string RFID { get; set; }
        public string MOBILENO { get; set; }
        public string BUILDINGNO { get; set; }
        public string STREETNAME { get; set; }
        public string DISTRICT { get; set; }
        public string CITY { get; set; }
        public string POSTAL { get; set; }
        public string ADDITIONALNO { get; set; }
        public string PICTURE { get; set; }
        public string FIREARMS { get; set; }
        public string CHECKOUTS { get; set; }

        public string CHECKINS { get; set; }
        
        public string DATEENCODED { get; set; }
        public string ENCODEDBY { get; set; }
        public string DATEMODIFIED { get; set; }
        public string MODIFIEDBY { get; set; }
    }
}

