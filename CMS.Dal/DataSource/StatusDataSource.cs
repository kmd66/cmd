using CMS.Model;
using CMS.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CMS.Dal.DataSource
{
    public class StatusDataSource : BaseDataSource, IDataSource
    {
        public StatusDataSource()
        {
            _contexts = new PblContexts();
        }
        readonly PblContexts _contexts;

        public async Task<Result<Status>> GetAsync()
        {
            try
            {
                var query = $"pbl.SpGetStatus";
                var ett = await _contexts.Status.FromSql(System.Runtime.CompilerServices.FormattableStringFactory.Create(query)).ToListAsync();
                if (ett == null || ett.Count == 0)
                    return Result<Status>.Successful(data: new Status());

                var returnMOdel = Map<Status, Dal.DbModel.Status>(ett.First());

                return Result<Status>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Status>.Failure(message: ex.Message);
            }
            finally
            {
                _contexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<List<Order>>> GetOrderStatusAsync()
        {
            try
            {
                var query = $"pbl.SpGetOrderStatus";
                var ett = await _contexts.StatusTypes.FromSql(System.Runtime.CompilerServices.FormattableStringFactory.Create(query)).ToListAsync();

                var returnMOdel = MapList<Order, Dal.DbModel.StatusType>(ett);

                return Result<List<Order>>.Successful(data: returnMOdel.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<Order>>.Failure(message: ex.Message);
            }
            finally
            {
                _contexts.ChangeTracker.Clear();
            }
        }

    }
}
