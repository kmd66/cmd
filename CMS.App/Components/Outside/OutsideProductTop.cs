using CMS.Model.Files;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class OutsideProductTop : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Controllers.PosteModel model)
        {
            return View("~/Views/Shared/Outside/ProductTop.cshtml", model);
        }

    }
}