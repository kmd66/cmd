using CMS.Dal.DataSource;
using CMS.Dal.DbModel;
using CMS.Helper;
using CMS.Model;
using CMS.Model.Files;

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

        public async Task<Result> Save(Model.Menu model, string state)
        {
            if (state == "add")
                return await Add(model);
            return await Edit(model);
        }
        private async Task<Result> Add(Model.Menu model)
        {
            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            model.UnicId = Guid.NewGuid();
            model.Order = Menus.List.Count + 10;

            var result = await _dataSource.AddAsync(model);
            if (!result.Success)
                return Result.Failure(message: result.Message);

            await SetOrder(model.Parent);

            return Result.Successful();
        }
        private async Task<Result> Edit(Model.Menu model)
        {
            var validationResult = await validation(model);
            if (!validationResult.Success)
                return Result.Failure(message: validationResult.Message);

            var result = await _dataSource.EditAsync(model);
            if (!result.Success)
                return Result.Failure(message: result.Message);

            return Result.Successful();
        }
        public async Task<Result> SetOrder(Model.Menu model, bool up)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var items = Menus.GetChild(new MenuVM() { ParentId = model.Parent });
            int index = items.FindIndex(x=> x.UnicId == model.UnicId);
            var item = items[index];

            if ((!up && index + 1 >= items.Count) || (up && index < 1))
                return Result.Successful();
          
            if (up)
                index--;
            else
                index++;
            
            items.Remove(item);
            items.Insert(index, item);
            foreach (var i in items)
            {
                i.Order = items.IndexOf(i) + 1;
            }

            await _dataSource.SetOrder(items);
            return Result.Successful();

        }
        private async Task SetOrder(Guid parent)
        {
            var items = Menus.GetChild(new MenuVM() { ParentId = parent });
            foreach (var item in items)
            {
                item.Order = items.IndexOf(item) + 1;
            }

            await _dataSource.SetOrder(items);

        }

        private async Task<Result> validation(Model.Menu model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            if (string.IsNullOrEmpty(model.Name))
                return Result.Failure(message: "نام وارد نشده");

            if (string.IsNullOrEmpty(model.Alias))
                return Result.Failure(message: "نام مستعار وارد نشده");

            if (model.Type == MenuType.Unknown)
                return Result.Failure(message: "نوع وارد نشده");

            if (model.Type == MenuType.Link && string.IsNullOrEmpty(model.Link))
                return Result.Failure(message: "لینک وارد نشده");

            if (model.Type == MenuType.Content && model.PostId == Guid.Empty)
                return Result.Failure(message: "مطلب انتخاب نشده");

            var result = await _dataSource.GetAsync(model.Name);
            if (result.Data != null)
                return Result.Failure(message: "نام تکراری است");

            return Result.Successful();
        }

        public Result<List<Model.Menu>> GetItems(MenuVM model)
        {
            if (!isAuthorize)
                return Result<List<Model.Menu>>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var items = Menus.GetList(model);
            return Result<List<Model.Menu>>.Successful(data: items);
        }

        public Result<List<Model.Menu>> GetList2(MenuVM modelVM, string extraName, Guid? ignore = null)
        {
            if (!isAuthorize)
                return Result<List<Model.Menu>>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var items = Menus.GetList2(modelVM, extraName);
            return Result<List<Model.Menu>>.Successful(data: items);
        }

        public async Task<Result<Model.Menu>> GetItem(Guid unicId)
        {
            if (!isAuthorize)
                return Result<Model.Menu>.Failure(message: Property.MsgUnUnauthorized, code: 401);

            var item = Menus.List.FirstOrDefault(x=> x.UnicId == unicId);
            if(item == null)
                return Result<Model.Menu>.Failure(message: "یافت نشد");
            var menu = Model.Menu.Instance(item);
            menu.Child = Menus.addChild(new MenuVM(), menu);
            return Result<Model.Menu>.Successful(data: menu);
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
