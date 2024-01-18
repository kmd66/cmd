using CMS.Model;
using CMS.Model.Interface;
using Microsoft.EntityFrameworkCore;


namespace CMS.Dal.DataSource
{
    public class ProductDataSource : BaseDataSource, IDataSource
    {
        public ProductDataSource()
        {
            _pblContexts = new CntContexts();
        }
        readonly CntContexts _pblContexts;

        public async Task<Result<Product>> GetAsync(long id = 0, Guid? unicId = null)
        {
            try
            {
                var ett = await _pblContexts.Products.SingleOrDefaultAsync(x =>
                   (id != 0 && x.Id == id)
                   || (id == 0 && x.UnicId == unicId)
                );
                if (ett == null)
                    return Result<Product>.Successful();

                var returnMOdel = Map<Product, Dal.DbModel.Product>(ett);

                return Result<Product>.Successful(data: returnMOdel);
            }
            catch (Exception ex)
            {
                return Result<Product>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }

        public async Task<Result> AddAsync(Product model)
        {
            try
            {
                var ett = Map<Dal.DbModel.Product, Product>(model);
                _pblContexts.Add<Dal.DbModel.Product>(ett);
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

        public async Task<Result> EditAsync(Product model)
        {
            try
            {
                var record = await GetAsync(model.Id);
                if (!record.Success)
                    return Result.Failure(message: record.Message);
                if (record == null)
                    return Result.Successful();

                var ett = Map<Dal.DbModel.Product, Product>(model);

                ett.Id = record.Data.Id;
                ett.UnicId = record.Data.UnicId;

                _pblContexts.Update<Dal.DbModel.Product>(ett);
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
