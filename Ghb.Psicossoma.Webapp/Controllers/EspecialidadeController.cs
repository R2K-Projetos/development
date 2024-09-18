using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Ghb.Psicossoma.Webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class EspecialidadeController : Controller
    {
        private readonly string baseAddress = "https://localhost:7188/api";
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public EspecialidadeController(CacheService cacheService, IConfiguration configuration)
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
            List<EspecialidadeViewModel>? list = new();
            HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/especialidade/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<EspecialidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<EspecialidadeViewModel>>(data);
                list = model?.Items.ToList();

                ViewBag.TotalEncontrado = list.Count;
            }
            return View(list);
        }

        public IActionResult Create()
        {
            EspecialidadeViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(EspecialidadeViewModel obj)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/especialidade/create", obj).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<EspecialidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<EspecialidadeViewModel>>(content);
            }

            return View(obj);
        }

        public ActionResult Edit(int id)
        {
            EspecialidadeViewModel? itemFound = null;
            string itemFind = $"{baseAddress}/especialidade/get/{id}";
            HttpResponseMessage message = _httpClient.GetAsync(itemFind).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<EspecialidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<EspecialidadeViewModel>>(content);
                itemFound = model!.Items.FirstOrDefault()!;
            }

            return View(itemFound);
        }

        [HttpPost]
        public ActionResult Edit(EspecialidadeViewModel obj)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/especialidade/update", obj).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<EspecialidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<EspecialidadeViewModel>>(content);
            }

            return View(obj);
        }
    }
}
