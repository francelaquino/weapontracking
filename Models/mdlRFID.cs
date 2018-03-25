using System;
using System.Collections.Generic;
using System.Linq;

namespace FMS.Models
{
    public class mdlRFID
    {
        public string ID { get; set; }
        public string GID { get; set; }
        public string ITEM { get; set; }
        
        public string DEFPIC { get; set; }
        public string RFID { get; set; }

        public string IPADD { get; set; }
        public DateTime MESSAGEDATE { get; set; }

    }
}