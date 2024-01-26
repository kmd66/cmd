using CMS.Model;
using CMS.Model.Interface;
using CMS.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


namespace CMS.Dal.DataSource
{
    public class UserDataSource : BaseDataSource,IDataSource
    {
        public UserDataSource() {
            _pblContexts = new PblContexts();
        }
        readonly PblContexts _pblContexts;

        public async Task<Result<User>> GetAsync(long id = 0, Guid? unicId = null)
        {
            try
            {
                var ett = await _pblContexts.Users.Where(x =>
                   (id != 0 && x.Id == id)
                   || (id == 0 && x.UnicId == unicId)
                ).Take(1).FirstOrDefaultAsync();
                if (ett == null)
                    return Result<User>.Successful();

                var returnMOdel = Map<User, Dal.DbModel.User>(ett);

                return Result<User>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<User>> GetByuserPassAsync(string user , string pass)
        {
            try
            {
                var ett = await _pblContexts.Users.Where(x =>
                   user == x.UserName && pass == x.Password
                ).FirstOrDefaultAsync();
                if (ett == null)
                    return Result<User>.Successful();

                var returnMOdel = Map<User, Dal.DbModel.User>(ett);

                return Result<User>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result<User>> AddAsync(User model)
        {
            try
            {
                var ett = Map<Dal.DbModel.User, User>(model);
                _pblContexts.Add<Dal.DbModel.User>(ett);
                await _pblContexts.SaveChangesAsync();
               
                return await GetAsync(0, ett.UnicId);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }
        public async Task<Result<User>> EditAsync(User model)
        {
            try
            {
                var ett = Map<Dal.DbModel.User, User>(model);
                _pblContexts.Update<Dal.DbModel.User>(ett);
                await _pblContexts.SaveChangesAsync();
               
                return await GetAsync(0, ett.UnicId);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure(message: ex.Message);
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
                _pblContexts.Remove<Dal.DbModel.User>(new DbModel.User { Id = id });
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

        public async Task<Result<IEnumerable<User>>> ListAsync(UserVM model)
        {
            try
            {
                var ett = await _pblContexts.Users.Where(x =>
                    (model.UserName == null || EF.Functions.Like(x.UserName, $"%{model.UserName}%"))
                    //&& (model.FirstName == null && x.FirstName == model.FirstName)
                    //&& (model.LastName == null && x.LastName == model.LastName)
                ).ToListAsync();

                var returnMOdel = MapList<User, DbModel.User>(ett);

                return Result<IEnumerable<User>>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<User>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }
    }
}


/*
 
        var _ds = new CMS.Dal.DataSource.UserDataSource();
        var model = new Model.User
            {
                UnicId = Guid.NewGuid(),
                FirstName = "f1",
                LastName = "l1",
                UserName = "u1",
                Password = "p1"
            };
        var m1 = await _ds.AddAsync(model);
        m1.Data.Password = "p1-1";
        await _ds.EditAsync(m1.Data);

        var m2 = await _ds.AddAsync(new Model.User
            {
                UnicId = Guid.NewGuid(),
                FirstName = "f2",
                LastName = "l2",
                UserName = "u2",
                Password = "p2"
            });

        var m3 = await _ds.GetAsync(m2.Data.Id);
        var m4 = await _ds.ListAsync(new Model.UserVM());
        var m5 = await _ds.ListAsync(new Model.UserVM { UserName = "1" });
        var m = await _ds.RemoveAsync(m2.Data.Id);
 */