using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class CidController : Controller
    {
        private readonly string baseAddress = "https://localhost:7188/api";
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public CidController(CacheService cacheService, IConfiguration configuration)
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
            List<CidViewModel>? list = new();
            HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/cid/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<CidViewModel>? model = JsonConvert.DeserializeObject<ResultModel<CidViewModel>>(data);
                list = model?.Items.ToList();

                ViewBag.TotalEncontrado = list.Count;
            }
            return View(list);
        }

        public IActionResult Create()
        {
			CidViewModel cid = new();
            return View(cid);
        }

        [HttpPost]
        public IActionResult Create(CidViewModel cid)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/cid/create", cid).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<CidViewModel>? model = JsonConvert.DeserializeObject<ResultModel<CidViewModel>>(content);
            }

            return View(cid);
        }

        public ActionResult Edit(int id)
        {
            CidViewModel? itemFound = null;
            string itemFind = $"{baseAddress}/cid/get/{id}";
            HttpResponseMessage message = _httpClient.GetAsync(itemFind).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<CidViewModel>? model = JsonConvert.DeserializeObject<ResultModel<CidViewModel>>(content);
                itemFound = model!.Items.FirstOrDefault()!;
            }

            return View(itemFound);
        }

        [HttpPost]
        public ActionResult Edit(CidViewModel cid)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/cid/update", cid).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<CidViewModel>? model = JsonConvert.DeserializeObject<ResultModel<CidViewModel>>(content);
            }

            return View(cid);
        }
    }
}
