using System.Text.Json;

namespace CMS.Model
{
    public class MessageVM : ListVM
    {
        public string? Name { get; set; }
        public MessageType Type { get; set; }
    }
}
