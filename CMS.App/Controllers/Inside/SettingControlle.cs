using CMS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.App.Controllers
{
    [Helper.Authorize]
    [Route("cmd-admin/setting")]
    public class SettingControlle : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Inside/Setting/Index.cshtml");
        }

        [Route("general")]
        public ActionResult General()
        {
            return View("~/Views/Inside/Setting/General.cshtml");
        }

        [Route("post")]
        public ActionResult Post()
        {
            return View("~/Views/Inside/Setting/Post.cshtml");
        }

        [Route("home-slide")]
        public ActionResult HomeSlide()
        {
            return View("~/Views/Inside/Setting/HomeSlide.cshtml");
        }

        [Route("social")]
        public ActionResult Social()
        {
            return View("~/Views/Inside/Setting/Social.cshtml");
        }

        [HttpPost, Route("save")]
        public ActionResult Save()
        {
            return View("~/Views/Inside/Setting/Social.cshtml");
        }

    }
}
