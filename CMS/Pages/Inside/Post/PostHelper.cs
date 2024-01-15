
using CMS.Dal.DataSource;
using CMS.Helper;
using CMS.Model;
using System.Text.RegularExpressions;

namespace CMS.Pages.Inside.Post
{
    public class PostHelper : BaseHelper
    {
        public PostHelper(string? auth)
            :base(auth)
        {
        }
        public async Task<Result> Save(Model.Post model, string state)
        {
            if (state == "add")
                return await Add(model);
            return await Edit(model);
        }
        private async Task<Result> Add(Model.Post model)
        {
            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            model.UnicId = Guid.NewGuid();

            return Result.Successful();
        }
        private async Task<Result> Edit(Model.Post model)
        {
            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            return Result.Successful();
        }
        private async Task<Result> validation(Model.Post model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            if (string.IsNullOrEmpty(model.Title))
                return Result.Failure(message: "نام وارد نشده");

            if (string.IsNullOrEmpty(model.Summary))
                return Result.Failure(message: "نام مستعار وارد نشده");

            if (string.IsNullOrEmpty(model.Content))
                return Result.Failure(message: "متن وارد نشده");

            string noHTML = Regex.Replace(model.Content, "<.*?>", "").Trim();

            if (string.IsNullOrEmpty(noHTML))
                return Result.Failure(message: "متن وارد نشده");

            if (model.Access == PostAccessType.Unknown)
                return Result.Failure(message: "دسترسی وارد نشده");

            if (string.IsNullOrEmpty(model.Summary))
            {
                model.Summary = noHTML.Substring(0, Math.Min(noHTML.Length, 50));
            }

            //    var result = await _dataSource.GetAsync(model.Name);
            //if (result.Data != null && result.Data.Id != model.Id)
            //    return Result.Failure(message: "نام تکراری است");

            //var resultAlias = await _dataSource.GetByAliasAsync(model.Alias);
            //if (resultAlias.Data != null && resultAlias.Data.Id != model.Id)
            //    return Result.Failure(message: "نام مستعار تکراری است");

            return Result.Successful();
        }
    }
}
