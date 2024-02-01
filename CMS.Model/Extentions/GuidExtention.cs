using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public static class GuidExtention
    {
        public static string String(this Guid val)
        {
            return val.ToString().Replace("-","");
        }
    }
}
