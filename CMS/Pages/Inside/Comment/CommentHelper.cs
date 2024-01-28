
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

        public async Task<Result> Save(Model.Comment model)
        {
            if (model.UnicId == Guid.Empty)
                return await Add(model);
            return await Edit(model);
        }
        private async Task<Result> Add(Model.Comment model)
        {
            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result<Model.Comment>.Failure(message: validationResult.Message);

            model.UnicId = Guid.NewGuid();
            model.Date = DateTime.Now;

            await _dataSource.AddAsync(model);

            return await _dataSource.GetAsync(0, model.UnicId);
        }
        private async Task<Result> Edit(Model.Comment model)
        {
            if (!isAuthorize)
                return Result<Model.Comment>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result<Model.Comment>.Failure(message: validationResult.Message);

            await _dataSource.EditAsync(model);
            return await _dataSource.GetAsync(0, model.UnicId);
        }
        private async Task<Result> validation(Model.Comment model)
        {
            if (string.IsNullOrEmpty(model.Text))
                return Result.Failure(message: "متن وارد نشده");

            if (string.IsNullOrEmpty(model.Name))
                return Result.Failure(message: "نام وارد نشده");

            if (string.IsNullOrEmpty(model.Mail))
                return Result.Failure(message: "ایمیل وارد نشده");

            if (!new EmailAddressAttribute().IsValid(model.Mail))
                return Result.Failure(message: "ایمیل صحیح وارد کنید");

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

        public async Task<Result<List<Model.Comment>>> List(CommentVM modelVm)
        {

            var retult = await _dataSource.ListAsync(modelVm);
            if (!retult.Success)
                return Result<List<Model.Comment>>.Failure(message: retult.Message);
            var model = retult.Data.ToList();
            var list = new List<Model.Comment>();

            foreach (var item in model)
            {
                if (item.ParentId == 0)
                {
                    item.Childs = model.Where(x => x.ParentId == item.Id).ToList();
                    item.Count = model.Count;
                    list.Add(item);
                }
            }
            return Result<List<Model.Comment>>.Successful(data: list);

        }
    }
}
