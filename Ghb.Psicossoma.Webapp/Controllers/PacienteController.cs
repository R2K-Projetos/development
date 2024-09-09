using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Ghb.Psicossoma.Webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class PacienteController : Controller
    {
        private readonly string baseAddress = "https://localhost:7188/api";
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public PacienteController(CacheService cacheService, IConfiguration configuration)
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
            List<PacienteViewModel>? pacientes = new();
            HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/paciente/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<PacienteViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PacienteViewModel>>(data);
                pacientes = model?.Items.ToList();

                ViewBag.TotalEncontrado = pacientes.Count;
            }

            return View(pacientes);
        }
    }
}
