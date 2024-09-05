using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Ghb.Psicossoma.Cache;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class AccountController : Controller
    {
        private readonly string baseAddress = "https://localhost:7020/api";
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public AccountController(CacheService cacheService, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _cacheService = cacheService;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["baseApiAddress"]!);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel loginData)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/user/authenticate", loginData).Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<AuthenticationModel> response = JsonConvert.DeserializeObject<ResultModel<AuthenticationModel>>(content);

            if (message.IsSuccessStatusCode && response?.HasError == false)
            {
                _cacheService.GetCacheEntry("token", response.Items.FirstOrDefault()?.Token!);
                return RedirectToAction("Index", "User");
            }

            return BadRequest(response?.Message);
        }

    }
}
