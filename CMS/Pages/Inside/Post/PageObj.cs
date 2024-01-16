using CMS.Model;

namespace CMS.Pages.Inside.Post
{
    public class PageObj: BasePageObj
    {
        public PostVM Serch = new PostVM();

        public List<Model.Post> Items = new List<Model.Post>();

        public PostHelper Helper { get; set; }
        public TagHelper TagHelper { get; set; }

        public List<Model.Menu> Menues = new List<Model.Menu>();

        public Model.Post EditItem { get; set; }
        
        public List<Model.Tag> Tags { get; set; }
    }
}
