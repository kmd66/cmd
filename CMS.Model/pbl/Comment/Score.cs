using System.Text.Json;

namespace CMS.Model
{
    public class Score : BaseModel
    {
        public Score() { }

        public long CommentId { get; set; }

        public int Value { get; set; }
    }
}