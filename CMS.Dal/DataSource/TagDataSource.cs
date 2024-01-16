using CMS.Model;
using CMS.Model.Interface;
using CMS.Model;
using Microsoft.EntityFrameworkCore;


namespace CMS.Dal.DataSource
{
    public class TagDataSource : BaseDataSource,IDataSource
    {
        public TagDataSource() {
            _pblContexts = new CntContexts();
        }
        readonly CntContexts _pblContexts;

        public async Task<Result<IEnumerable<Tag>>> GetAsync(long postId)
        {
            try
            {
                var ett = await _pblContexts.Tags.Where(x =>
                    x.PostId == postId
                ).ToListAsync();
                if (ett == null)
                    return Result<IEnumerable<Tag>>.Successful();

                var returnMOdel = MapList<Tag, Dal.DbModel.Tag>(ett);

                return Result<IEnumerable<Tag>>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Tag>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }
        public async Task<Result<IEnumerable<Tag>>> AddAsync(long postId, IEnumerable<Tag> tags)
        {
            try
            {
                var texts = System.Text.Json.JsonSerializer.Serialize(tags.Select(x => x.Text));
                var ett = await _pblContexts.Tags.FromSql($"cnt.SpTagAdd @PostId = {postId}, @Json = {texts}").ToListAsync();

                var returnMOdel = MapList<Tag, Dal.DbModel.Tag>(ett);

                return Result<IEnumerable<Tag>>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Tag>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }
    }
}
