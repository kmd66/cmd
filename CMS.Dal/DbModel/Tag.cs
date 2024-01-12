using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CMS.Dal.DbModel
{
    [Index(nameof(UnicId))]
    public class Tag : BaseModel
    {

        public long PostID { get; set; }
        [MaxLength(25)]
        public string Text { get; set; }
    }
}

