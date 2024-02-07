using CMS.Model;

namespace CMS.Pages.Inside.Order
{
    public class PageObj: BasePageObj
    {
        public OrderVM Serch = new OrderVM { PageIndex = 1, PageSize = 10, Type = OrderType.سفارش };

        public List<Model.Order> Items = new List<Model.Order>();

        public Model.Order EditItem { get; set; }
        public CMS.Model.OrderGet? OrderGet { get; set; }

        public OrderHelper Helper { get; set; }
        public CMS.Pages.Inside.Post.PostHelper PostHelper { get; set; }

    }
}
