using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly string baseAddress = "https://localhost:7188/api";

        public UserController(CacheService cacheService)
        {
            _httpClient = new HttpClient();
            _cacheService = cacheService;

            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _cacheService.GetCacheEntry("token", ""));
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<UserViewModel>? users = new();
            HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/user/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<UserViewModel>? model = JsonConvert.DeserializeObject<ResultModel<UserViewModel>>(data);
                users = model?.Items.ToList();
            }

            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserViewModel user)
        {
            UserViewModel? result = null;
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/user/create", user).Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<UserViewModel>? response = JsonConvert.DeserializeObject<ResultModel<UserViewModel>>(content);

            if (message.IsSuccessStatusCode)
                result = response?.Items.FirstOrDefault()!;

            return View(result);
        }
    }
}
