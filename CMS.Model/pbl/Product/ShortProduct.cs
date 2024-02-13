using System.Reflection;
using System.Text.Json;

namespace CMS.Model
{
    public class ShortProduct
    {
        public Guid UnicId { get; set; }
        public static List<ShortProduct> Instance(List<Product> items)
        {
            string jsonString = JsonSerializer.Serialize(items);
            var model = JsonSerializer.Deserialize<List<ShortProduct>>(jsonString);
            return model;
        }
        public long Price { get; set; }

        public long SpecialPrice { get; set; }

    }
}
