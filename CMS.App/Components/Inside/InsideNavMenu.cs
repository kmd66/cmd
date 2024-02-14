using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class InsideNavMenu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Views/Shared/Inside/NavMenu.cshtml");
        }
    }
}