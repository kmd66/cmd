using CMS.Dal.DataSource;
using CMS.Dal.DbModel;
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
            _dataSource = new MenuDataSource();
        }
        public bool isAuthorize { get; set; }
        private readonly MenuDataSource _dataSource;

        public Result<List<Model.Menu>> GetItems(MenuVM model)
        {
            if (!isAuthorize)
                return Result<List<Model.Menu>>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var items = Menus.GetList(model);
            return Result<List<Model.Menu>>.Successful(data: items);
        }

        public async Task<Result<Model.Menu>> GetItem(Guid unicId)
        {
            if (!isAuthorize)
                return Result<Model.Menu>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var item = Menus.List.FirstOrDefault(x=> x.UnicId == unicId);
            if(item == null)
                return Result<Model.Menu>.Failure(message: "یافت نشد");
            item.Child = Menus.addChild(new MenuVM(), item);
            return Result<Model.Menu>.Successful(data: item);
        }

        public Result ChangeIndex(bool up, Model.Menu model)
        {
            if (!isAuthorize)
                return Result<List<Model.Menu>>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            //Menus
            //var items = Menus.GetList(model);
            return Result.Successful();
        }

        public async Task<Result> EditItem(Model.Menu model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

           await  _dataSource.EditAsync(model);

            return Result.Successful();
        }
    }
}
