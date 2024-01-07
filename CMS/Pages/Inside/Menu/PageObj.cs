
using CMS.Model;
using CMS.Model.Files;

namespace CMS.Pages.Inside.Menu
{
    public class PageObj: BasePageObj
    {
        public MenuVM Serch = new MenuVM();

        public List<Model.Menu> Items = new List<Model.Menu>();
        public MenuHelper Helper { get; set; }
        public Model.Menu? EditItem { get; set; }
    }
}
