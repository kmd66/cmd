using CMS.Model;
using CMS.Model.Interface;
using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace CMS.Dal.DataSource
{
    public class PostDataSource : BaseDataSource,IDataSource
    {
        public PostDataSource() {
            _pblContexts = new PblContexts();
        }
        readonly PblContexts _pblContexts;

        public async Task<Result<Post>> GetAsync(long id = 0, Guid? unicId = null)
        {
            try
            {
                var ett = await _pblContexts.Posts.SingleOrDefaultAsync(x =>
                   (id != 0 && x.Id == id)
                   || (id == 0 && x.UnicId == unicId)
                );
                if (ett == null)
                    return Result<Post>.Successful();

                var returnMOdel = Map<Post, Dal.DbModel.Post>(ett);

                return Result<Post>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Post>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<Post>> GetAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return Result<Post>.Successful();
                var ett = await _pblContexts.Posts.SingleOrDefaultAsync(x =>
                    x.Title == name
                );
                if (ett == null)
                    return Result<Post>.Successful();

                var returnMOdel = Map<Post, Dal.DbModel.Post>(ett);

                return Result<Post>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Post>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }
        public async Task<Result<Post>> GetByAliasAsync(string alias)
        {
            try
            {
                if (string.IsNullOrEmpty(alias))
                    return Result<Post>.Successful();
                var ett = await _pblContexts.Posts.SingleOrDefaultAsync(x =>
                    x.Alias == alias
                );
                if (ett == null)
                    return Result<Post>.Successful();

                var returnMOdel = Map<Post, Dal.DbModel.Post>(ett);

                return Result<Post>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Post>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> AddAsync(Post model)
        {
            try
            {
                var ett = Map<Dal.DbModel.Post, Post>(model);
                _pblContexts.Add<Dal.DbModel.Post>(ett);
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
        public async Task<Result> EditAsync(Post model)
        {
            try
            {
                var record = await GetAsync(model.Id);
                if(!record.Success)
                    return Result.Failure(message: record.Message);
                if (record == null)
                    return Result.Successful();

                var ett = Map<Dal.DbModel.Post, Post>(model);
               
                ett.Id = record.Data.Id;
                ett.UnicId = record.Data.UnicId;
                ett.Date = record.Data.Date;
                ett.Hit = record.Data.Hit;
                
                _pblContexts.Update<Dal.DbModel.Post>(ett);
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

        public async Task<Result> RemoveAsync(long id = 0)
        {
            try
            {
                _pblContexts.Remove<Dal.DbModel.Post>(new DbModel.Post { Id = id });
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
