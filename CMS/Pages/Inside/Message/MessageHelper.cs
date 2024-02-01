
using CMS.Dal;
using CMS.Dal.DataSource;
using CMS.Helper;
using CMS.Model;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CMS.Pages.Inside.Message
{
    public class MessageHelper : BaseHelper
    {
        public MessageHelper(string? auth/*, string? remoteIpAddress*/)
            : base(auth)
        {
            _dataSource = new MessageDataSource();
        }
        private readonly MessageDataSource _dataSource;

        public async Task<Result<Model.Message>> Get(string unicId)
        {
            if (!isAuthorize)
                return Result<Model.Message>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            if (string.IsNullOrEmpty(unicId))
                return Result<Model.Message>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            Guid _unicId =  new Guid(unicId);

            return await _dataSource.GetAsync(unicId: _unicId);
        }
        public async Task<Result> Add(Model.Message model, Captcha captcha)
        {
            if(captcha == null)
                return Result.Successful();

            if(string.IsNullOrEmpty(captcha.Code) || !CaptchaHelper.Validate(captcha))
                return Result.Failure(message: "کد امنیتی را وارد کنید");

            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            model.UnicId = Guid.NewGuid();
            model.Date = DateTime.Now;

            return await _dataSource.AddAsync(model);
        }
        private async Task<Result> validation(Model.Message model)
        {
            if (string.IsNullOrEmpty(model.Text))
                return Result.Failure(message: "متن را وارد نشده");

            if (string.IsNullOrEmpty(model.Name))
                return Result.Failure(message: "نام را وارد نشده");

            if (string.IsNullOrEmpty(model.Phone))
                return Result.Failure(message: "شماره تماس را وارد نشده");

            if (string.IsNullOrEmpty(model.Mail))
                model.Mail ="";
            else if (!new EmailAddressAttribute().IsValid(model.Mail))
                return Result.Failure(message: "ایمیل را صحیح وارد کنید");


            model.Text = model.Text.Xss();
            model.Name = model.Name.Xss();
            model.Mail = model.Mail.Xss();
            model.Phone = model.Phone.Xss();

            if (model.Type == MessageType.Unknown)
                model.Type = MessageType.خوانده_نشده;

            return Result.Successful();
        }

        public async Task<Result<List<Model.Message>>> List(MessageVM modelVm)
        {
            if (!isAuthorize)
                return Result<List<Model.Message>>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            return await _dataSource.ListAsync(modelVm);

        }
        public async Task<Result> SetType(long id, MessageType type)
        {
            if (!isAuthorize)
                return Result<List<Model.Message>>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            return await _dataSource.SetTypeAsync(id, type);
        }
        public async Task<Result> Remove(long id = 0)
        {
            if (!isAuthorize)
                return Result<List<Model.Comment>>.Failure(message: Property.MsgUnUnauthorized, code: 401);
            return await _dataSource.RemoveAsync(id);
        }
    }
}
