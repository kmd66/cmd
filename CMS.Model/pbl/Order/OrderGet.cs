using System.Reflection;
using System.Text.Json;

namespace CMS.Model
{
    public class OrderGet
    {
        public List<OrderPost> posts { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public OrderType Type { get; set; }
    }
}
