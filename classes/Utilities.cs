using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Utilities
{
    public class Utility
    {
        public string GenerateId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public string formatDate(DateTime Date)
        {
            return Date.ToString("dd-MMM-yy");
        }

        public string formatDateTime(DateTime Date)
        {
            return Date.ToString("dd-MMM-yy hh:mm tt");
        }

        public string formatLongDateTime(DateTime Date)
        {
            return Date.ToString("ddd, MMM dd, yyyy H:mm:ss tt");
        }

        public string formatNumner(string Value)
        {
            return Convert.ToDecimal(Value).ToString("#0.00");
        }
    }
}