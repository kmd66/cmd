
using CMS.Dal;
using CMS.Dal.DataSource;
using CMS.Helper;
using CMS.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CMS.Pages.Inside.Post
{
    public class ProductHelper : BaseHelper
    {
        public ProductHelper(string? auth)
            : base(auth)
        {
            _dataSource = new ProductDataSource();
        }
        private readonly ProductDataSource _dataSource;

        public async Task<Result<Model.Product>> Get(Guid unicId)
        {
            var result =  await _dataSource.GetAsync(0, unicId);
            if (!result.Success)
                return Result<Model.Product>.Failure(message: result.Message);
            
            if (result.Data == null)
                result.Data = new Product();

            return Result<Model.Product>.Successful(data: result.Data);
        }
        public async Task<Result<Model.Product>> Save(Model.Product model, Model.Post postModel)
        {
            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result<Model.Product>.Failure(message: validationResult.Message);

            var result = await _dataSource.GetAsync(0, model.UnicId);
            if (!result.Success)
                return Result<Model.Product>.Failure(message: result.Message);

            if (result.Data == null)
                return await Add(model, postModel);
            return await Edit(model);
        }
        private async Task<Result<Model.Product>> Add(Model.Product model, Model.Post postModel)
        {
            model.UnicId = postModel.UnicId;
            await _dataSource.AddAsync(model);
            return await _dataSource.GetAsync(0, model.UnicId);
        }
        private async Task<Result<Model.Product>> Edit(Model.Product model)
        {
            await _dataSource.EditAsync(model);
            return await _dataSource.GetAsync(0, model.UnicId);
        }
        private async Task<Result> validation(Model.Product model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);
            
            if (model.Type == ProductType.Unknown)
                return Result.Failure(message: "نوع سفارش وارد نشده");
            
            return Result.Successful();
        }

    }
}
