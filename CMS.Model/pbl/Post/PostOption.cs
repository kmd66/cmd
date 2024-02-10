using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class PostOption : BaseModel<PostOption>
    {
        public PostOption() { }

        public bool NoFlow { get; set; }

        public bool NoIndex { get; set; }

        public bool IsComment { get; set; }

        public bool IsScore { get; set; }

        public string Redirect { get; set; }

    }
}

