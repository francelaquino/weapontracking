using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    public class mdlTrack
    {
        public string ID { get; set; }
        public string F_ID { get; set; }
        
        public string X { get; set; }
        public string Y { get; set; }
        public string TAGS { get; set; }
        public string READTIME { get; set; }
        public string LOCATION { get; set; }
        public string MAPIMAGE { get; set; }
        public string MAPTITLE { get; set; }
        public string COUNT { get; set; }
        public string DESCRIPTION { get; set; }
        public string ACCESS { get; set; }
        public string DOOR { get; set; }
        public string LOCATIONID { get; set; }
        public string FIREARMTYPE { get; set; }
        public string DEFPIC { get; set; }
    }
}