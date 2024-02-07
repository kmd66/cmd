using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CMS.Dal.DbModel
{
    [Keyless]
    public class OrderDto : Order
    {
        public int Total { get; set; }

    }
}
