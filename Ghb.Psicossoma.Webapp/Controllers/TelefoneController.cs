using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class TelefoneController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public TelefoneController(CacheService cacheService, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _cacheService = cacheService;

            _httpClient.BaseAddress = new Uri(_configuration["baseApiAddress"]!);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _cacheService.GetCacheEntry("token", ""));
        }

        [HttpPost]
        public ActionResult Edit(TelefoneViewModel obj)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"Telefone/update", obj).Result;
            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<TelefoneViewModel>? model = JsonConvert.DeserializeObject<ResultModel<TelefoneViewModel>>(content);
            }

            return Ok(obj);
        }
    }
}
