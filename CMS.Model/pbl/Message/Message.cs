using CMS.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CMS.Model.pbl.Message
{
    public class Message : BaseModel<Comment>
    {
        public MessageType Type { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Text { get; set; }
    }
}