using CMS.App.Components;
using CMS.App.Helper;
using CMS.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CMS.App.Controllers
{
    [DelayApiAttribute]
    public class OutsideApiController : Controller
    {

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
                return Result<dynamic>.Successful(data: new { });

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

        [HttpPost, Route("/SaveMessage")]
        public async Task<Result> SaveMessage(SaveMessageModel model)
        {
            Helper.MessageHelper helper = new Helper.MessageHelper(null);
            Helper.Captcha captcha = new Helper.Captcha
            {
                Text = model.CaptchaText,
                Key = model.CaptchaKey
            };
            Model.Message message = model.MessageInstance();
            return await helper.Add(message, captcha);
        }

        [HttpPost, Route("/SaveComment")]
        public async Task<Result> SaveComment([FromForm] SaveCommentModel model)
        {
            Helper.CommentHelper helper = new Helper.CommentHelper(null);
            Dal.DataSource.PostDataSource db = new Dal.DataSource.PostDataSource();
            var result = await db.GetAsync(unicId: model.PostUnicId);
            if (!result.Success || result.Data == null)
                return Result.Failure(message: "S C 1");

            Helper.Captcha captcha = new Helper.Captcha
            {
                Text = model.CaptchaText,
                Key = model.CaptchaKey
            };
            Comment message = model.CommentInstance();
            message.PostId = result.Data.Id;
            return await helper.Save(message, captcha);
        }

        [HttpPost, Route("/CaptchaReload")]
        public async Task<Result<Helper.Captcha>> CaptchaReload()
        {
            var captcha = Helper.CaptchaHelper.GenerateInPersian(charLength: 3, minDrawLine: 3, maxDrawLine: 5);
            HttpContext.Session.SetString(captcha.Key, captcha.Code);
            captcha.Code = "";
            return Result<Helper.Captcha>.Successful(data: captcha);
        }
    }
}