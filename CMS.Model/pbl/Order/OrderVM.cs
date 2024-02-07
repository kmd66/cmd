namespace CMS.Model
{
    public class OrderVM : ListVM
    {
        public OrderType Type { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? TrackingCode { get; set; }

    }
}
