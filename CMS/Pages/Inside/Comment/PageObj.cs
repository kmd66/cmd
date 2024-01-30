using CMS.Model;

namespace CMS.Pages.Inside.Comment
{
    public class PageObj: BasePageObj
    {
        public PostVM Serch = new PostVM { PageIndex = 1, PageSize = 10 };

        public List<Model.Comment> Items = new List<Model.Comment>();

        public Model.Comment EditItem { get; set; }

        public CommentHelper Helper { get; set; }

    }
}
