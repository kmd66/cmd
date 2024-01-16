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
        public static long StringToLong(this string i)
        {
            try
            {
                return long.Parse(i);
            }
            catch
            {
                return 0;
            }
        }

        public static bool IsNullOrEmpty(this long? i) => i == null || i == 0 ? false : true;
        public static bool IsNullOrEmpty(this long i) => i == 0 ? false : true;
        public static bool IsNullOrEmpty(this double? i) => i == null || i == 0 ? false : true;
        public static bool IsNullOrEmpty(this double i) => i == 0 ? false : true;
        public static bool IsNullOrEmpty(this int? i)=> i == null || i == 0? false : true;
        public static bool IsNullOrEmpty(this int i) => i == 0 ? false : true;

        public static double Default(this long? i) => i == null ? 0 : (int)i;
        public static double Default(this double? i) => i == null ? 0 : (int)i;
        public static int Default(this int? i) => i == null ? 0 : (int)i;

        public static int ToInt(this double i) => (int)i;
        public static int ToInt(this double? i) => i == null ? 0 : (int)i;


        public static double ToDouble (this int? i) => i == null ? 0 : (int)i;
        public static double ToDouble(this int i) => i;
    }
}
