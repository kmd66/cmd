namespace CMS.Model
{
    public class Order : BaseModel
    {
        public long ProductId { get; set; }

        public string TrackingCode { get; set; }

        public OrderType Type { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Mail { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string Basket { get; set; }

    }
}
