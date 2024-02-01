using CMS.Model;
using CMS.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CMS.Dal.DataSource
{
    public class MessageDataSource : BaseDataSource, IDataSource
    {
        public MessageDataSource()
        {
            _pblContexts = new PblContexts();
        }
        readonly PblContexts _pblContexts;

        public async Task<Result<Message>> GetAsync(long id = 0, Guid? unicId = null)
        {
            try
            {
                var ett = await _pblContexts.Messages.Where(x =>
                   (id != 0 && x.Id == id)
                   || (id == 0 && x.UnicId == unicId)
                ).Take(1).FirstOrDefaultAsync();
                if (ett == null)
                    return Result<Message>.Successful();
                var returnMOdel = Map<Message, Dal.DbModel.Message>(ett);

                return Result<Message>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Message>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> AddAsync(Message model)
        {
            try
            {
                var ett = Map<Dal.DbModel.Message, Message>(model);
                _pblContexts.Add<Dal.DbModel.Message>(ett);
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
        public async Task<Result> SetTypeAsync(long id, MessageType type)
        {
            try
            {
                var ett = await GetAsync(id);
                if (ett.Data == null)
                    return Result<Message>.Successful();

                ett.Data.Type = type;
                var model = Map<Dal.DbModel.Message, Message>(ett.Data);
                _pblContexts.Update<Dal.DbModel.Message>(model);
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

        public async Task<Result<List<Message>>> ListAsync(MessageVM modelVm)
        {
            try
            {
                //if (modelVm.PageIndex.Default() == 0)
                //    modelVm.PageIndex = 1;
                //if (modelVm.PageSize.Default() == 0)
                //    modelVm.PageSize = 10;

                //var t = modelVm.PageIndex.Default() - (1 * modelVm.PageSize.Default() -1);
                var type = (byte)modelVm.Type;
                //var ett = await _pblContexts.Messages.Where(x =>
                //   (type == 0 || x.Type == type)
                //   && (modelVm.Name == null || x.Name== modelVm.Name)
                //)
                //    .OrderBy(x => x.Id)
                //    .Skip(t<0?0: t+1)
                //    .Take(modelVm.PageSize.Default()).ToListAsync();

                var query = $"pbl.SpGetMessages"
                    + $" @Type = {((byte)modelVm.Type).Query()}"
                    + $", @Name = {modelVm.Name.Query()}"
                    + $", @PageSize = {modelVm.PageSize.Query()}"
                    + $", @PageIndex = {modelVm.PageIndex.Query()}";

                var ett = await _pblContexts.Messages.FromSql(System.Runtime.CompilerServices.FormattableStringFactory.Create(query)).ToListAsync();


                if (ett == null)
                    return Result<List<Message>>.Successful(data: new List<Message>());
    
                var returnMOdel = MapList<Message, Dal.DbModel.Message>(ett).ToList();

                if(returnMOdel.Count > 0)
                {
                    var c = await _pblContexts.Messages.Where(x =>
                       (type == 0 || x.Type == type)
                       && (modelVm.Name == null || x.Name == modelVm.Name)
                    ).CountAsync();

                    returnMOdel[0].Count = c;
                }
                return Result<List<Message>>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<List<Message>>.Failure(message: ex.Message);
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
                _pblContexts.Remove<Dal.DbModel.Message>(new DbModel.Message { Id = id });
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