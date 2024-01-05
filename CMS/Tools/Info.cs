using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Reflection;

namespace CMS.Tools
{
    public class Info : Model.Interface.IInfo
    {

        private ProtectedLocalStorage _localStorage;
        public Info(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }
        public Task<string?> SetSessionStorage(string key, string vlue)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> GetSessionStorage(string key)
        {
            var t = await _localStorage.GetAsync<string>("token");
            return t.Value;
        }
    }
}
