using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public static class QueryExtention
    {
        public static string Query(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return "NULL";
            return $"N'{s}'";
        }
        public static string JsonQuery(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return "[]";
            return s;
        }
        public static string Query(this int? i)
        {
            if (i == null)
                return "NULL";
            return i.ToString();
        }
        public static string Query(this int i)
            => i.ToString();

        public static string Query(this bool? b)
        {
            if (b == null)
                return "NULL";
            return (bool)b ? "1" : "0";
        }

        public static string Query(this bool b)
            => b? "1" : "0";

        public static string Query(this long? i)
        {
            if (i == null)
                return "NULL";
            return i.ToString();
        }
        public static string Query(this long i)
            => i.ToString();
        public static string Query(this byte i)
            => i.ToString();
    }
}
