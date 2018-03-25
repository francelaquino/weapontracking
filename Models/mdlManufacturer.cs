using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    public class mdlManufacturer
    {
        public string ID { get; set; }
        public string GID { get; set; }
        public string MANUFACTURER { get; set; }
        public string DATEENCODED { get; set; }
        public string ENCODEDBY { get; set; }
        public string DATEMODIFIED { get; set; }
        public string MODIFIEDBY { get; set; }
    }
}