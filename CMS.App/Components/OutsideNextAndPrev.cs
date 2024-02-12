using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class OutsideNextAndPrev : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Model.Post> model)
        {
            return View(model);
        }
    }
}