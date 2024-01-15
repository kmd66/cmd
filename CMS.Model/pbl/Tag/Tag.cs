using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class Tag : BaseModel<Tag>
    {
        public Tag() { }
        public long PostID { get; set; }
        public string Text { get; set; }
    }
}
