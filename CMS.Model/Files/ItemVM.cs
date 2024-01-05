using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model.Files
{
    public class ItemVM
    {
        public FileExtensions Extension { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
    }
}
