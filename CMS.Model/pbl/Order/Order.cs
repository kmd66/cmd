using System.Reflection;
using System.Text.Json;

namespace CMS.Model
{
    public class Order : BaseModel<Order>
    {
        public string TrackingCode { get; set; }

        public OrderType Type { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Mail { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string Basket { get; set; }
        public List<string> GetBasket()
        {
            if (string.IsNullOrEmpty(Basket))
                return new List<string>();
            return JsonSerializer.Deserialize<List<string>>(Basket);
        }
        public void SetBasket(List<string> model)
        {
            if (model == null)
                Basket = "[]";
            else
                Basket = JsonSerializer.Serialize(model);
        }

        public string Text { get; set; }
        
        public DateTime Date { get; set; }

        public int Total { get; set; }

    }
}
