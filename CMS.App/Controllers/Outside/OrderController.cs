using CMS.App.Components;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CMS.App.Controllers
{
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

        [HttpPost, Route("/basket/tracking/getItems")]
        public async Task<Result<OrderGet>> GetTrackingItems([FromForm] SaveOrderModel model)
        {
            Helper.OrderHelper helper = new Helper.OrderHelper(null);

            Helper.Captcha captcha = new Helper.Captcha
            {
                Text = model.CaptchaText,
                Key = model.CaptchaKey
            };
            if (captcha == null)
                return Result<OrderGet>.Failure(message: "کد امنیتی را وارد کنید");

            if (string.IsNullOrEmpty(captcha.Text) || !Helper.CaptchaHelper.Validate(captcha))
                return Result<OrderGet>.Failure(message: "کد امنیتی را وارد کنید");

            var result = await helper.GetyByTrackingCode(model.TrackingCode);

            return await helper.GetyByTrackingCode(model.TrackingCode);
        }

        [HttpPost, Route("/SaveOrder")]
        public async Task<Result<string>> SaveComment([FromForm] SaveOrderModel model)
        {
            if (model.ListBasket == null || model.ListBasket.Count == 0)
                return Result<string>.Successful(data: "e1rwq21sdfe1");

            Helper.OrderHelper helper = new Helper.OrderHelper(null);

            var basket = model.ListBasket.Take(15).ToList();
            Helper.Captcha captcha = new Helper.Captcha
            {
                Text = model.CaptchaText,
                Key = model.CaptchaKey
            };
            Order message = model.OrderInstance();
            return await helper.Add(model, captcha, basket);
        }

        [HttpPost, Route("/Order/GetPost")]
        public async Task<Result<dynamic>> GetPost([FromForm] List<string> model)
        {
            if (model == null || model.Count == 0)
                return Result<dynamic>.Successful(data: new {  });

            var basket = model.Take(15).ToList();
            CMS.Dal.DataSource.PostDataSource db = new CMS.Dal.DataSource.PostDataSource();
            CMS.Dal.DataSource.ProductDataSource dbProduct = new CMS.Dal.DataSource.ProductDataSource();

            var postResult = await db.ListAsync(basket);
            if (postResult.Data.ToList().Count == 0)
                return Result<dynamic>.Successful(data: new { });

            var posts = ShortPost.Instance(postResult.Data.ToList());

            var productResult = await dbProduct.ListAsync(postResult.Data.ToList().Select(x => x.UnicId).ToList());
            var products = ShortProduct.Instance(productResult.Data.ToList());

            return Result<dynamic>.Successful(data: new { Posts = posts, Products = products });
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