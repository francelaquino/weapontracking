using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    public class mdlMapModel
    {
        public string mapwidth { get; set; }
        public string mapheight { get; set; }
        public List<mdlMapCategories> categories { get; set; }
        public List<mdlMapLevels> levels { get; set; }
        
    }

    public class mdlMapCategories
    {
        public string id { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public string show { get; set; }


    }
    public class mdlMapLevels
    {
        public string id { get; set; }
        public string title { get; set; }
        public string map { get; set; }

        public List<mdlMapLocations> locations { get; set; }


    }

    public class mdlMapLocations
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string zoom { get; set; }
        public string pin { get; set; }
        public string link { get; set; }
        public string fill { get; set; }
        public string x { get; set; }
        public string y { get; set; }


    }
}