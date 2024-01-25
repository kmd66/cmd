using CMS.Model;

namespace CMS.Pages.Inside.Post
{
    public class PageObj: BasePageObj
    {
        public PostVM Serch = new PostVM {PageIndex=1,PageSize = 10 };

        public List<Model.Post> Items = new List<Model.Post>();

        public List<Model.Menu> Menues = new List<Model.Menu>();

        public Model.Post EditItem { get; set; }

        public Model.Product EditProductItem { get; set; }

        public List<ProductProperty> ProductPropertys { get; set; }

        public List<ProductImg> Imgs = new List<ProductImg>();

        public List<Model.Tag> Tags { get; set; }


        public PostHelper Helper { get; set; }

        public ProductHelper ProductHelper { get; set; }

        public TagHelper TagHelper { get; set; }
    }
}
