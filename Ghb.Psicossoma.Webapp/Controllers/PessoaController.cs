using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Ghb.Psicossoma.Webapp.Controllers
{
    public class PessoaController : Controller
    {
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
            HttpResponseMessage message = _httpClient.GetAsync($"pessoa/getall").Result;

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
            PessoaViewModel model = new();
            model.Endereco = new();
            model.Telefone = new();
            model.Endereco.Ufs = FillUf();
            model.TiposTelefone = FillTipoTelefone();
            model.OpcoesSexo = FillSexoDropDown();
            model.TipoDeParentesco = FillGrauParentesco();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PessoaViewModel pessoa)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"pessoa/create", pessoa).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PessoaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PessoaViewModel>>(content);
            }

            return View(pessoa);
        }
       
        public ActionResult Edit(int id)
        {
            PessoaViewModel? itemFound = null;
            HttpResponseMessage message = _httpClient.GetAsync($"pessoa/get/{id}").Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PessoaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PessoaViewModel>>(content);
                itemFound = model!.Items.FirstOrDefault()!;

                itemFound.OpcoesSexo = FillSexoDropDown();

                itemFound.Endereco = GetEnderecoPessoa(itemFound.Id);
                if (itemFound.Endereco is null)
                    itemFound.Endereco = new();

                itemFound.Endereco.Ufs = FillUf();

                itemFound.Telefone = new();
                itemFound.Telefone.TiposTelefone = FillTipoTelefone();
                itemFound.TelefonesPessoa = GetTelefonePessoa(itemFound.Id);

                itemFound.TipoDeParentesco = FillGrauParentesco();
            }

            return View(itemFound);
        }

        [HttpPost]
        public ActionResult Edit(PessoaViewModel pessoa)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"pessoa/update", pessoa).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<PessoaViewModel>? model = JsonConvert.DeserializeObject<ResultModel<PessoaViewModel>>(content);
                pessoa.OpcoesSexo = FillSexoDropDown();
            }

            return View(pessoa);
        }

        #region Pessoa
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
        #endregion

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

        public IActionResult ObterPartialFormTelefone(int id)
        {
            TelefoneViewModel? itemFound = null;
            string itemFind = $"telefone/get/{id}";
            HttpResponseMessage message = _httpClient.GetAsync(itemFind).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<TelefoneViewModel>? model = JsonConvert.DeserializeObject<ResultModel<TelefoneViewModel>>(content);
                itemFound = model!.Items.FirstOrDefault()!;
                itemFound.TiposTelefone = FillTipoTelefone();
            }

            return PartialView("~/Views/Shared/_PartialFormTelefone.cshtml", itemFound);
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
                    HttpResponseMessage message = _httpClient.GetAsync($"pessoa/getall").Result;

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
        #endregion
    }
}
