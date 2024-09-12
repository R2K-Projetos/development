using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Ghb.Psicossoma.Webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class ProdutoConvenioController : Controller
    {
        private readonly string baseAddress = "https://localhost:7188/api";
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public ProdutoConvenioController(CacheService cacheService, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _cacheService = cacheService;

            _httpClient.BaseAddress = new Uri(_configuration["baseApiAddress"]!);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _cacheService.GetCacheEntry("token", ""));
        }

        public IActionResult Index()
        {
            return View();
        }

        public List<ProdutoConvenioViewModel> GetByName(string name)
        {
            List<ProdutoConvenioViewModel>? produtosConvenio = new();

            if (!string.IsNullOrEmpty(name))
            {
                var f = _cacheService.GetGenericCacheEntry("produtosConvenio", null);
                if (f is not null)
                {
                    List<ProdutoConvenioViewModel> produtosConvenioCache = (List<ProdutoConvenioViewModel>)f;
                    produtosConvenio = produtosConvenioCache
                              .Where(p => p.Descricao.Contains(name, StringComparison.OrdinalIgnoreCase))
                              .ToList();
                }
                else
                {
                    HttpResponseMessage message = _httpClient.GetAsync($"{baseAddress}/produtoconvenio/getall").Result;

                    if (message.IsSuccessStatusCode)
                    {
                        string? data = message.Content.ReadAsStringAsync().Result;
                        ResultModel<ProdutoConvenioViewModel>? model = JsonConvert.DeserializeObject<ResultModel<ProdutoConvenioViewModel>>(data);
                        List<ProdutoConvenioViewModel>? produtosConvenioCache = model?.Items.ToList();
                        _cacheService.SetGenericCacheEntry("produtosConvenio", produtosConvenioCache);
                        produtosConvenio = produtosConvenioCache
                                  .Where(p => p.Descricao.Contains(name, StringComparison.OrdinalIgnoreCase))
                                  .ToList();
                    }
                }
            }

            return produtosConvenio;
        }
    }
}
