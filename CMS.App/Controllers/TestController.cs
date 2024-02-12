using CMS.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMS.App.Controllers
{
    [Route("/cms-test")]
    public class TestController : Controller
    {

        public TestController()
        {
        }

        public IActionResult Index()
        {
            return View("~/Views/Inside/CmsLogin/Index.cshtml");
        }

    }
}