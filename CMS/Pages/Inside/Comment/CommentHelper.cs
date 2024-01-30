
using CMS.Dal;
using CMS.Dal.DataSource;
using CMS.Helper;
using CMS.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CMS.Pages.Inside.Comment
{
    public class CommentHelper : BaseHelper
    {
        public CommentHelper(string? auth/*, string? remoteIpAddress*/)
            : base(auth)
        {
            _dataSource = new CommentDataSource();
            _menuDataSource = new MenuDataSource();
        }
        private readonly CommentDataSource _dataSource;
        private readonly MenuDataSource _menuDataSource;

        public async Task<Result> Save(Model.Comment model,Captcha captcha = null)
        {
            if (model.UnicId == Guid.Empty)
                return await Add(model);
            return await Edit(model);
        }
        private async Task<Result> Add(Model.Comment model, Captcha captcha)
        {
            if(captcha == null)
                return Result.Successful();

            if(string.IsNullOrEmpty(captcha.Code) || !CaptchaHelper.Validate(captcha))
                return Result.Failure(message: "کد امنیتی را وارد کنید");

            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            if (model.Score ==0)
                return Result.Failure(message: "امتیاز را وارد کنید");

            model.UnicId = Guid.NewGuid();
            model.Date = DateTime.Now;

            var result = await _dataSource.AddAsync(model);
            if (!result.Success)
                return Result<Model.Comment>.Failure(message: result.Message);

            var comment = await _dataSource.GetAsync(0, model.UnicId);
            if (comment.Data != null)
                await _dataSource.AddScoreAsync(comment.Data.Id, model.Score);
         
            return Result.Successful();
        }
        private async Task<Result> Edit(Model.Comment model)
        {
            if (!isAuthorize)
                return Result<Model.Comment>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result<Model.Comment>.Failure(message: validationResult.Message);


            var result = await _dataSource.EditAsync(model);
            if (!result.Success)
                return Result<Model.Comment>.Failure(message: result.Message);

            return Result<Model.Comment>.Successful();
        }
        private async Task<Result> validation(Model.Comment model)
        {
            if (string.IsNullOrEmpty(model.Text))
                return Result.Failure(message: "متن را وارد نشده");

            if (string.IsNullOrEmpty(model.Name))
                return Result.Failure(message: "نام را وارد نشده");

            if (string.IsNullOrEmpty(model.Mail))
                return Result.Failure(message: "ایمیل را وارد نشده");

            if (!new EmailAddressAttribute().IsValid(model.Mail))
                return Result.Failure(message: "ایمیل را صحیح وارد کنید");

            if (string.IsNullOrEmpty(model.WebSite))
                model.WebSite = "";

            model.Text = model.Text.Xss();
            model.Name = model.Name.Xss();
            model.Mail = model.Mail.Xss();
            model.WebSite = model.WebSite.Xss();

            if (model.Type == CommentType.Unknown)
                model.Type = CommentType.در_انتضار_تایید;

            return Result.Successful();
        }

        public async Task<Result<ScoreComment>> List(CommentVM modelVm)
        {

            var retult = await _dataSource.ListAsync(modelVm);
            if (!retult.Success)
                return Result<ScoreComment>.Failure(message: retult.Message);
            var model = retult.Data.ToList();
            var list = new List<Model.Comment>();
            
            var scores = model.Where(x => x.Score > 0).ToList();

            foreach (var item in model)
            {
                if (item.ParentId == 0)
                {
                    item.Childs = model.Where(x => x.ParentId == item.Id).ToList();
                    list.Add(item);
                }
            }
            var t = scores.Count;
            return Result<ScoreComment>.Successful(data:
                new ScoreComment
                {
                    Comments = list,
                    ScoreCount = scores.Count,
                    ScoreSum = scores.Sum(x=>x.Score),
                });

        }
    }
}
