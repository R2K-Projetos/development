using Newtonsoft.Json;
using Ghb.Psicossoma.Cache;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Ghb.Psicossoma.Webapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ghb.Psicossoma.Webapp.Models.ResultModel;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class ProfissionalController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;
        private readonly IConfiguration _configuration;

        public ProfissionalController(CacheService cacheService, IConfiguration configuration)
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

        public IActionResult Create()
        {
            ProfissionalViewModel profissional = new();
            profissional.Endereco = new();
            profissional.Telefone = new();
            profissional.Endereco.Ufs = FillUf();
            profissional.TiposTelefone = FillTipoCelular();
            profissional.OpcoesSexo = FillSexoDropDown();
            profissional.Registros = FillRegistro();

            return View(profissional);
        }

        [HttpPost]
        public IActionResult Create(ProfissionalViewModel profissional)
        {
            ProfissionalViewModel? result = null;
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"profissional/create", profissional).Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<ProfissionalViewModel>? response = JsonConvert.DeserializeObject<ResultModel<ProfissionalViewModel>>(content);

            if (message.IsSuccessStatusCode)
            {
                if (!response?.HasError ?? false)
                {
                    result = response?.Items.FirstOrDefault()!;
                }
                else
                {
                    //TODO: Determinar como serão exibidas mensagens de erro ao usuário
                    //Aqui, como não há associação com Pessoa, a API retorna erro, na propriedade message.
                }
            }

            return View(result);
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

        private List<SelectListItem> FillUf()
        {
            List<SelectListItem> ufs = new();
            HttpResponseMessage message = _httpClient.GetAsync($"uf/getall").Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<UfViewModel>? response = JsonConvert.DeserializeObject<ResultModel<UfViewModel>>(content);

            if (message.IsSuccessStatusCode)
            {
                if (!response?.HasError ?? false)
                {
                    foreach (UfViewModel item in response?.Items!)
                    {
                        SelectListItem select = new() { Text = item.Nome, Value = item.Id.ToString() };
                        ufs.Add(select);
                    }

                    ufs.Insert(0, new SelectListItem() { Text = "--Selecione o estado--", Value = "-1" });
                }
            }

            return ufs;
        }

        private List<SelectListItem> FillTipoCelular()
        {
            List<SelectListItem> tipos = new();
            HttpResponseMessage message = _httpClient.GetAsync($"telefone/gettypes").Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<TipoTelefoneViewModel>? response = JsonConvert.DeserializeObject<ResultModel<TipoTelefoneViewModel>>(content);

            if (message.IsSuccessStatusCode)
            {
                if (!response?.HasError ?? false)
                {
                    foreach (TipoTelefoneViewModel item in response?.Items!)
                    {
                        SelectListItem select = new() { Text = item.Nome, Value = item.Id.ToString() };
                        tipos.Add(select);
                    }

                    tipos.Insert(0, new SelectListItem() { Text = "--Selecione o tipo--", Value = "-1" });
                }
            }

            return tipos;
        }

        private List<SelectListItem> FillRegistro()
        {
            List<SelectListItem> registros = new();
            HttpResponseMessage message = _httpClient.GetAsync($"registroprofissional/getall").Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<RegistroProfissionalViewModel>? response = JsonConvert.DeserializeObject<ResultModel<RegistroProfissionalViewModel>>(content);

            if (message.IsSuccessStatusCode)
            {
                if (!response?.HasError ?? false)
                {
                    foreach (RegistroProfissionalViewModel item in response?.Items!)
                    {
                        SelectListItem select = new() { Text = item.Nome, Value = item.Id.ToString() };
                        registros.Add(select);
                    }

                    registros.Insert(0, new SelectListItem() { Text = "--Selecione o registro--", Value = "-1" });
                }
            }

            return registros;
        }
    }
}
