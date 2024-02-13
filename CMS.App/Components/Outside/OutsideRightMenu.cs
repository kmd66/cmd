using CMS.Model.Files;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class OutsideRightMenu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CMS.Dal.DataSource.PostDataSource db = new CMS.Dal.DataSource.PostDataSource();
            var model = new OutsideRightMenuModel();
            
            var item = Option.GetItem(OptionType.Social);
            if (!string.IsNullOrEmpty(item.Text))
                model.socialItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Model.Social>>(item.Text);

            var result = await db.ListAsync(new CMS.Model.PostVM() { Special = true, IsProduct = true }, 5);
            model.postItems = result.Data.ToList();

            return View(model);
        }

    }
    public class OutsideRightMenuModel
    {
        public List<Model.Post> postItems { get; set; }
        public List<Model.Social> socialItems { get; set; }
    }
}