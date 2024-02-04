using CMS.Model;
using CMS.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CMS.Dal.DataSource
{
    public class OrderDataSource : BaseDataSource, IDataSource
    {
        public OrderDataSource()
        {
            _pblContexts = new PblContexts();
        }
        readonly PblContexts _pblContexts;

        public async Task<Result<Order>> GetAsync(long id = 0, Guid? unicId = null)
        {
            try
            {
                var ett = await _pblContexts.Orders.Where(x =>
                   (id != 0 && x.Id == id)
                   || (id == 0 && x.UnicId == unicId)
                ).Take(1).FirstOrDefaultAsync();
                if (ett == null)
                    return Result<Order>.Successful();

                var returnMOdel = Map<Order, Dal.DbModel.Order>(ett);

                return Result<Order>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Order>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> AddAsync(Order model)
        {
            try
            {
                var ett = Map<Dal.DbModel.Order, Order>(model);
                _pblContexts.Add<Dal.DbModel.Order>(ett);
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

        public async Task<Result> EditAsync(Order model)
        {
            try
            {
                var record = await GetAsync(model.Id);
                if (!record.Success)
                    return Result.Failure(message: record.Message);
                if (record.Data == null)
                    return Result.Successful();

                var ett = Map<Dal.DbModel.Order, Order>(record.Data);

                ett.Type = (byte)record.Data.Type;
                ett.Text = record.Data.Text;

                _pblContexts.Update<Dal.DbModel.Order>(ett);
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

        public async Task<Result<IEnumerable<Order>>> ListAsync(OrderVM model)
        {
            try
            {
                var query = $"cnt.SpGetOrders"
                    + $" @Type = {((byte)model.Type).Query()}";
                var ett = await _pblContexts.Orders.FromSql(System.Runtime.CompilerServices.FormattableStringFactory.Create(query)).ToListAsync();

                var returnMOdel = MapList<Order, Dal.DbModel.Order>(ett);

                return Result<IEnumerable<Order>>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Order>>.Failure(message: ex.Message);
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
