using CMS.App.Components;
using CMS.App.Helper;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CMS.App.Controllers
{
    [Delay]
    public class OrderController : Controller
    {
        SaveOrderModel model = new SaveOrderModel();

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
            if (string.IsNullOrEmpty(trackingcode) || trackingcode.Length != 10)
                return Redirect("/");

            var captcha = Helper.CaptchaHelper.GenerateInPersian(charLength: 3, minDrawLine: 3, maxDrawLine: 5);
            HttpContext.Session.SetString(captcha.Key, captcha.Code);

            ViewBag.TrackingCode = trackingcode;
            return View("~/Views/Outside/Basket/Tracking.cshtml", model);
        }
    }
    public class SaveOrderModel : Model.Order
    {
        public Model.Order OrderInstance()
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(this);
            var model = System.Text.Json.JsonSerializer.Deserialize<Model.Order>(jsonString);
            return model;
        }
        public string CaptchaText { get; set; }
        public string CaptchaKey { get; set; }
        public string TrackingCode { get; set; }
        public List<string> ListBasket { get; set; }
    }
}