using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    public class mdlReaders
    {
        public string ID { get; set; }
        public string GID { get; set; }
        public string READERMODEL { get; set; }
        public string READERNAME { get; set; }
        public string PORT { get; set; }
        public string IP { get; set; }
        public string TIME { get; set; }
        public string ALARMNAME { get; set; }
        public string DOOR { get; set; }
        public string LOCATION { get; set; }
        public string LOCATIONID { get; set; }
        public string ENCODEDBY { get; set; }
        public string DATEENCODED { get; set; }
        public string MODIFIEDBY { get; set; }
        public string DATEMODIFIED { get; set; }
    }
}