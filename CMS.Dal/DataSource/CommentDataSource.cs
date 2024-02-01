using CMS.Model;
using CMS.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace CMS.Dal.DataSource
{
    public class CommentDataSource : BaseDataSource, IDataSource
    {
        public CommentDataSource()
        {
            _pblContexts = new CntContexts();
        }
        readonly CntContexts _pblContexts;

        public async Task<Result<Comment>> GetAsync(long id = 0, Guid? unicId = null)
        {
            try
            {
                var ett = await _pblContexts.Comments.Where(x =>
                   (id != 0 && x.Id == id)
                   || (id == 0 && x.UnicId == unicId)
                ).Take(1).FirstOrDefaultAsync();
                if (ett == null)
                    return Result<Comment>.Successful();

                var childs = await _pblContexts.Comments.Where(x =>
                    x.ParentId == ett.Id
                ).ToListAsync();
                var returnMOdel = Map<Comment, Dal.DbModel.Comment>(ett);
                returnMOdel.Childs = MapList<Comment, Dal.DbModel.Comment>(childs).ToList();

                return Result<Comment>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Comment>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> AddAsync(Comment model)
        {
            try
            {
                var ett = Map<Dal.DbModel.Comment, Comment>(model);
                _pblContexts.Add<Dal.DbModel.Comment>(ett);
                await _pblContexts.SaveChangesAsync();

                return Result.Successful();
            }
            catch (Exception ex)
            {
                return Result.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }
        public async Task<Result> AddScoreAsync(long commentId, int score)
        {
            try
            {
                var ett = new DbModel.Score
                {
                    UnicId = Guid.NewGuid(),
                    CommentId = commentId,
                    Value = score
                };
                _pblContexts.Add<Dal.DbModel.Score>(ett);
                await _pblContexts.SaveChangesAsync();

                return Result.Successful();
            }
            catch (Exception ex)
            {
                return Result.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> EditAsync(Comment model)
        {
            try
            {
                var record = await GetAsync(model.Id);
                if (!record.Success)
                    return Result.Failure(message: record.Message);
                if (record == null)
                    return Result.Successful();

                var ett = Map<Dal.DbModel.Comment, Comment>(model);
                
                ett.Id = record.Data.Id;
                ett.UnicId = record.Data.UnicId;

                _pblContexts.Update<Dal.DbModel.Comment>(ett);
                await _pblContexts.SaveChangesAsync();
                return Result.Successful();
            }
            catch (Exception ex)
            {
                return Result.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<IEnumerable<Comment>>> ListAsync(CommentVM modelVm)
        {
            try
            {
                //var result = from com in _pblContexts.Comments
                //    join sco in _pblContexts.Scores on com.Id equals sco.CommentId into Details
                //    from m in Details.DefaultIfEmpty()
                //    where com.PostId == modelVm.PostId
                //        && com.Type == (byte)modelVm.Type
                //    select new Comment
                //    {
                //        PostId = com.PostId,
                //        Name = com.Name,
                //        Mail = com.Mail,
                //        WebSite = com.WebSite,
                //        Text = com.Text,
                //        ParentId = com.ParentId,
                //        //Type = com.Type,
                //        Score = m.Value
                //    };
                //var etts = await result.ToListAsync();

                var query = $"cnt.SpGetComments"
                    + $" @PostId = {modelVm.PostId.Query()}"
                    + $", @Type = {((byte)modelVm.Type).Query()}";
               
                var ett = await _pblContexts.CommentDtos.FromSql(System.Runtime.CompilerServices.FormattableStringFactory.Create(query)).ToListAsync();

                var returnMOdel = MapList<Comment, Dal.DbModel.CommentDto>(ett);

                return Result<IEnumerable<Comment>>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Comment>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<List<Comment>>> ListInsideAsync(CommentVM modelVm)
        {
            try
            {
                var query = $"cnt.SpGetInsideComments"
                    + $" @Type = {((byte)modelVm.Type).Query()}"
                    + $", @Name = {modelVm.Name.Query()}"
                    + $", @PageSize = {modelVm.PageSize.Query()}"
                    + $", @PageIndex = {modelVm.PageIndex.Query()}";

                var ett = await _pblContexts.CommentDtos.FromSql(System.Runtime.CompilerServices.FormattableStringFactory.Create(query)).ToListAsync();

                var returnMOdel = MapList<Comment, DbModel.CommentDto>(ett);

                return Result<List<Comment>>.Successful(data: returnMOdel.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<Comment>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> RemoveAsync(long id = 0)
        {
            try
            {
                var items = await GetAsync(id);
                _pblContexts.Remove<Dal.DbModel.Comment>(new DbModel.Comment { Id = id });
                if (items.Data?.Childs?.Count > 0)
                {
                    var childs = MapList<Dal.DbModel.Comment, Comment>(items.Data.Childs);
                    _pblContexts.Comments.RemoveRange(childs);
                }
                await _pblContexts.SaveChangesAsync();

                return Result.Successful();
            }
            catch (Exception ex)
            {
                return Result.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

    }
}