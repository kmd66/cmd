using Microsoft.AspNetCore.Mvc;

namespace CMS.App.Components
{
    public class InsideSideMenu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Model.Status status = new Model.Status();
            Dal.DataSource.StatusDataSource db = new Dal.DataSource.StatusDataSource();
            var result = await db.GetAsync();
            if (result.Data != null)
                status = result.Data;
            return View("~/Views/Shared/Inside/SideMenu.cshtml", status);
        }
    }
}