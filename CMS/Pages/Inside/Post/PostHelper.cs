
using CMS.Dal.DataSource;
using CMS.Helper;
using CMS.Model;

namespace CMS.Pages.Inside.Post
{
    public class PostHelper : BaseHelper
    {
        public PostHelper(string? auth)
            :base(auth)
        {
        }
        public async Task<Result> Save(Model.Post model, string state)
        {
            if (state == "add")
                return await Add(model);
            return await Edit(model);
        }
        private async Task<Result> Add(Model.Post model)
        {

            return Result.Successful();
        }
        private async Task<Result> Edit(Model.Post model)
        {

            return Result.Successful();
        }
    }
}
