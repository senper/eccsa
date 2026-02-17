using ApiCallerService.Model;
using ApiCallerService.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiCallerService.ApiClient
{
    public class ApiConfig
    {
        public string BaseAddress { get; set; }
        public string Url { get; set; }
        public string AuthUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class Auth
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
    public class DeserializeSchema
    {
        [JsonProperty("$id")]
        public int Id { get; set; }
        [JsonProperty("list")]
        public List<CertificateOfOrigin> List { get; set; }
    }
    public class Client
    {
        public IConfiguration Configuration { get; }
        private ApiConfig ApiConfig = new ApiConfig();
        private Auth auth = new();
        private string connectionString = string.Empty;
        private DateTime LastSuccessfulReadTime = DateTime.MinValue;
        private HttpClient _theClient = null;
        public Client(IConfiguration configuration)
        {
            Configuration = configuration;
            SetApiConfig();
        }


        public void SetApiConfig()
        {
            connectionString = Configuration.GetConnectionString("DefaultConnection");
            //Api Configs
            var apiConfigSection = Configuration.GetSection("ApiConfig");
            ApiConfig.BaseAddress = apiConfigSection.GetSection("BaseAddress").Value;
            ApiConfig.Url = apiConfigSection.GetSection("Url").Value;
            ApiConfig.AuthUrl = apiConfigSection.GetSection("AuthUrl").Value;
            ApiConfig.UserName = apiConfigSection.GetSection("UserName").Value;
            ApiConfig.Password = apiConfigSection.GetSection("Password").Value;

        }
        public HttpClient TheClient
        {
            get
            {
                var uri = new Uri(ApiConfig.BaseAddress);
                if (_theClient != null) return _theClient;
                _theClient = new ServiceCollection()
               .AddHttpClient()
               .BuildServiceProvider()
               .GetService<IHttpClientFactory>()
               .CreateClient();
                // var httpClient = new HttpClient();
                _theClient.BaseAddress = new Uri(ApiConfig.BaseAddress);
                _theClient.DefaultRequestHeaders.Accept.Clear();
                _theClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return _theClient;
            }
        }
        public async Task<bool> Authenticate()
        {
            var context = new Context(connectionString);
            var respository = new Repository(context);
            //return true;
            var httpClient = new ServiceCollection()
                .AddHttpClient()
                .BuildServiceProvider()
                .GetService<IHttpClientFactory>()
                .CreateClient();
            // var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ApiConfig.BaseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var payload = "{\"userName\":\"" + ApiConfig.UserName + "\",\"password\": \"" + ApiConfig.Password + "\"}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
            var retryCount = 0;
            var errorMsg = string.Format("Unable to authenticate with message #message# at {0}/{1}", ApiConfig.BaseAddress, ApiConfig.AuthUrl);
            auth = new();
            while (string.IsNullOrEmpty(auth.Token) && retryCount < 10)
            {
                try
                {
                    var response = await httpClient.PostAsync(ApiConfig.AuthUrl, c);
                    if (response.IsSuccessStatusCode)
                    {
                        var bearer = await response.Content.ReadAsStringAsync();
                        //Log.Logger.Information(bearer);
                        auth = JsonConvert.DeserializeObject<Auth>(bearer);
                    }
                    else
                    {
                        retryCount = await Retry(retryCount, response.StatusCode.ToString(), errorMsg, respository);
                    }
                }
                catch (Exception ex)
                {
                    retryCount = await Retry(retryCount, ex.Message, errorMsg, respository);
                }
            }
            return !string.IsNullOrEmpty(auth.Token);
        }

        private async Task<int> Retry(int retryCount, string statusCode, string message, Repository repository)
        {
            var msg = message.Replace("#message#", statusCode);
            Log.Logger.Warning(msg);
            retryCount++;
            var delay = retryCount == 1 ? 2 : (retryCount == 2 ? 5 : (retryCount == 3 ? 10 : 0 ));  // (int)Math.Pow(10, retryCount);
            delay = delay * 60 * 1000;            
            Log.Logger.Information(string.Format("Retry after {0} seconds", delay / 1000));
            await repository.SaveESWRequests(false, DateTime.Today, msg);
            await Task.Delay(delay);
            SetApiConfig();
            return retryCount;
        }
        public async Task<DataReadResult> ReadData(DateTime from)
        {
            var context = new Context(connectionString);
            var respository = new Repository(context);
            //
            List<CertificateOfOrigin> data = null;
            var res = "";
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiConfig.BaseAddress)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.DefaultRequestHeaders.Authorization =
            //  new AuthenticationHeaderValue("Bearer", auth.Token);
            string username = ApiConfig.UserName;
            string password = ApiConfig.Password;

            string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));

            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + svcCredentials);

            var retryCount = 0;
            var query = $"{ApiConfig.Url}?fromDate=" +
                        $"{from.ToString("yyyy MM dd").Replace(" ", "")}";
            var errorMsg = string.Format("Unable to get data with message #message# at {0}{2}", ApiConfig.BaseAddress, ApiConfig.Url, query);
            while (data == null && retryCount <= 3)
            {
                try
                {
                    var response = await httpClient.GetAsync(query);
                    if (response.IsSuccessStatusCode)
                    {
                        res = await response.Content.ReadAsStringAsync();
                        //res = File.ReadAllText("text.json");
                        data = JsonConvert.DeserializeObject<List<CertificateOfOrigin>>(res);
                        LastSuccessfulReadTime = DateTime.UtcNow;
                    }
                    else
                    {
                        retryCount = await Retry(retryCount, response.StatusCode.ToString(), errorMsg, respository);
                    }
                }
                catch (Exception ex)
                {
                    retryCount = await Retry(retryCount, ex.Message, errorMsg, respository);
                }
            }
            return new DataReadResult { List = data, JsonValue = res };
        }
        public async Task<bool> SaveData(DataReadResult data)
        {
            if (data == null) return false;
            var context = new Context(connectionString);
            var respository = new Repository(context);
            var success = await respository.Save(data);
            var dirPath = Path.Combine(
                                        Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "jsonfiles");
            var filePath = @$"{Path.Combine(dirPath,
                                            DateTime.UtcNow.ToFileTimeUtc().ToString())}.json";
            try
            {
                Directory.CreateDirectory(dirPath);
                File.WriteAllText(filePath, data.JsonValue);
            }
            catch (Exception ex)
            {
                Log.Logger.Warning(string.Format("Json file save error {0}", filePath));
                Log.Logger.Warning(string.Format("Json file save error {0} {1}", ex.Message), ex);
            }
            return success;
        }
        public async Task<DateTime?> LastReadSuccess()
        {
            var context = new Context(connectionString);
            var respository = new Repository(context);
            return await respository.LastSuccessReadDate();
        }
    }
}
