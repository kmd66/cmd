using Microsoft.AspNetCore.Components;
using CMS.Model;
using System.Net;

namespace CMS.Helper
{
    public class ApiRequest
    {

        protected NavigationManager _navManager { get; }
        public ApiRequest(NavigationManager navManager)
        {
            _navManager = navManager;
        }

        private async Task<HttpResponseMessage> Request(string route, object? data = null, string? token = null)
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {

                    if(token != null)
                        client.DefaultRequestHeaders.Add("Authorization", token);

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(data ?? "");
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(_navManager.BaseUri + route, content);
                    return response;
                }
            }
            catch (Exception e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadGateway);
                response.Content = new StringContent(e.Message);
                return response;
            }
        }
        public async Task<Result<T>> Post<T>(string route, object? data = null, string? token = null)
        {
            var response = await Request(route, data, token);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var failure = Result<T>.Failure(message: $"ApiRequest Post  Failure {response.StatusCode}");
                failure.Code = ((int)response.StatusCode);
                return failure;
            }
            var responseString = response.Content.ReadAsStringAsync().Result;
            var t = Newtonsoft.Json.JsonConvert.DeserializeObject<Result<T>>(responseString);

            return t;
        }
        public async Task<Result> Post(string route, object? data = null, string? token = null)
        {
            var response = await Request(route, data, token);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var failure = Result.Failure(message: $"ApiRequest Post  Failure {response.StatusCode}");
                failure.Code = ((int)response.StatusCode);
                return failure;
            }
            var responseString = response.Content.ReadAsStringAsync().Result;
            var t = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(responseString);

            return t;
        }

    }
}
