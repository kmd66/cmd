
using CMS.Dal;
using CMS.Dal.DataSource;
using CMS.Helper;
using CMS.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CMS.Pages.Inside.Post
{
    public class PostOptionHelper : BaseHelper
    {
        public PostOptionHelper(string? auth)
            : base(auth)
        {
            _dataSource = new PostOptionDataSource();
        }
        private readonly PostOptionDataSource _dataSource;

        public async Task<Result<Model.PostOption>> Get(Guid unicId)
        {
            if (!isAuthorize)
                return Result<Model.PostOption>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            Guid? _unicId = null;

            var result = await _dataSource.GetAsync(0, unicId);
            if (!result.Success)
                return Result<Model.PostOption>.Failure(message: result.Message);

            if (result.Data == null)
                result.Data = new PostOption();

            return Result<Model.PostOption>.Successful(data: result.Data);

        }
        public async Task<Result<Model.PostOption>> Save(Model.PostOption model)
        {
            if (!isAuthorize)
                return Result<Model.PostOption>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            if (model.Id == 0)
                return await Add(model);
            return await Edit(model);
        }
        private async Task<Result<Model.PostOption>> Add(Model.PostOption model)
        {
            await _dataSource.AddAsync(model);
            return await _dataSource.GetAsync(0, model.UnicId);
        }

        private async Task<Result<Model.PostOption>> Edit(Model.PostOption model)
        {
            await _dataSource.EditAsync(model);
            return await _dataSource.GetAsync(0, model.UnicId);
        }
    }
}
