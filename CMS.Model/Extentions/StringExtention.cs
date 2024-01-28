using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public static class StringExtention
    {
        public static string Xss(this string? s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            var _s = s.Replace("&", "&amp").Replace("<", "(").Replace(">", ")").Replace("\"", "`").Replace("\'", "`");
            return _s;
        }
    }
}
