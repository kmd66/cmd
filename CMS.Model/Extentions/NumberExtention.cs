using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public static class NumberExtention
    {
        public static long StringToLong(this string s)
        {
            try
            {
                return long.Parse(s);
            }
            catch
            {
                return 0;
            }
        }
    }
}
