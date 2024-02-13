using CMS.App.Components;
using CMS.App.Models;
using CMS.Dal.DbModel;
using CMS.Dal.Migrations;
using CMS.Dal.Migrations.CntContextsMigrations;
using CMS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.Diagnostics;

namespace CMS.App.Controllers
{
    public class PostController : Controller
    {
        Dal.DataSource.PostDataSource db = new Dal.DataSource.PostDataSource();
        Dal.DataSource.PostOptionDataSource dbPostOption = new Dal.DataSource.PostOptionDataSource();
        Dal.DataSource.ProductDataSource dbProduct = new Dal.DataSource.ProductDataSource();
        Helper.CommentHelper commentHelper = new Helper.CommentHelper(null);
        PosteModel model = new PosteModel();

        [Route("/{name}")]
        public async Task<IActionResult> Post(string name)
        {
            model.name = name;
            ViewBag.PrefixLink = "";

            await SeletePost(false);

            if (model.post == null)
                await SeleteMenuPost();

            if (model.post == null)
                return Redirect("/Error/404");

            return View("~/Views/Outside/Post/Index.cshtml", model);
        }

        [Route("/product/{name}")]
        public async Task<IActionResult> Product(string name)
        {
            model.name = name;
            ViewBag.PrefixLink = "/product";

            await SeletePost(true);

            if (model.post == null)
                return Redirect("/Error/404");

            return View("~/Views/Outside/Post/ProductIndex.cshtml", model);
        }

        async Task SeletePost(bool isProduct)
        {
            var result = await db.GetByAliasAsync(model.name, published: true, isProduct: isProduct);
            if (result.Data == null)
                return;

            model.post = result.Data;

            await selectPostOption();

            if (isProduct)
                await SeleteProduct();
            if (result.Data == null)
                return;

            var resultNextAndPrev = await db.NextAndPrevAsync(model.post);
            model.nextAndPrev = resultNextAndPrev.Data;

            var resultRelated = await db.RelatedAsync(model.post, 3);
            model.related = resultRelated.Data;

            await selectComments();
        }
        async Task SeleteProduct()
        {
            var resultProduct = await dbProduct.GetAsync(unicId: model.post.UnicId);
            if (resultProduct.Data == null)
            {
                model.post = null;
                return;
            }
            model.product = resultProduct.Data;
            model.productPropertys = model.product.GetPropertys(); ;
        }
        async Task selectComments()
        {
            if (model.postOption?.IsComment == false)
                return;

            model.scoreComment = new ScoreComment();
            var result = await commentHelper.List(new CommentVM { PostId = model.post.Id, Type = CommentType.منتشر_شده });
            model.scoreComment = result.Data;
        }
        async Task SeleteMenuPost()
        {
            var result = await db.GetMenuPost(model.name);
            if (result.Data == null)
                return;
            model.isMenuPost = true;
            model.post = result.Data;
        }

        async Task selectPostOption()
        {
            var resultPostOption = await dbPostOption.GetAsync(unicId: model.post.UnicId);
            if (resultPostOption.Data == null)
                return;
            model.postOption = resultPostOption.Data;
        }

    }
    public class PosteModel
    {
        public string id = Guid.NewGuid().String();
        public string name { get; set; }
        public Model.Post post { get; set; }

        public ScoreComment scoreComment = new ScoreComment();
        public Model.PostOption postOption { get; set; }
        public CMS.Model.Product product { get; set; }
        public List<ProductProperty> productPropertys = new List<ProductProperty>();

        public bool isMenuPost = false;
        public List<Model.Post> nextAndPrev = new List<Model.Post>();
        public List<Model.Post> related = new List<Model.Post>();

    }
}