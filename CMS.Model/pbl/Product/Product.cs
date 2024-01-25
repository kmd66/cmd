using System.Reflection;
using System.Text.Json;

namespace CMS.Model
{
    public class Product : BaseModel
    {
        public long Price { get; set; }

        public long SpecialPrice { get; set; }

        public string ProductID { get; set; }

        public ProductType Type { get; set; }

        public int Number { get; set; }

        public string Property { get; set; }

        public List<ProductProperty> GetPropertys()
        {
            if (string.IsNullOrEmpty(Property))
                return new List<ProductProperty>();
            else
                return JsonSerializer.Deserialize<List<ProductProperty>>(Property);
        }

        public void SetPropertys(List<ProductProperty> model)
        {
            if (model != null)
                Property = JsonSerializer.Serialize(model);
            else
                Property = "[]";
        }

        public string Img { get; set; }

        public List<ProductImg> GetImgs()
        {
            if (string.IsNullOrEmpty(Img))
                return new List<ProductImg>();
            else
                return JsonSerializer.Deserialize<List<ProductImg>>(Img);
        }

        public void SetImgs(List<ProductImg> model)
        {
            if (model != null)
                Img = JsonSerializer.Serialize(model);
            else
                Img = "[]";
        }
    }
}
