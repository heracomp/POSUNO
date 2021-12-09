using Newtonsoft.Json;
using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POSUNO.Helpers
{
    public class ApiService
    {
        public static async Task<Response> LoginAsync(LoginRequest model)
        {
            try
            {
                string request = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

                HttpClientHandler handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                string _url = Settings.GetApiUrl();
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(_url)
                };

                HttpResponseMessage response = await client.PostAsync("api/Account/Login", content);
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccesss = false,
                        Message = result
                    };
                }
                User user = JsonConvert.DeserializeObject<User>(result);
                return new Response
                {
                    IsSuccesss = true,
                    Result = user
               };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccesss = false,
                    Message = ex.Message
                };
            }
        }

        public static async Task<Response> GetListAsync<T>(string contoller)
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                string _url = Settings.GetApiUrl();
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(_url)
                };

                HttpResponseMessage response = await client.GetAsync($"api/{contoller}");
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccesss = false,
                        Message = result
                    };
                }
                List<T> list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccesss = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccesss = false,
                    Message = ex.Message
                };
            }
        }
    }
}
