using System.Text.Json;

namespace CMS.Model
{
    public class CommentVM : ListVM
    {
        public long PostId { get; set; }
        public string? Name { get; set; }
        public CommentType Type { get; set; }
    }
}
