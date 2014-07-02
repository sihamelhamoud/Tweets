using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweets
{
    public class GlobalCode
    {
        public int field2int(string field)
        {
            int varint = 0;
            try
            {
                varint = Convert.ToInt16(field);
            }
            catch { varint = 0; }
            return varint;
        }

        public string field2Str(string field)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(field) || field == "" || field == null)
                    return "";
            }
            catch {  }
            return field;
        }

    }
}