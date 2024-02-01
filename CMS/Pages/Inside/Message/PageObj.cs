using CMS.Model;

namespace CMS.Pages.Inside.Message
{
    public class PageObj: BasePageObj
    {
        public MessageVM Serch = new MessageVM { PageIndex = 1, PageSize = 10, Type = MessageType.خوانده_نشده };

        public List<Model.Message> Items = new List<Model.Message>();

        public Model.Message EditItem { get; set; }

        public MessageHelper Helper { get; set; }

    }
}
