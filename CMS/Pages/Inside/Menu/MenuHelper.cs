using CMS.Helper;
using CMS.Model;

namespace CMS.Pages.Inside.Menu
{
    public class MenuHelper
    {
        public MenuHelper(string? auth)
        {
            if (auth != null)
            {
                var claims = new JwtHelper().GetClaims(auth);
                if (claims != null)
                    isAuthorize = true;
            }
        }
        public bool isAuthorize { get; set; }

        public Result<List<Model.Menu>> GetItems(MenuVM model)
        {
            if (!isAuthorize)
                return Result<List<Model.Menu>>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var items = Menus.GetList(model);
            return Result<List<Model.Menu>>.Successful(data: items);
        }
    }
}
