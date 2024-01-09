using CMS.Helper;
using CMS.Model;
using CMS.Model.Files;

namespace CMS.Pages.Inside.FileManager
{
    public class PageObj: BasePageObj
    {
        public ItemVM Serch = new ItemVM();

        public List<Item> Items = new List<Item>();
        public UploadHelper Upload { get; set; }
        public Func<Item,Task> SelectItem { get; set; }
        public Item? RemoveItem { get; set; }
        public Item? ChengeNameItem { get; set; }
    }
}
