using CMS.Dal.DataSource;
using CMS.Model;
using CMS.Model.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CMS.Controllers
{
    [Route("api/[controller]")]
    [Tools.Authorize, ApiController]
    public class UserController : ControllerBase
    {
        public UserController(IRequestInfo requestInfo) {
            _dataSource = new UserDataSource();
            _requestInfo = requestInfo;
        }

        private readonly UserDataSource _dataSource;
        private readonly IRequestInfo _requestInfo;

        [HttpPost, Route("ChangePassword")]
        public async Task<Result> ChangePassword([FromBody] UserChangePassword model)
        {
            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.NewPassword1) || string.IsNullOrEmpty(model.NewPassword2))
                return Result.Failure(message: "رمز عبور وارد نشده");
            if (model.NewPassword1 != model.NewPassword2)
                return Result.Failure(message: "رمز عبور و تکرار آن مطابقت ندارد");

            var userResult = await _dataSource.GetByuserPassAsync(_requestInfo.UserName, model.Password);
            if (!userResult.Success)
                return Result.Failure(message: userResult.Message);
            if (userResult.Data == null)
                return Result.Failure(message: "کاربر یافت نشد");

            var checkResult = await checkPassword(model.NewPassword1);
            if (!checkResult.Success)
                return Result.Failure(message: checkResult.Message);

            userResult.Data.Password = model.NewPassword1;

            var editResult = await _dataSource.EditAsync(userResult.Data);
            if (!editResult.Success)
                return Result.Failure(message: editResult.Message);


            return Result.Successful();
        }
        private async Task<Result> checkPassword(string password)
        {
            var error = "";
            if (string.IsNullOrWhiteSpace(password)
                || password.Length < 8)
                error += $"طول رشته باید برابر {8} باشد.";
            if (!Regex.Match(password, @"^(?=.*[^\da-zA-Z])", RegexOptions.ECMAScript).Success)
                error += " رشته باید حاوی مقادیر خاص (!@#) باشد.";
            if (!Regex.Match(password, @"^(?=.*\d)", RegexOptions.ECMAScript).Success)
                error += " رشته باید حاوی عدد باشد.";
            if (!Regex.Match(password, @"^(?=.*[A-Z])", RegexOptions.ECMAScript).Success)
                error += " رشته باید حاوی حروف بزرگ باشد.";
            if (!Regex.Match(password, @"^(?=.*[a-z])", RegexOptions.ECMAScript).Success)
                error += " رشته باید حاوی حروف کوچک باشد";

            if (error == "") return Result.Successful();
            else return Result.Failure(message: error);
        }
    }
}
