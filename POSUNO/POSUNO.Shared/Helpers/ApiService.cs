﻿using Newtonsoft.Json;
using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

                HttpResponseMessage response = await client.PostAsync("api/Account/CreateToken", content);
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccesss = false,
                        Message = result
                    };
                }
                TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);
                return new Response
                {
                    IsSuccesss = true,
                    Result = tokenResponse
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
        public static async Task<Response> PostAsync<T>(string contoller, T model)
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

                HttpResponseMessage response = await client.PostAsync($"api/{contoller}",content);
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccesss = false,
                        Message = result
                    };
                }
                T item = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccesss = true,
                    Result = item
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
        public static async Task<Response> PutAsync<T>(string contoller, T model,int id)
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

                HttpResponseMessage response = await client.PutAsync($"api/{contoller}/{id}", content);
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccesss = false,
                        Message = result
                    };
                }
                T item = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccesss = true,
                    Result = item
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
        public static async Task<Response> DeleteAsync(string contoller, int id)
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

                HttpResponseMessage response = await client.DeleteAsync($"api/{contoller}/{id}");
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccesss = false,
                        Message = result
                    };
                }
                return new Response
                {
                    IsSuccesss = true,
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
        public static async Task<Response> GetListAsync<T>(string contoller,string token)
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
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
        public static async Task<Response> PostAsync<T>(string contoller, T model, string token)
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                HttpResponseMessage response = await client.PostAsync($"api/{contoller}", content);
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccesss = false,
                        Message = result
                    };
                }
                T item = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccesss = true,
                    Result = item
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
        public static async Task<Response> PutAsync<T>(string contoller, T model, int id, string token)
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                HttpResponseMessage response = await client.PutAsync($"api/{contoller}/{id}", content);
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccesss = false,
                        Message = result
                    };
                }
                T item = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccesss = true,
                    Result = item
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
        public static async Task<Response> DeleteAsync(string contoller, int id, string token)
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                HttpResponseMessage response = await client.DeleteAsync($"api/{contoller}/{id}");
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccesss = false,
                        Message = result
                    };
                }
                return new Response
                {
                    IsSuccesss = true,
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
