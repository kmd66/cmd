using Microsoft.EntityFrameworkCore;

namespace CMS.Dal.DbModel
{
    [Keyless]
    public class CommentDto : Comment
    {
        public int Count { get; set; }
        public int Score { get; set; }
    }
}
