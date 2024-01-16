
using CMS.Dal;
using CMS.Dal.DataSource;
using CMS.Helper;
using CMS.Model;
using System.Text.RegularExpressions;

namespace CMS.Pages.Inside.Post
{
    public class TagHelper : BaseHelper
    {
        public TagHelper(string? auth)
            : base(auth)
        {
            _dataSource = new TagDataSource();
        }
        private readonly TagDataSource _dataSource;

        public async Task<Result<List<Tag>>> Get(long id)
        {
            var result = await _dataSource.GetAsync(id);
            if (!result.Success)
                return Result<List<Tag>>.Failure(message: result.Message);
            return Result<List<Tag>>.Successful(data: result.Data.ToList());
        }
        public async Task<Result> Add(long id, List<Tag> model)
        {
            if (!isAuthorize)
                return Result.Failure(message: Property.MsgUnUnauthorized, code: 401);

            return await _dataSource.AddAsync(id, model);
        }
    }
}
