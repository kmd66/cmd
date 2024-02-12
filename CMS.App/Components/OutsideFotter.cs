using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class OutsideFotter : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CMS.Dal.DataSource.PostDataSource db = new CMS.Dal.DataSource.PostDataSource();
            var resultItems = await db.ListAsync(new CMS.Model.PostVM() { IsProduct = false }, 5);
            var resultProduct = await db.ListAsync(new CMS.Model.PostVM() { IsProduct = true }, 5);
            var model = new OutsideFotterModel
            {
                PostItems = resultItems.Data.ToList(),
                ProductItems =  resultProduct.Data.ToList(),

            };

            return View(model);
        }
    }
    public class OutsideFotterModel
    {
        public List<Model.Post> PostItems { get; set; }
        public List<Model.Post> ProductItems { get; set; }
    }
}