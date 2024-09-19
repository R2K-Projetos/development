using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class FuncionalidadeController : Controller
    {
        private readonly string baseAddress = "https://localhost:7188/api/funcionalidade";
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public FuncionalidadeController(CacheService cacheService, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _cacheService = cacheService;

            _httpClient.BaseAddress = new Uri(_configuration["baseApiAddress"]!);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _cacheService.GetCacheEntry("token", ""));
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<FuncionalidadeViewModel>? list = new();
            HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<FuncionalidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<FuncionalidadeViewModel>>(data);
                list = model?.Items.ToList();

                ViewBag.TotalEncontrado = list.Count;
            }
            return View(list);
        }

        public IActionResult Create()
        {
            FuncionalidadeViewModel model = new();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FuncionalidadeViewModel obj)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/create", obj).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<FuncionalidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<FuncionalidadeViewModel>>(content);
            }

            return View(obj);
        }

        public ActionResult Edit(int id)
        {
            FuncionalidadeViewModel? itemFound = null;
            string itemFind = $"{baseAddress}/get/{id}";
            HttpResponseMessage message = _httpClient.GetAsync(itemFind).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<FuncionalidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<FuncionalidadeViewModel>>(content);
                itemFound = model!.Items.FirstOrDefault()!;
            }

            return View(itemFound);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FuncionalidadeViewModel obj)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/update", obj).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<FuncionalidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<FuncionalidadeViewModel>>(content);
            }

            return View(obj);
        }
    }
}
