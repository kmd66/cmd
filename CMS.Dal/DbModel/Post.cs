using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CMS.Dal.DbModel
{
    [Index(nameof(UnicId))]
    public class Post: BaseModel
    {

        public Guid MenuId { get; set; }

        [MaxLength(25)]
        public string Title { get; set; }

        [MaxLength(25)]
        public string Alias { get; set; }

        [MaxLength(350)]
        public string Summary { get; set; }

        public string Content { get; set; }

        public string Img { get; set; }

        public bool Special { get; set; }

        public bool Published { get; set; }

        public DateTime? PublishDown { get; set; }

        public DateTime Date { get; set; }

        public byte Access { get; set; }

        public int Hit { get; set; }
    }
}

