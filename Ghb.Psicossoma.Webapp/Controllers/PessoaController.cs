using Newtonsoft.Json;
using Ghb.Psicossoma.Cache;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class PessoaController : Controller
    {
        private readonly string baseAddress = "https://localhost:7188/api";
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public PessoaController(CacheService cacheService, IConfiguration configuration)
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
            List<PessoaViewModel>? pessoas = new();
            HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/pessoa/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<PessoaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PessoaViewModel>>(data);
                pessoas = model?.Items.ToList();

                ViewBag.TotalEncontrado = pessoas.Count;
            }

            return View(pessoas);
        }

        public IActionResult Create()
        {
            PessoaViewModel pessoa = new();
            pessoa.OpcoesSexo = FillSexoDropDown();
            pessoa.Sexo = "";

            return View(pessoa);
        }

        [HttpPost]
        public IActionResult Create(PessoaViewModel pessoa)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/pessoa/create", pessoa).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PessoaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PessoaViewModel>>(content);
            }

            return View(pessoa);
        }
       
        public ActionResult Edit(int id)
        {
            PessoaViewModel? pessoaFound = null;
            string pessoaFind = $"{baseAddress}/pessoa/get/{id}";
            HttpResponseMessage message = _httpClient.GetAsync(pessoaFind).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PessoaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PessoaViewModel>>(content);
                pessoaFound = model!.Items.FirstOrDefault()!;
                pessoaFound.OpcoesSexo = FillSexoDropDown();
            }

            return View(pessoaFound);
        }

        [HttpPost]
        public ActionResult Edit(PessoaViewModel pessoa)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"{baseAddress}/pessoa/update", pessoa).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PessoaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PessoaViewModel>>(content);
                pessoa.OpcoesSexo = FillSexoDropDown();
            }

            return View(pessoa);
        }

        private List<SelectListItem> FillSexoDropDown()
        {
            List<SelectListItem> sexo = new()
            {
                new() { Text = "--Selecione o sexo--", Value = ""},
                new() { Text = "Masculino", Value = "M"},
                new() { Text = "Feminino", Value = "F"}
            };

            return sexo;
        }

        public List<PessoaViewModel> GetByName(string name)
        {
            List<PessoaViewModel>? pessoas = new();

            if (!string.IsNullOrEmpty(name))
            {
                var f = _cacheService.GetGenericCacheEntry("pessoas", null);
                if (f is not null)
                {
                    List<PessoaViewModel> pessoasCache = (List<PessoaViewModel>)f;
                    pessoas = pessoasCache
                              .Where(p => p.Nome.Contains(name, StringComparison.OrdinalIgnoreCase))
                              .ToList();
                }
                else
                {
                    HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/pessoa/getall").Result;

                    if (message.IsSuccessStatusCode)
                    {
                        string? data = message.Content.ReadAsStringAsync().Result;
                        ResultModel<PessoaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PessoaViewModel>>(data);
                        List<PessoaViewModel>? pessoasCache = model?.Items.ToList();
                        _cacheService.SetGenericCacheEntry("pessoas", pessoasCache);
                        pessoas = pessoasCache
                                  .Where(p => p.Nome.Contains(name, StringComparison.OrdinalIgnoreCase))
                                  .ToList();
                    }
                }
            }

            return pessoas;
        }
    }
}
