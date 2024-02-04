using CMS.Model;
using System.ComponentModel.DataAnnotations;

namespace CMS.Dal.DbModel
{
    public class Order : BaseModel
    {

        [MaxLength(12)]
        public string TrackingCode { get; set; }

        public byte Type { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Mail { get; set; }

        [MaxLength(12)]
        public string Mobile { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(12)]
        public string PostalCode { get; set; }

        [MaxLength(1000)]
        public string Basket { get; set; }

        [MaxLength(5000)]
        public string Text { get; set; }

        public DateTime Date { get; set; }

    }
}
