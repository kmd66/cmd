using CMS.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMS.App.Controllers
{
    public class HomeController : Controller
    {
        CMS.Dal.DataSource.PostDataSource db = new CMS.Dal.DataSource.PostDataSource();
        HomeModel model = new HomeModel();

        public async Task<IActionResult> Index()
        {
            await SeletePost();
            await SeleteSpecialItem();
            return View(model);
        }

        async Task SeleteSpecialItem()
        {
            model.itemsTop = new List<Model.Post>();
            model.itemsBot = new List<Model.Post>();
            var result = await db.ListAsync(new CMS.Model.PostVM() { Special = true, IsProduct = false }, 6);
            var items = result.Data.ToList();
            if (items.Count < 1)
                return;
            for (int i = 0; i < items.Count; ++i)
            {
                if (i < 3)
                    model.itemsTop.Add(items[i]);
                else
                    model.itemsBot.Add(items[i]);
            }
        }

        async Task SeletePost()
        {
            var homePostId = CMS.Model.Option.GetItem(CMS.Model.OptionType.HomePostId);
            if (string.IsNullOrEmpty(homePostId.Text))
                return;

            var id = new Guid(homePostId.Text);
            var result = await db.GetAsync(0, id);
            if (result.Data == null)
                return;

            model.post = result.Data;
        }
    }
    public class HomeModel
    {
        public CMS.Model.Post post { get; set; }

        public List<CMS.Model.Post> itemsTop { get; set; }
        public List<CMS.Model.Post> itemsBot { get; set; }
    }
}