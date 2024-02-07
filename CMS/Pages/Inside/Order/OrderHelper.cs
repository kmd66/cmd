
using CMS.Dal;
using CMS.Dal.DataSource;
using CMS.Helper;
using CMS.Model;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace CMS.Pages.Inside.Order
{
    public class OrderHelper : BaseHelper
    {
        public OrderHelper(string? auth)
            : base(auth)
        {
            _dataSource = new OrderDataSource();
            _dataSourcePost = new CMS.Dal.DataSource.PostDataSource();
            _dataSourceProduct = new CMS.Dal.DataSource.ProductDataSource();
        }
        private readonly OrderDataSource _dataSource;
        private readonly PostDataSource _dataSourcePost;
        private readonly ProductDataSource _dataSourceProduct;

        public async Task<Result<Model.Order>> Get(string unicId)
        {
            if (!isAuthorize)
                return Result<Model.Order>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            if (string.IsNullOrEmpty(unicId))
                return Result<Model.Order>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            Guid _unicId =  new Guid(unicId);

            return await _dataSource.GetAsync(unicId: _unicId);
        }
        public async Task<Result<OrderGet>> GetyByTrackingCode(string trackingCode)
        {
            if (string.IsNullOrEmpty(trackingCode))
                return Result<OrderGet>.Failure(message: "موردی یافت نشد");
            return await _dataSource.GetyByTrackingCodeAsync(trackingCode);
        }
        public async Task<Result<string>> Add(Model.Order model, Captcha captcha, List<string> basket)
        {
            if (basket == null || basket.Count == 0)
                return Result<string>.Successful(data: "bsk e1");

            var captchaResult = await CaptchaCheck(captcha);
            if (!captchaResult.Success)
                return Result<string>.Failure(message: captchaResult.Message);

            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result<string>.Failure(message: validationResult.Message);

            model.UnicId = Guid.NewGuid();
            model.Date = DateTime.Now;
            model.TrackingCode = Guid.NewGuid().String().Substring(0, 10);
            model.Text = "";
            model.SetBasket(basket);

            var result = await _dataSource.AddAsync(model);
            if (!result.Success)
                return Result<string>.Failure(message: result.Message);

            return Result<string>.Successful(data: model.TrackingCode);
        }
        private async Task<Result> CaptchaCheck(Captcha captcha)
        {
            if (captcha == null)
                return Result.Failure(message: "کد امنیتی را وارد کنید");

            if (string.IsNullOrEmpty(captcha.Code) || !CaptchaHelper.Validate(captcha))
                return Result.Failure(message: "کد امنیتی را وارد کنید");

            return Result.Successful();
        }
        public async Task<Result> Edit(Model.Order model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            return await _dataSource.EditAsync(model);
        }
        private async Task<Result> validation(Model.Order model)
        {
            if (string.IsNullOrEmpty(model.FirstName))
                return Result.Failure(message: "نام را وارد نشده");

            if (string.IsNullOrEmpty(model.LastName))
                return Result.Failure(message: "نام خانوادگی را وارد نشده");

            if (string.IsNullOrEmpty(model.Mail)) 
                model.Mail = "";
            else if (!new EmailAddressAttribute().IsValid(model.Mail))
                return Result.Failure(message: "ایمیل را صحیح وارد کنید");

            if (string.IsNullOrEmpty(model.Mobile))
                return Result.Failure(message: "شماره تماس را وارد نشده");

            if (string.IsNullOrEmpty(model.Address))
                return Result.Failure(message: "آدرس را وارد نشده");

            if (string.IsNullOrEmpty(model.PostalCode))
                model.PostalCode = "";

            model.FirstName = model.FirstName.Xss();
            model.LastName = model.LastName.Xss();
            model.Mail = model.Mail.Xss();
            model.Mobile = model.Mobile.Xss();
            model.Address= model.Address.Xss();
            model.PostalCode = model.PostalCode.Xss();

            if (model.Type == OrderType.Unknown)
                model.Type = OrderType.سفارش;

            return Result.Successful();
        }

        public async Task<Result<List<Model.Order>>> List(OrderVM model)
        {
            if (!isAuthorize)
                return Result<List<Model.Order>>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            return await _dataSource.ListAsync(model);
        }

        public async Task<Result> Remove(long id = 0)
        {
            if (!isAuthorize)
                return Result<List<Model.Order>>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            return await _dataSource.RemoveAsync(id);
        }
    }
}
