using System.Text.Json;

namespace CMS.Model
{
    public class Comment : BaseModel<Comment>
    {
        public long PostId { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string WebSite { get; set; }
        public string Text { get; set; }
        public long ParentId { get; set; }
        public CommentType Type { get; set; }

        public DateTime Date { get; set; }

        public List<Comment> Childs { get; set; }
        public int Count { get; set; }
        public int? Score { get; set; }
    }
}
