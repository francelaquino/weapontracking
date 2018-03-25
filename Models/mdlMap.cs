using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    public class mdlMap
    {
        public string ID { get; set; }
        public string GID { get; set; }
        public string MAP { get; set; }
        public string DESCRIPTION { get; set; }
        public string IMAGE { get; set; }
        public string LOCATIONS { get; set; }
        
        public string DATEENCODED { get; set; }
        public string ENCODEDBY { get; set; }
        public string DATEMODIFIED { get; set; }
        public string MODIFIEDBY { get; set; }
    }
}