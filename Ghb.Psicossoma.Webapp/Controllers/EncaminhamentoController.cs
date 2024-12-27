using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Ghb.Psicossoma.Webapp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class EncaminhamentoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public EncaminhamentoController(CacheService cacheService, IConfiguration configuration)
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
            List<EncaminhamentoViewModel>? list = new();
            HttpResponseMessage message = _httpClient.GetAsync($"encaminhamento/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<EncaminhamentoViewModel>? model = JsonConvert.DeserializeObject<ResultModel<EncaminhamentoViewModel>>(data);
                list = model?.Items.ToList();

                ViewBag.TotalEncontrado = list.Count;
            }
            return View(list);
        }

        public ActionResult Edit(int id)
        {
            EncaminhamentoViewModel? itemFound = null;
            HttpResponseMessage message = _httpClient.GetAsync($"encaminhamento/Get/{id}").Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<EncaminhamentoViewModel>? model = JsonConvert.DeserializeObject<ResultModel<EncaminhamentoViewModel>>(content);
                itemFound = model!.Items.FirstOrDefault()!;

                itemFound.OpcoesEspecialidade = FillEspecialidade();
                itemFound.OpcoesPlanoSaude = FillPlanoSaude();
                itemFound.OpcoesCID = FillCID();
            }

            return View(itemFound);
        }

        private List<SelectListItem> FillEspecialidade()
        {
            List<SelectListItem> registros = new();
            HttpResponseMessage message = _httpClient.GetAsync($"especialidade/getall").Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<EspecialidadeViewModel>? response = JsonConvert.DeserializeObject<ResultModel<EspecialidadeViewModel>>(content);

            if (message.IsSuccessStatusCode)
            {
                if (!response?.HasError ?? false)
                {
                    foreach (EspecialidadeViewModel item in response?.Items!)
                    {
                        SelectListItem select = new() { Text = item.Nome, Value = item.Id.ToString() };
                        registros.Add(select);
                    }

                    registros.Insert(0, new SelectListItem() { Text = "[Selecione]", Value = "-1" });
                }
            }

            return registros;
        }

        private List<SelectListItem> FillPlanoSaude()
        {
            List<SelectListItem> registros = new();
            HttpResponseMessage message = _httpClient.GetAsync($"planosaude/getall").Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<PlanoSaudeViewModel>? response = JsonConvert.DeserializeObject<ResultModel<PlanoSaudeViewModel>>(content);

            if (message.IsSuccessStatusCode)
            {
                if (!response?.HasError ?? false)
                {
                    foreach (PlanoSaudeViewModel item in response?.Items!)
                    {
                        SelectListItem select = new() { Text = item.Nome, Value = item.Id.ToString() };
                        registros.Add(select);
                    }

                    registros.Insert(0, new SelectListItem() { Text = "[Selecione]", Value = "-1" });
                }
            }

            return registros;
        }

        private List<SelectListItem> FillCID()
        {
            List<SelectListItem> registros = new();
            HttpResponseMessage message = _httpClient.GetAsync($"cid/GetAllAtivos").Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<CidViewModel>? response = JsonConvert.DeserializeObject<ResultModel<CidViewModel>>(content);

            if (message.IsSuccessStatusCode)
            {
                if (!response?.HasError ?? false)
                {
                    foreach (CidViewModel item in response?.Items!)
                    {
                        SelectListItem select = new() { Text = item.Nome, Value = item.Id.ToString() };
                        registros.Add(select);
                    }

                    registros.Insert(0, new SelectListItem() { Text = "[Selecione]", Value = "-1" });
                }
            }

            return registros;
        }
    }
}
