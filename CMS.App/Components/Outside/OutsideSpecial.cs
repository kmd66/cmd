using CMS.Model.Files;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class OutsideSpecial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<CMS.Model.Post> items)
        {
            return View("~/Views/Shared/Outside/Special.cshtml", items);
        }
    }
}