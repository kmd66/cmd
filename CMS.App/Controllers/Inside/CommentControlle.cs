//using CMS.App.Components;
//using CMS.Model;
//using Microsoft.AspNetCore.Mvc;

//namespace CMS.App.Controllers
//{
//    [Route("Comment")]
//    public class CommentControlle : Controller
//    {
//        public ActionResult Index()
//        {
//            var cooki = Request.Cookies["token"];
//            if (!string.IsNullOrEmpty(cooki))
//            {
//                var claims = new Helper.JwtHelper().GetClaims(cooki);
//                if (claims == null)
//                {
//                    Response.Cookies.Delete("token");
//                }
//                else
//                {
//                    return RedirectToAction("Index", "CmdAdmin");
//                }
//            }

//            return View("~/Views/Inside/CmsLogin/Index.cshtml");
//        }

//        [HttpPost, Route("SaveComment")]
//        public async Task<Result<TokenObject>> Get([FromForm] SaveCommentModel model)
//        {
//            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
//                return Result<TokenObject>.Failure(message: "نام کاربری و رمز عبور را وارد کنید");

//            var dataSource = new Dal.DataSource.UserDataSource();
//            var userResult = await dataSource.GetByuserPassAsync(model.UserName, model.Password);
//            if (!userResult.Success)
//                return Result<TokenObject>.Failure(message: userResult.Message);
//            if (userResult.Data == null)
//                return Result<TokenObject>.Failure(message: "کاربر یافت نشد");


//            var resultClaims = await getClaims(userResult.Data);
//            if (!resultClaims.Success)
//                return Result<TokenObject>.Failure(message: resultClaims.Message);
//            var claims = resultClaims.Data;

//            var expireDate = DateTime.Now.AddHours(Property.AccessTokenExpireTimeSpan);
//            var issuedDate = DateTime.Now;

//            var token = GetToken(claims, issuedDate, expireDate);

//            //var ds2 = claims2.Where(x => x.Type == "RefreshTokenID").ToList();
            
//            CookieOptions options = new CookieOptions();
//            options.Expires = expireDate;
//            Response.Cookies.Append("token", $"{token.AccessToken}", options);
//            token.AccessToken = "";
//            return Result<TokenObject>.Successful(data: token);
//        }
//    }
//}
