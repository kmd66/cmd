﻿using CMS.Model;
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
        public async Task<Result<OrderGet>> GetyByTrackingCodeAsync(string trackingCode)
        {
            try
            {
                var ett = await _pblContexts.Orders.Where(x =>
                    x.TrackingCode == trackingCode
                ).Take(1).FirstOrDefaultAsync();
               
                if (ett == null)
                    return Result<OrderGet>.Failure(message: "موردی یافت نشد");

                var returnMOdel = Map<OrderGet, Dal.DbModel.Order>(ett);

                var query = $"pbl.SpGetOrderPost"
                    + $" @TrackingCode = {trackingCode.Query()}";
                var orderPosts = await _pblContexts.OrderPost.FromSql(System.Runtime.CompilerServices.FormattableStringFactory.Create(query)).ToListAsync();

                var orderPostMOdel = MapList<OrderPost, Dal.DbModel.OrderPost>(orderPosts);

                returnMOdel.posts = orderPostMOdel.ToList();

                return Result<OrderGet>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<OrderGet>.Failure(message: ex.Message);
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

                ett.Type = (byte)model.Type;
                ett.Text = model.Text;

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

        public async Task<Result<List<Order>>> ListAsync(OrderVM model)
        {
            try
            {
                var query = $"pbl.SpGetOrders"
                    + $" @Type = {((byte)model.Type).Query()}"
                    + $", @FirstName = {model.FirstName.Query()}"
                    + $", @LastName = {model.LastName.Query()}"
                    + $", @TrackingCode = {model.TrackingCode.Query()}"
                    + $", @PageSize = {model.PageSize.Query()}"
                    + $", @PageIndex = {model.PageIndex.Query()}";
                var ett = await _pblContexts.OrderDto.FromSql(System.Runtime.CompilerServices.FormattableStringFactory.Create(query)).ToListAsync();

                var returnMOdel = MapList<Order, Dal.DbModel.OrderDto>(ett);

                return Result<List<Order>>.Successful(data: returnMOdel.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<Order>>.Failure(message: ex.Message);
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
