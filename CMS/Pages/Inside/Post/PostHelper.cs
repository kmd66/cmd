﻿
using CMS.Dal;
using CMS.Dal.DataSource;
using CMS.Helper;
using CMS.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CMS.Pages.Inside.Post
{
    public class PostHelper : BaseHelper
    {
        public PostHelper(string? auth)
            : base(auth)
        {
            _dataSource = new PostDataSource();
            _menuDataSource = new MenuDataSource();
        }
        private readonly PostDataSource _dataSource;
        private readonly MenuDataSource _menuDataSource;

        public async Task<Result<Model.Post>> Get(long id = 0, string unicId = null)
        {
            if (!isAuthorize)
                return Result<Model.Post>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            Guid? _unicId = null;
            if (!string.IsNullOrEmpty(unicId))
                _unicId = new Guid(unicId);
            return await _dataSource.GetAsync(id, _unicId);
        }
        public async Task<Result<Model.Post>> Save(Model.Post model, string state)
        {
            if (model.UnicId == Guid.Empty)
                return await Add(model);
            return await Edit(model);
        }
        private async Task<Result<Model.Post>> Add(Model.Post model)
        {
            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result<Model.Post>.Failure(message: validationResult.Message);

            model.UnicId = Guid.NewGuid();
            model.Date = DateTime.Now;

            await _dataSource.AddAsync(model);

            return await _dataSource.GetAsync(0, model.UnicId);
        }
        private async Task<Result<Model.Post>> Edit(Model.Post model)
        {
            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result<Model.Post>.Failure(message: validationResult.Message);

            await _dataSource.EditAsync(model);
            return await _dataSource.GetAsync(0, model.UnicId);
        }
        private async Task<Result> validation(Model.Post model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            if (string.IsNullOrEmpty(model.Title))
                return Result.Failure(message: "نام وارد نشده");

            if (string.IsNullOrEmpty(model.Alias))
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

            var result = await _dataSource.GetAsync(model.Title);
            if (result.Data != null && result.Data.Id != model.Id)
                return Result.Failure(message: "نام تکراری است");

            var resultAlias = await _dataSource.GetByAliasAsync(model.Alias);
            if (resultAlias.Data != null && resultAlias.Data.Id != model.Id)
                return Result.Failure(message: "نام مستعار تکراری است");

            var resultMenuAlias = await _menuDataSource.GetByAliasAsync(model.Alias);
            if (resultAlias.Data != null && resultAlias.Data.Id != model.Id)
                return Result.Failure(message: "نام مستعار تکراری است");

            return Result.Successful();
        }

        public async Task<Result<List<Model.Post>>> List(PostVM model)
        {
            if (!isAuthorize)
                return Result<List<Model.Post>>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            
            var retult = await _dataSource.ListAsync(model);
            if (!retult.Success)
                return Result<List<Model.Post>>.Failure(message: retult.Message);
            return Result<List<Model.Post>>.Successful(data: retult.Data.ToList());

        }
    }
}
