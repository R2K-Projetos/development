using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Ghb.Psicossoma.Webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class ConvenioController : Controller
    {
        private readonly string baseAddress = "https://localhost:7188/api";
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public ConvenioController(CacheService cacheService, IConfiguration configuration)
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
            List<ConvenioViewModel>? convenios = new();
            HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/convenio/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<ConvenioViewModel>? model = JsonConvert.DeserializeObject<ResultModel<ConvenioViewModel>>(data);
                convenios = model?.Items.ToList();

                ViewBag.TotalEncontrado = convenios.Count;
            }
            return View(convenios);
        }

        public IActionResult Create()
        {
            ConvenioViewModel convenio = new();
            return View(convenio);
        }

        [HttpPost]
        public IActionResult Create(ConvenioViewModel convenio)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/convenio/create", convenio).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<ConvenioViewModel>? model = JsonConvert.DeserializeObject<ResultModel<ConvenioViewModel>>(content);
            }

            return View(convenio);
        }

        public ActionResult Edit(int id)
        {
            ConvenioViewModel? convenioFound = null;
            string convenioFind = $"{baseAddress}/convenio/get/{id}";
            HttpResponseMessage message = _httpClient.GetAsync(convenioFind).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<ConvenioViewModel>? model = JsonConvert.DeserializeObject<ResultModel<ConvenioViewModel>>(content);
                convenioFound = model!.Items.FirstOrDefault()!;
            }
            return View(convenioFound);
        }

        [HttpPost]
        public ActionResult Edit(ConvenioViewModel convenio)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/convenio/update", convenio).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<ConvenioViewModel>? model = JsonConvert.DeserializeObject<ResultModel<ConvenioViewModel>>(content);
            }

            return View(convenio);
        }
    }
}
