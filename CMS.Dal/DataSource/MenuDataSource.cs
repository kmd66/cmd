using CMS.Model;
using CMS.Model.Interface;
using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


namespace CMS.Dal.DataSource
{
    public class MenuDataSource : BaseDataSource,IDataSource
    {
        public MenuDataSource() {
            _pblContexts = new PblContexts();
        }
        readonly PblContexts _pblContexts;

        public async Task<Result<Menu>> GetAsync(long id = 0, Guid? unicId = null)
        {
            try
            {
                var ett = await _pblContexts.Menus.SingleOrDefaultAsync(x =>
                   (id != 0 && x.Id == id)
                   || (id == 0 && x.UnicId == unicId)
                );
                if (ett == null)
                    return Result<Menu>.Successful();

                var returnMOdel = Map<Menu, Dal.DbModel.Menu>(ett);

                return Result<Menu>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Menu>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> AddAsync(Menu model)
        {
            try
            {
                var ett = Map<Dal.DbModel.Menu, Menu>(model);
                _pblContexts.Add<Dal.DbModel.Menu>(ett);
                await _pblContexts.SaveChangesAsync();
               
                return await ListAsync();
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
        public async Task<Result> EditAsync(Menu model)
        {
            try
            {
                var ett = Map<Dal.DbModel.Menu, Menu>(model);
                _pblContexts.Update<Dal.DbModel.Menu>(ett);
                await _pblContexts.SaveChangesAsync();
               
                return await ListAsync();
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
                _pblContexts.Remove<Dal.DbModel.Menu>(new DbModel.Menu { Id = id });
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

        public async Task<Result> ListAsync()
        {
            try
            {
                var ett = await _pblContexts.Menus.ToListAsync();

                Menus.List = MapList<Menu, DbModel.Menu>(ett).ToList();
                Menus.List.OrderBy(x => x.Order);
                return Result.Successful();
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Menu>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }
    }
}


/*
 
        var _ds = new CMS.Dal.DataSource.MenuDataSource();
        var model = new Model.Menu
            {
                UnicId = Guid.NewGuid(),
                FirstName = "f1",
                LastName = "l1",
                MenuName = "u1",
                Password = "p1"
            };
        var m1 = await _ds.AddAsync(model);
        m1.Data.Password = "p1-1";
        await _ds.EditAsync(m1.Data);

        var m2 = await _ds.AddAsync(new Model.Menu
            {
                UnicId = Guid.NewGuid(),
                FirstName = "f2",
                LastName = "l2",
                MenuName = "u2",
                Password = "p2"
            });

        var m3 = await _ds.GetAsync(m2.Data.Id);
        var m4 = await _ds.ListAsync(new Model.MenuVM());
        var m5 = await _ds.ListAsync(new Model.MenuVM { MenuName = "1" });
        var m = await _ds.RemoveAsync(m2.Data.Id);
 */