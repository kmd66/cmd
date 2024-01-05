using System.Reflection;

namespace CMS.Model.Interface
{
    public interface IInfo
    {
        public Task<string?> SetSessionStorage(string key, string vlue);
        public Task<string?> GetSessionStorage(string key);
    }
}
