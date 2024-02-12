using CMS.App.Components;
using CMS.App.Models;
using CMS.Dal.Migrations;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMS.App.Controllers
{
    public class CategoryController : Controller
    {
        CMS.Dal.DataSource.PostDataSource db = new CMS.Dal.DataSource.PostDataSource();
        CategoryModel model = new CategoryModel();
        public PostPagInationModel postPagInationModel { get; set; }

        [Route("/category/{name}/{pageNumber?}")]
        public async Task<IActionResult> Category(string name, int? pageNumber)
        {
            await selectMenu(name, pageNumber);
            postPagInationModel = new PostPagInationModel
            {
                Style = "background: unset;",
                Link = $"/category/{model.menu?.Alias}"
            };
            if (model.menu == null)
                return Redirect("/Error/404");

            await selectItem();

            model.postPagInation = await new PostPagInationView().InvokeAsync(postPagInationModel);

            return View("~/Views/Outside/Category/Index.cshtml", model);
        }

        [Route("/product-category/{name}/{pageNumber?}")]
        public async Task<IActionResult> ProductCategory(string name, int? pageNumber)
        {
            await selectMenu(name, pageNumber);
            postPagInationModel = new PostPagInationModel
            {
                Style = "background: unset;",
                Link = $"/product-category/{model.menu?.Alias}"
            };

            if (model.menu == null)
                return Redirect("/Error/404");
            await selectProductItem();

            model.postPagInation = await new PostPagInationView().InvokeAsync(postPagInationModel);

            return View("~/Views/Outside/Category/ProductIndex.cshtml", model);
        }

        [Route("search/{name}/{pageNumber?}")]
        public async Task<IActionResult> SearchCategory(string name, int? pageNumber)
        {
            model.searchText = name.Replace("-", " ");
            model.Serch.Title = name;
            await selectMenu(name, pageNumber,false);
            postPagInationModel = new PostPagInationModel
            {
                Style = "background: unset;",
                Link = $"/search/{name}"
            };

            await selectSearchItem();

            model.postPagInation = await new PostPagInationView().InvokeAsync(postPagInationModel);

            return View("~/Views/Outside/Category/SerchIndex.cshtml", model);
        }

        async Task selectMenu(string name, int? pageNumber, bool isSelectMenu = true) 
        {
            if (pageNumber.Default() == 0)
                model.Serch.PageIndex = 1;
            else
            {
                try
                {
                    model.Serch.PageIndex = pageNumber;
                }
                catch
                {
                    model.Serch.PageIndex = 1;
                }
            }

            if (isSelectMenu)
            {
                model.menu = Menus.GetByAlias(name);
                model.Serch.MenuId = model.menu?.UnicId;
            }

            try
            {
                var homePostId = CMS.Model.Option.GetItem(CMS.Model.OptionType.PostCount);
                model.Serch.PageSize = int.Parse(homePostId.Text);
            }
            catch
            {
                model.Serch.PageSize = 10;
            }
        }

        async Task selectItem()
        {
            var result = await db.ListAsync(model.Serch);
            model.items = result.Data.ToList();

            postPagInationModel.ItemModel = new PageInation
            {
                PageIndex = model.Serch?.PageIndex,
                PageSize = model.Serch?.PageSize,
                Total = model.items?.Count > 0 ? model.items[0].Total : 0,
                ItemsCount = model.items.ToList().Count
            };

        }
        async Task selectProductItem()
        {
            var result = await db.ListPostProductAsync(model.Serch);
            model.productItems = result.Data?.ToList();

            postPagInationModel.ItemModel = new PageInation
            {
                PageIndex = model.Serch?.PageIndex,
                PageSize = model.Serch?.PageSize,
                Total = model.productItems?.Count > 0 ? model.productItems[0].Total : 0,
                ItemsCount = model.productItems.ToList().Count
            };

        }
        async Task selectSearchItem()
        {
            var result = await db.SearchAsync(model.Serch);
            if(!result.Success)
            {
                model.msg = result.Message;
                return;
            }
            model.searchItems = result.Data.ToList();
            postPagInationModel.ItemModel = new PageInation
            {
                PageIndex = model.Serch?.PageIndex,
                PageSize = model.Serch?.PageSize,
                Total = model.searchItems?.Count > 0 ? model.searchItems[0].Total : 0,
                ItemsCount = model.searchItems.ToList().Count
            };
            model.total = postPagInationModel.ItemModel.Total;
        }
    }
    public class CategoryModel
    {
        public Menu? menu { get; set; }
        public PostVM Serch = new PostVM { PageIndex = 1, PageSize = 10, Published = true };
        public List<Post> items = new List<Post>();
        public List<PostProduct> productItems = new List<PostProduct>();
        public List<PostProduct> searchItems = new List<PostProduct>();
        public string searchText { get; set; }
        public int? total { get; set; }
        public string postPagInation { get; set; }
        public string msg { get; set; }
    }
}