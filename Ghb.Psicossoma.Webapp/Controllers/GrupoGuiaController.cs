using Newtonsoft.Json;
using Ghb.Psicossoma.Cache;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class GrupoGuiaController : Controller
    {
        private readonly string baseAddress = "https://localhost:7188/api";
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public GrupoGuiaController(CacheService cacheService, IConfiguration configuration)
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
            List<GrupoGuiaViewModel>? gruposGuia = new();
            HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/grupoguia/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<GrupoGuiaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<GrupoGuiaViewModel>>(data);
                gruposGuia = model?.Items.ToList();

                ViewBag.TotalEncontrado = gruposGuia.Count;
            }

            return View(gruposGuia);
        }

        public IActionResult Create()
        {
            GrupoGuiaViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(GrupoGuiaViewModel grupoGuia)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/grupoguia/create", grupoGuia).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<GrupoGuiaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<GrupoGuiaViewModel>>(content);
            }

            return View(grupoGuia);
        }

        public ActionResult Edit(int id)
        {
            GrupoGuiaViewModel? grupoGuiaFound = null;
            string grupoGuiaFind = $"{baseAddress}/grupoguia/get/{id}";
            HttpResponseMessage message = _httpClient.GetAsync(grupoGuiaFind).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<GrupoGuiaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<GrupoGuiaViewModel>>(content);
                grupoGuiaFound = model!.Items.FirstOrDefault()!;
            }

            return View(grupoGuiaFound);
        }

        [HttpPost]
        public ActionResult Edit(GrupoGuiaViewModel grupoGuia)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/grupoguia/update", grupoGuia).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<GrupoGuiaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<GrupoGuiaViewModel>>(content);
            }

            return View(grupoGuia);
        }

    }
}
