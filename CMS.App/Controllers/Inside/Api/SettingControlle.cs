using CMS.App.Helper;
using CMS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.App.Controllers
{
    [InsideDelayApi]
    [Helper.Authorize]
    [Route("cmd-admin/setting/api")]
    public class SettingApiControlle : Controller
    {
        [HttpPost, Route("save")]
        public ActionResult Save()
        {
            return View("~/Views/Inside/Setting/Social.cshtml");
        }

    }
}
