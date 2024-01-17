using CMS.Model;
using CMS.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CMS.Dal.DataSource
{
    public class OptionDataSource : BaseDataSource, IOptionDataSource,IDataSource
    {
        public OptionDataSource() {
            _pblContexts = new PblContexts();
        }
        readonly PblContexts _pblContexts;

        public async Task<Result> AddAsync(List<Option> model)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(model);
                var ett = await _pblContexts.Options.FromSql($"pbl.SpOptionAdd {json.JsonQuery()}").ToListAsync();
                return setOptions(ett);
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
                var ett = await _pblContexts.Options.ToListAsync();
                return setOptions(ett);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Option>>.Failure(message: ex.Message);
            }
            finally
            {
                _pblContexts.ChangeTracker.Clear();
            }
        }
        private Result setOptions(List<DbModel.Option> model)
        {
            try
            {
                var list = MapList<Option, DbModel.Option>(model).ToList();
                Option.SetInstance(list);
                return Result.Successful();
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Option>>.Failure(message: ex.Message);
            }
        }

    }
}
