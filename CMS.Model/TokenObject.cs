using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class TokenObject
    {
        public string Expires { get; set; }
        public string Issued { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
    }
}
