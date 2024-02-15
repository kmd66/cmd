using CMS.App.Helper;
using CMS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.App.Controllers
{
    [Helper.Authorize]
    [Route("cmd-admin")]
    public class AdminControlle : Controller
    {
        [InsideDelay]
        [Route("")]
        [Route("index")]
        public ActionResult Index()
        {
            return View("~/Views/Inside/CmdAdmin/Index.cshtml");
        }
        
        //[AllowAnonymous, Route("test")]
        //public ActionResult Test()
        //{
        //    return View("~/Views/Inside/CmdAdmin/Index.cshtml");
        //}
    }
}
