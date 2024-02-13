using CMS.Dal.DataSource;
using CMS.Model;
using System.ComponentModel.DataAnnotations;

namespace CMS.App.Helper
{
    public class CommentHelper : BaseHelper
    {
        public CommentHelper(string? auth/*, string? remoteIpAddress*/)
            : base(auth)
        {
            _dataSource = new CommentDataSource();
        }
        private readonly CommentDataSource _dataSource;

        public async Task<Result<Model.Comment>> Get(string unicId)
        {
            if (!isAuthorize)
                return Result<Model.Comment>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            if (string.IsNullOrEmpty(unicId))
                return Result<Model.Comment>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            Guid _unicId = new Guid(unicId);

            return await _dataSource.GetAsync(unicId: _unicId);
        }
        public async Task<Result> AddInside(Model.Comment model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);
            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            model.UnicId = Guid.NewGuid();
            model.Date = DateTime.Now;

            var result = await _dataSource.AddAsync(model);
            if (!result.Success)
                return Result<Model.Comment>.Failure(message: result.Message);

            return Result.Successful();
        }
        public async Task<Result> Save(Model.Comment model, string reply)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            if (string.IsNullOrEmpty(reply))
                return Result.Failure(message: "پاسخ وارد نشده");

            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            var userDb = new UserDataSource();
            var userResult = await userDb.GetAsync(id: UserId);
            if (!userResult.Success)
                return Result.Failure(message: userResult.Message);

            var result = await _dataSource.EditAsync(model);
            if (!result.Success)
                return Result.Failure(message: result.Message);

            Model.Comment saveModel = new Model.Comment
            {
                PostId = model.PostId,
                Name = $"{userResult.Data.FirstName} {userResult.Data.LastName}",
                Mail = "admin",
                WebSite = "admin",
                Text = reply,
                ParentId = model.Id,
                Type = CommentType.منتشر_شده,
                UnicId = Guid.NewGuid(),
                Date = DateTime.Now,

            };
            await _dataSource.AddAsync(saveModel);
            return Result.Successful();
        }
        public async Task<Result> Save(Model.Comment model, Captcha captcha = null)
        {
            if (model.UnicId == Guid.Empty)
                return await Add(model, captcha);
            return await EditInside(model);
        }
        private async Task<Result> Add(Model.Comment model, Captcha captcha)
        {
            if (captcha == null)
                return Result.Successful();

            if (string.IsNullOrEmpty(captcha.Text) || !CaptchaHelper.Validate(captcha))
                return Result.Failure(message: "کد امنیتی را وارد کنید");

            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            if (model.Score == 0)
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
        public async Task<Result> EditInside(Model.Comment model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            return await _dataSource.EditAsync(model);
        }
        public async Task<Result> SetPublicAsync(Model.Comment model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            model.Type = CommentType.منتشر_شده;

            return await _dataSource.EditAsync(model);
        }
        private async Task<Result> validation(Model.Comment model)
        {
            if (model.PostId == 0)
                return Result.Failure(message: "Comment E1");

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
                    ScoreSum = scores.Sum(x => x.Score),
                });

        }

        public async Task<Result<List<Model.Comment>>> ListInside(CommentVM modelVm)
        {
            if (!isAuthorize)
                return Result<List<Model.Comment>>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            return await _dataSource.ListInsideAsync(modelVm);

        }
        public async Task<Result> Remove(long id = 0)
        {
            if (!isAuthorize)
                return Result<List<Model.Comment>>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            return await _dataSource.RemoveAsync(id);
        }
    }
}
