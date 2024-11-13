using Ghb.Psicossoma.Cache;
using Ghb.Psicossoma.Webapp.Models;
using Ghb.Psicossoma.Webapp.Models.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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
            List<ProfissionalViewModel>? profissionais = new();
            HttpResponseMessage message = _httpClient.GetAsync($"profissional/getall").Result;

            if (message.IsSuccessStatusCode)
            {
                string? data = message.Content.ReadAsStringAsync().Result;
                ResultModel<ProfissionalViewModel>? model = JsonConvert.DeserializeObject<ResultModel<ProfissionalViewModel>>(data);
                profissionais = model?.Items.ToList();

                ViewBag.TotalEncontrado = profissionais?.Count;
            }

            return View(profissionais);
        }

        public IActionResult Create()
        {
            ProfissionalViewModel model = new();
            model.OpcoesSexo = FillSexoDropDown();
            model.Endereco = new();
            model.Endereco.Ufs = FillUf();
            model.Telefone = new();
            model.TiposTelefone = FillTipoTelefone();
            model.TiposRegistroProfissional = FillRegistro();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ProfissionalViewModel profissional)
        {
            if (ModelState.IsValid)
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
            }
            else
            {
                profissional.OpcoesSexo = FillSexoDropDown();
                profissional.Endereco = new();
                profissional.Endereco.Ufs = FillUf();
                profissional.Telefone = new();
                profissional.TiposTelefone = FillTipoTelefone();
                profissional.TiposRegistroProfissional = FillRegistro();
            }
            return View(profissional);
        }

        public ActionResult Edit(int id)
        {
            ProfissionalViewModel? itemFound = null;
            HttpResponseMessage message = _httpClient.GetAsync($"profissional/Get/{id}").Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<ProfissionalViewModel>? model = JsonConvert.DeserializeObject<ResultModel<ProfissionalViewModel>>(content);
                itemFound = model!.Items.FirstOrDefault()!;

                itemFound.OpcoesSexo = FillSexoDropDown();

                itemFound.Endereco = new();
                itemFound.Endereco = GetEnderecoPessoa(itemFound.PessoaId);
                itemFound.Endereco.Ufs = FillUf();

                itemFound.Telefone = new();
                itemFound.Telefone.TiposTelefone = FillTipoTelefone();
                itemFound.TelefonesPessoa = GetTelefonePessoa(itemFound.PessoaId);

                itemFound.TiposRegistroProfissional = FillRegistro();
            }

            return View(itemFound);
        }

        #region Profissional
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

                    registros.Insert(0, new SelectListItem() { Text = "[Selecione]", Value = "-1" });
                }
            }

            return registros;
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
            HttpResponseMessage message = _httpClient.GetAsync($"telefone/get/{id}").Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<TelefoneViewModel>? model = JsonConvert.DeserializeObject<ResultModel<TelefoneViewModel>>(content);
                itemFound = model!.Items.FirstOrDefault()!;
                itemFound.TiposTelefone = FillTipoTelefone();
            }

            return PartialView("~/Views/Shared/_PartialFormTelefone.cshtml", itemFound);
        }

        public IActionResult ObterPartialListaTelefones(int PessoaId)
        {
            var lista = GetTelefonePessoa(PessoaId);

            return PartialView("~/Views/Profissional/_PartialProfissionalEspecialidades.cshtml", lista);       
        }
        #endregion

        #region Especialidades
        public IActionResult ObterPartialEspecialidades(int ProfissionalId)
        {
            List<EspecialidadeViewModel>? lista = new();
            HttpResponseMessage message = _httpClient.GetAsync($"Especialidade/GetListaDisponivel/" + ProfissionalId).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<EspecialidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<EspecialidadeViewModel>>(content);
                lista = model?.Items.ToList();
            }

            return PartialView("~/Views/Profissional/_PartialProfissionalEspecialidades.cshtml", lista);
        }

        [HttpPost]
        public IActionResult AdicionaEspecialidade(ProfissionalEspecialidadeViewModel obj)
        {
            HttpResponseMessage message = _httpClient.PostAsJsonAsync($"Especialidade/AdicionaEspecialidade", obj).Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<ProfissionalEspecialidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<ProfissionalEspecialidadeViewModel>>(content);
            }

            return Ok(obj);
        }

        public IActionResult RetiraEspecialidade(ProfissionalEspecialidadeViewModel obj)
        {
            HttpResponseMessage message = _httpClient.DeleteAsync($"Especialidade/RetiraEspecialidade/{obj.ProfissionalId}/{obj.EspecialidadeId}").Result;

            if (message.IsSuccessStatusCode)
            {
                string content = message.Content.ReadAsStringAsync().Result;
                ResultModel<ProfissionalEspecialidadeViewModel>? model = JsonConvert.DeserializeObject<ResultModel<ProfissionalEspecialidadeViewModel>>(content);
            }
            return Ok();
        }
        #endregion
    }
}
