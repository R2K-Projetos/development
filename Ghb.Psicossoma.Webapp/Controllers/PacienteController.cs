using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class PacienteController : Controller
    {
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
            HttpResponseMessage message = _httpClient.GetAsync($"paciente/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<PacienteViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PacienteViewModel>>(data);
                pacientes = model?.Items.ToList();

                ViewBag.TotalEncontrado = pacientes.Count;
            }

            return View(pacientes);
        }

        public IActionResult Create()
        {
            PacienteViewModel model = new();
            model.OpcoesSexo = FillSexoDropDown();            
            model.Endereco = new();
            model.Endereco.Ufs = FillUf();
            model.Telefone = new();
            model.TiposTelefone = FillTipoTelefone();            
            model.TipoDeParentesco = FillGrauParentesco();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PacienteViewModel obj)
        {
            if (ModelState.IsValid || 1 == 1)
            {
                PacienteViewModel? result = null;
                HttpResponseMessage message = _httpClient.PostAsJsonAsync($"paciente/create", obj).Result;
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PacienteViewModel>? response = JsonConvert.DeserializeObject<ResultModel<PacienteViewModel>>(content);

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
            }
            obj.OpcoesSexo = FillSexoDropDown();
            obj.Endereco = new();
            obj.Endereco.Ufs = FillUf();
            obj.Telefone = new();
            obj.TiposTelefone = FillTipoTelefone();

            return View(obj);
        }

        public ActionResult Edit(int id)
        {
            PacienteViewModel? itemFound = null;
            HttpResponseMessage message = _httpClient.GetAsync($"paciente/get/{id}").Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PacienteViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PacienteViewModel>>(content);
                itemFound = model!.Items.FirstOrDefault()!;

                itemFound.OpcoesSexo = FillSexoDropDown();

                itemFound.Endereco = GetEnderecoPessoa(itemFound.PessoaId);                
                itemFound.Endereco.Ufs = FillUf();

                itemFound.Telefone = new();
                itemFound.Telefone.TiposTelefone = FillTipoTelefone();
                itemFound.TelefonesPessoa = GetTelefonePessoa(itemFound.PessoaId);

                itemFound.TipoDeParentesco = FillGrauParentesco();
            }

            return View(itemFound);
        }

        [HttpPost]
        public ActionResult Edit(PacienteViewModel obj)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"paciente/update", obj).Result;
            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PacienteViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PacienteViewModel>>(content);
            }

            return View(obj);
        }

        private List<SelectListItem> FillSexoDropDown()
        {
            List<SelectListItem> sexo = new()
            {
                new() { Text = "[Selecione]", Value = ""},
                new() { Text = "Masculino", Value = "M"},
                new() { Text = "Feminino", Value = "F"}
            };
            return sexo;
        }

        #region Endereco
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
                        SelectListItem select = new() { Text = item.Sigla, Value = item.Id.ToString() };
                        ufs.Add(select);
                    }

                    ufs.Insert(0, new SelectListItem() { Text = "[Selecione]", Value = "-1" });
                }
            }

            return ufs;
        }

        public IActionResult FillCidadesUF(int ufId)
        {
            HttpResponseMessage message = _httpClient.GetAsync($"cidade/GetAllByUf?ufId={ufId}").Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<CidadeViewModel>? response = JsonConvert.DeserializeObject<ResultModel<CidadeViewModel>>(content);

            var listaCidades = new List<CidadeViewModel>();
            if (message.IsSuccessStatusCode)
            {
                if (!response?.HasError ?? false)
                {
                    foreach (CidadeViewModel item in response?.Items!)
                    {
                        var cidade = new CidadeViewModel
                        {
                            Id = item.Id,
                            Nome = item.Nome,
                            UFId = ufId
                        };
                        listaCidades.Add(cidade);
                    }
                }
            }

            return Json(listaCidades);
        }

        private EnderecoViewModel GetEnderecoPessoa(int PessoaId)
        {
            EnderecoViewModel? itemFound = null;
            HttpResponseMessage message = _httpClient.GetAsync($"Endereco/GetEnderecoPessoa/{PessoaId}").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<EnderecoViewModel>? model = JsonConvert.DeserializeObject<ResultModel<EnderecoViewModel>>(data);
                itemFound = model!.Items.FirstOrDefault()!;
            }

            return itemFound;
        }
        #endregion

        #region Telefones
        private List<SelectListItem> FillTipoTelefone()
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

                    tipos.Insert(0, new SelectListItem() { Text = "[Selecione]", Value = "-1" });
                }
            }

            return tipos;
        }

        private List<TelefoneViewModel> GetTelefonePessoa(int PessoaId)
        {
            List<TelefoneViewModel>? lista = new();
            HttpResponseMessage message = _httpClient.GetAsync($"Telefone/GetAllTelefonePessoa/{PessoaId}").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<TelefoneViewModel>? model = JsonConvert.DeserializeObject<ResultModel<TelefoneViewModel>>(data);
                lista = model?.Items.ToList();
            }

            return lista;
        }

        public IActionResult ObterPartialFormTelefone(int Id, int PessoaId)
        {
            var item = new TelefoneViewModel();

            if (Id == 0) {
                item.Id = Id;
                item.PessoaId = PessoaId;
                item.TipoTelefoneId = 0;
                item.Principal = false;
                item.DDDNum = string.Empty;
                item.Ativo = true;
                item.TiposTelefone = FillTipoTelefone();
            }
            else
            {
                HttpResponseMessage message = _httpClient.GetAsync($"telefone/get/{Id}").Result;

                if (message.IsSuccessStatusCode)
                {
                    string content = message.Content.ReadAsStringAsync().Result;
                    ResultModel<TelefoneViewModel>? model = JsonConvert.DeserializeObject<ResultModel<TelefoneViewModel>>(content);
                    item = model!.Items.FirstOrDefault()!;
                    item.TiposTelefone = FillTipoTelefone();
                }
            }            

            return PartialView("~/Views/Shared/_PartialFormTelefone.cshtml", item);
        }

        public IActionResult ObterPartialListaTelefones(int PessoaId)
        {
            var lista = GetTelefonePessoa(PessoaId);

            return PartialView("~/Views/Shared/_PartialListaTelefones.cshtml", lista);
        }
        #endregion

        #region Relacionamento (Grau Parentesco)
        private List<SelectListItem> FillGrauParentesco()
        {
            List<SelectListItem> list = new();
            HttpResponseMessage message = _httpClient.GetAsync($"GrauParentesco/GetAll").Result;
            string content = message.Content.ReadAsStringAsync().Result;
            ResultModel<GrauParentescoViewModel>? response = JsonConvert.DeserializeObject<ResultModel<GrauParentescoViewModel>>(content);

            if (message.IsSuccessStatusCode)
            {
                if (!response?.HasError ?? false)
                {
                    foreach (GrauParentescoViewModel item in response?.Items!)
                    {
                        SelectListItem select = new() { Text = item.Nome, Value = item.Id.ToString() };
                        list.Add(select);
                    }

                    list.Insert(0, new SelectListItem() { Text = "[Selecione]", Value = "-1" });
                }
            }

            return list;
        }
        #endregion
    }
}