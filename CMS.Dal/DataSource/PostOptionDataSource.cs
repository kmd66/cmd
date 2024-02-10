using CMS.Model;
using CMS.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CMS.Dal.DataSource
{
    public class PostOptionDataSource : BaseDataSource, IDataSource
    {
        public PostOptionDataSource()
        {
            _pblContexts = new CntContexts();
        }
        readonly CntContexts _pblContexts;

        public async Task<Result<PostOption>> GetAsync(long id = 0, Guid? unicId = null)
        {
            try
            {
                var ett = await _pblContexts.PostOptions.Where(x =>
                   (id != 0 && x.Id == id)
                   || (id == 0 && x.UnicId == unicId)
                ).Take(1).FirstOrDefaultAsync();
                if (ett == null)
                    return Result<PostOption>.Successful();

                var returnMOdel = Map<PostOption, Dal.DbModel.PostOption>(ett);

                return Result<PostOption>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<PostOption>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> AddAsync(PostOption model)
        {
            try
            {
                var ett = Map<Dal.DbModel.PostOption, PostOption>(model);
                _pblContexts.Add<Dal.DbModel.PostOption>(ett);
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

        public async Task<Result> EditAsync(PostOption model)
        {
            try
            {
                var record = await GetAsync(model.Id);
                if (!record.Success)
                    return Result.Failure(message: record.Message);
                if (record.Data == null)
                    return Result.Successful();

                var ett = Map<Dal.DbModel.PostOption, PostOption>(model);

                ett.Id = record.Data.Id;
                ett.UnicId = record.Data.UnicId;
                
                _pblContexts.Update<Dal.DbModel.PostOption>(ett);
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
