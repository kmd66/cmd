using CMS.Model.Files;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class OutsideSlide : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<CMS.Model.HomeSlide> items = new List<Model.HomeSlide>();
            var option = CMS.Model.Option.GetItem(CMS.Model.OptionType.HomeSlide);
            if (!string.IsNullOrEmpty(option.Text))
                items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CMS.Model.HomeSlide>>(option.Text);

            return View("~/Views/Shared/Outside/Slide.cshtml", items);
        }

    }
}