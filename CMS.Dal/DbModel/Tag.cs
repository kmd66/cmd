using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Dal.DbModel
{
    public class Tag 
    {
        public Guid Id { get; set; }

        [MaxLength(25)]
        public string Text { get; set; }

        [ForeignKey("Post")]
        public long PostId { get; set; }

        public Post Post { get; set; }
    }
}

