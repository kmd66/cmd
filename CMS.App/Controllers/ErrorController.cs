using CMS.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMS.App.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int? statusCode)
        {
            switch (statusCode.Value)
            {
                case 404:
                    return View("~/Views/Error/Error404.cshtml");
                default:
                    return View("~/Views/Error/Error.cshtml");
            }
        }
    }
}