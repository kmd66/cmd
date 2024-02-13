using CMS.App.Components;
using CMS.App.Models;
using CMS.Dal.DbModel;
using CMS.Dal.Migrations;
using CMS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Diagnostics;

namespace CMS.App.Controllers
{
    public class OrderController : Controller
    {
        CMS.Dal.DataSource.PostDataSource db = new CMS.Dal.DataSource.PostDataSource();
        Helper.MessageHelper helper = new Helper.MessageHelper(null);
        SingleModel model = new SingleModel();

        [Route("/basket")]
        public async Task<IActionResult> Basket()
        {
            return View("~/Views/Outside/Basket/Index.cshtml", model);
        }

        [Route("/basket/order")]
        public async Task<IActionResult> Order()
        {
            return View("~/Views/Outside/Basket/Order.cshtml", model);
        }

        [Route("/basket/tracking/{trackingcode}")]
        public async Task<IActionResult> Tracking(string trackingcode)
        {
            return View("~/Views/Outside/Basket/Tracking.cshtml", model);
        }
    }
}