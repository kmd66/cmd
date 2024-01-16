using Microsoft.EntityFrameworkCore;

namespace CMS.Dal.DbModel
{
    [Keyless]
    public class PostDto : Post
    {
        public string? MenuName { get; set; }
        public int Total { get; set; }
    }
}
