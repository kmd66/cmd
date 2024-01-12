using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CMS.Dal.DbModel
{
    [Index(nameof(UnicId))]
    public class Menu : BaseModel
    {
        public int Order { get; set; }

        public byte Type { get; set; }
        
        [MaxLength(25)]
        public string Name { get; set; }

        public string Link { get; set; }

        public Guid PostId { get; set; }

        public Guid Parent { get; set; }

        public string Img { get; set; }

        public DateTime Date { get; set; }

        public bool Published { get; set; }
    }
}
