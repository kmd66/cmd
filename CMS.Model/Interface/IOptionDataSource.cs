using System.Text.Json;

namespace CMS.Model.Interface
{
    public interface IOptionDataSource
    {
        Task<Result> AddAsync(List<Option> model);
        Task<Result> ListAsync();
    }
}
