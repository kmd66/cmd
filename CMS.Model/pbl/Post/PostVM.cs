using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class PostVM : ListVM
    {

        public string? Title { get; set; }

        public string? Alias { get; set; }

        public bool? Special { get; set; }

        public bool? Published { get; set; }

        public bool? IsProduct { get; set; }

    }
}
