using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CMS.Dal.DbModel
{
    [Index(nameof(UnicId))]
    public class Post: BaseModel
    {

        [MaxLength(25)]
        public string Title { get; set; }

        [MaxLength(25)]
        public string Alias { get; set; }

        [MaxLength(350)]
        public string Summary { get; set; }

        public bool Content { get; set; }

        public string Img { get; set; }

        public bool Special { get; set; }

        public bool published { get; set; }

        public DateTime? publishDown { get; set; }

        public DateTime Date { get; set; }

        public byte Access { get; set; }

        public int Hit { get; set; }
    }
}

