//using CMS.Dal.DataSource;
//using CMS.Model;

//namespace CMS.Pages.Inside.Setting
//{
//    public class OptinHelper : CMS.Helper.BaseHelper
//    {
//        public OptinHelper(string? auth)
//            : base(auth)
//            => _dataSource = new OptionDataSource();
//        public OptinHelper(bool auth)
//            : base(auth)
//            => _dataSource = new OptionDataSource();

//        private readonly OptionDataSource _dataSource;
//        public async Task<Result> Add(List<Option> model)
//        {
//            if (!isAuthorize)
//                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);
//            return await _dataSource.AddAsync(model);
//        }
//    }
//}
