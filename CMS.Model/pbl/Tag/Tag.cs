namespace CMS.Model
{
    public class Tag
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public long PostId { get; set; }
    }
}
