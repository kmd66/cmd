using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace CMS.Dal.DbModel
{
    [Index(nameof(UnicId))]
    public class Message : BaseModel
    {
        public byte Type { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Mail { get; set; }
        [MaxLength(11)]
        public string Phone { get; set; }
        [MaxLength(500)]
        public string Text { get; set; }

        public DateTime Date { get; set; }

    }
}
