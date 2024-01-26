using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CMS.Dal.DbModel
{
    [Index(nameof(UnicId))]
    public class Score : BaseModel
    {
        public Score() { }

        public long CommentId { get; set; }

        public int Value { get; set; }
    }
}
