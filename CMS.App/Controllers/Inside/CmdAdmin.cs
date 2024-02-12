using CMS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.App.Controllers
{
    [Route("cmd-admin")]
    public class CmdAdmin : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Inside/CmdAdmin/Index.cshtml");
        }
    }
}
