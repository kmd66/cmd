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

    }
}
