using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class OutsideNextAndPrev : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Model.Post> model)
        {
            return View("~/Views/Shared/Outside/NextAndPrev.cshtml", model);
        }
    }
}