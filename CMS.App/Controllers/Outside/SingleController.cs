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
    public class SingleController : Controller
    {
        CMS.Dal.DataSource.PostDataSource db = new CMS.Dal.DataSource.PostDataSource();
        Helper.MessageHelper helper = new Helper.MessageHelper(null);
        SingleModel model = new SingleModel();

        [Route("/contact")]
        public async Task<IActionResult> Contact()
        {
            setCaptcha();
            await SeletePost();
            return View("~/Views/Outside/Single/Contact.cshtml", model);
        }

        [HttpPost, Route("/CaptchaReload")]
        public async Task<Result<Helper.Captcha>> CaptchaReload()
        {
            var captcha = Helper.CaptchaHelper.GenerateInPersian(charLength: 3, minDrawLine: 3, maxDrawLine: 5);
            HttpContext.Session.SetString(captcha.Key, captcha.Code);
            captcha.Code = "";
            return Result<Helper.Captcha>.Successful(data: captcha);
        }

        [HttpPost, Route("/SaveMessage")]
        public async Task<Result> SaveMessage(SaveMessageModel model)
        {
            Helper.Captcha captcha = new Helper.Captcha
            {
                Text = model.CaptchaText,
                Key = model.CaptchaKey
            };
            Model.Message message = model.MessageInstance();
            return await helper.Add(message, captcha);
        }

        void setCaptcha()
        {
            model.captcha = Helper.CaptchaHelper.GenerateInPersian(charLength: 3, minDrawLine: 3, maxDrawLine: 5);
            HttpContext.Session.SetString(model.captcha.Key, model.captcha.Code);
        }
        async Task SeletePost()
        {
            var homePostId = CMS.Model.Option.GetItem(CMS.Model.OptionType.ContactPostId);
            if (string.IsNullOrEmpty(homePostId.Text))
                return;

            var id = new Guid(homePostId.Text);
            var result = await db.GetAsync(0, id);
            if (result.Data == null)
                return;

            model.post = result.Data;
        }

    }
    public class SingleModel
    {
        public string id = Guid.NewGuid().String();
        public CMS.Model.Post post { get; set; }

        public Helper.Captcha captcha = new Helper.Captcha();
        public CMS.Model.Message message { get; set; }

    }
    public class SaveMessageModel: Model.Message
    {
        public Model.Message MessageInstance()
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(this);
            var menu = System.Text.Json.JsonSerializer.Deserialize<Model.Message>(jsonString);
            return menu;
        }
        public string CaptchaText { get; set; }
        public string CaptchaKey { get; set; }

    }
}