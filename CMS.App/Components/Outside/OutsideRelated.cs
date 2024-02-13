using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class OutsideRelated : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Model.Post> model)
        {
            return View("~/Views/Shared/Outside/Related.cshtml", model);
        }
    }
}