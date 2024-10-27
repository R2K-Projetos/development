using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class PlanoSaudeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public PlanoSaudeController(CacheService cacheService, IConfiguration configuration)
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
            List<PlanoSaudeViewModel>? list = new();
            HttpResponseMessage message = _httpClient.GetAsync($"planosaude/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<PlanoSaudeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PlanoSaudeViewModel>>(data);
                list = model?.Items.ToList();

                ViewBag.TotalEncontrado = list.Count;
            }
            return View(list);
        }

        public ActionResult Create()
        {
            PlanoSaudeViewModel model = new();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlanoSaudeViewModel obj)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"planosaude/create", obj).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PlanoSaudeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PlanoSaudeViewModel>>(content);
            }

            return View(obj);
        }

        public ActionResult Edit(int id)
        {
            PlanoSaudeViewModel? itemFound = null;
            string itemFind = $"planosaude/get/{id}";
            HttpResponseMessage message = _httpClient.GetAsync(itemFind).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PlanoSaudeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PlanoSaudeViewModel>>(content);
                itemFound = model!.Items.FirstOrDefault()!;
            }

            return View(itemFound);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlanoSaudeViewModel obj)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"planosaude/update", obj).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PlanoSaudeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PlanoSaudeViewModel>>(content);
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
