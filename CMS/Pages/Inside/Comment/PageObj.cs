using CMS.Model;

namespace CMS.Pages.Inside.Comment
{
    public class PageObj: BasePageObj
    {
        public CommentVM Serch = new CommentVM { PageIndex = 1, PageSize = 10, Type = CommentType.در_انتضار_تایید };

        public List<Model.Comment> Items = new List<Model.Comment>();

        public Model.Comment EditItem { get; set; }

        public CommentHelper Helper { get; set; }
        public CMS.Pages.Inside.Post.PostHelper PostHelper { get; set; }

    }
}
