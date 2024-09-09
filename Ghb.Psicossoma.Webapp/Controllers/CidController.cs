using Newtonsoft.Json;
using Ghb.Psicossoma.Cache;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            List<CidViewModel>? listCid = new();
            HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/cid/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<CidViewModel>? model = JsonConvert.DeserializeObject<ResultModel<CidViewModel>>(data);
                listCid = model?.Items.ToList();

                ViewBag.TotalEncontrado = listCid.Count;
            }
            return View(listCid);
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
            CidViewModel? cidFound = null;
            string cidFind = $"{baseAddress}/cid/get/{id}";
            HttpResponseMessage message = _httpClient.GetAsync(cidFind).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<CidViewModel>? model = JsonConvert.DeserializeObject<ResultModel<CidViewModel>>(content);
                cidFound = model!.Items.FirstOrDefault()!;
            }

            return View(cidFound);
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
