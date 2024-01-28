using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CMS.Dal.DbModel
{
    [Index(nameof(UnicId))]
    public class Comment : BaseModel
    {
        public Comment() { }
        public long PostId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)] 
        public string Mail { get; set; }
        
        [MaxLength(70)]
        public string WebSite { get; set; }
        
        [MaxLength(500)]
        public string Text { get; set; }
        
        public long ParentId { get; set; }
        
        public byte Type { get; set; }

        public DateTime Date { get; set; }
    }
}
