using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public static class DateExtention
    {
        static PersianCalendar pc = new PersianCalendar();
        public static string ToPersion(this DateTime d)
        {
            try
            {
                var t = pc.GetMonth(d);
                return $"{pc.GetYear(d)}/{pc.GetMonth(d)}/{pc.GetDayOfMonth(d)} {pc.GetHour(d)}:{pc.GetMinute(d)}";
            }
            catch
            {
                var _d = DateTime.Now;
                return $"{pc.GetYear(_d)}/{pc.GetMonth(_d)}/{pc.GetDayOfMonth(_d)} {pc.GetHour(_d)}:{pc.GetMinute(_d)}";
            }
        }
    }
}
