﻿using CMS.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CMS.App.Components
{
    public class OutsideComment : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Controllers.PosteModel model)
        {
            CommentModel _model = new CommentModel { post = model };
            setCaptcha(_model);
            return View("~/Views/Shared/Outside/Comment.cshtml", _model);
        }
        void setCaptcha(CommentModel model)
        {
            model.captcha = Helper.CaptchaHelper.GenerateInPersian(charLength: 3, minDrawLine: 3, maxDrawLine: 5);
            HttpContext.Session.SetString(model.captcha.Key, model.captcha.Code);
        }
    }


    public class CommentModel
    {
        public Controllers.PosteModel post { get; set; }
        public Helper.Captcha captcha = new Helper.Captcha();
    }
    public class SaveCommentModel : Model.Comment
    {
        public Model.Comment CommentInstance()
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(this);
            var model = System.Text.Json.JsonSerializer.Deserialize<Model.Comment>(jsonString);
            return model;
        }
        public string CaptchaText { get; set; }
        public string CaptchaKey { get; set; }
        public Guid PostUnicId{ get; set; }

    }
}