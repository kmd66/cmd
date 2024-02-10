using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CMS.Dal.DbModel
{
    [Index(nameof(UnicId))]
    public class PostOption : BaseModel
    {
        public PostOption() { }

        public bool? NoFlow { get; set; }

        public bool? NoIndex { get; set; }

        public bool? IsComment { get; set; }

        public bool? IsScore { get; set; }

        public string Redirect { get; set; }

    }
}

