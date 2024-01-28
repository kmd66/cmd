using Microsoft.EntityFrameworkCore;

namespace CMS.Dal.DbModel
{
    [Keyless]
    public class CommentDto : Comment
    {
        public string? MenuName { get; set; }
        public int Total { get; set; }
    }
}
