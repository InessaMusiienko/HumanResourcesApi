using HumanResources.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Text;

namespace HumanResources.Services
{
    public class HttpService
    {
        private const string ApiKeyHeaderName = "Api_key";
        private const string ApiKeyHeaderValue = "123456";

        private Uri baseAddress = new Uri("https://localhost:7175/api");

        private readonly HttpClient _client;

        public HttpService()
        {
            _client = new HttpClient() { BaseAddress = baseAddress};
            _client.DefaultRequestHeaders.Add(ApiKeyHeaderName,ApiKeyHeaderValue);
        }

        public T Get<T> (string route)
        {
            var responce = _client.GetAsync(_client.BaseAddress + route).Result;
            if (responce.IsSuccessStatusCode)
            {
                string data = responce.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(data);
            }
            else return default(T);
        }

        public bool Post<T>(string route, T model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + route, content).Result;
            if(!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public bool Delete(string route)
        {
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + route).Result;
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
    }
}
