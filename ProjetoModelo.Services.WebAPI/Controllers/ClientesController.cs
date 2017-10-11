using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using System.Xml.XPath;
using Newtonsoft.Json;
using ProjetoModelo.Application.Interfaces;
using ProjetoModelo.Application.ViewModels;
using ProjetoModelo.Domain.Entities;

namespace ProjetoModelo.Services.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClientesController : ApiController
    {
        private readonly IClienteAppService _clienteApp;

        public ClientesController(IClienteAppService clienteApp)
        {
            _clienteApp = clienteApp;
        }

        // GET: api/Clientes
        public IEnumerable<ClienteViewModel> Get()
        {
            return _clienteApp.GetAll();
        }


        public SysDataTablePager<EnderecoApiViewModel> Get(Guid id, int? iDisplayStart, string sEcho, string sSearch)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            sEcho = nvc["sEcho"];


            var cliente = _clienteApp.GetById(id).Enderecos;
            var xpto = new List<EnderecoApiViewModel>();

            foreach (var endereco in cliente)
            {
                xpto.Add(new EnderecoApiViewModel()
                {
                    Rua = endereco.Rua,
                    Bairro = endereco.Bairro,
                    Numero = endereco.Numero,
                    CEP = endereco.CEP,
                    Estado = endereco.Estado.Nome,
                    Cidade = endereco.Cidade.Nome,
                    Complemento = endereco.Complemento
                });
            }

            var myStuffPaged = new SysDataTablePager<EnderecoApiViewModel>();

            myStuffPaged.iTotalRecords = xpto.Count;
            myStuffPaged.iTotalDisplayRecords = xpto.Count;
            myStuffPaged.aaData = xpto.ToList();
            myStuffPaged.sEcho = sEcho;

            return myStuffPaged;
        }

        // POST: api/Clientes
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Clientes/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Clientes/5
        public void Delete(int id)
        {
        }
    }

    public class SysDataTablePager<T>
    {
        public string sEcho { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<T> aaData { get; set; }
    }

    public class DataTablesParam
    {
        public int iDisplayStart { get; set; }
        public int iDisplayLength { get; set; }
        public int iColumns { get; set; }
        public string sSearch { get; set; }
        public bool bEscapeRegex { get; set; }
        public int iSortingCols { get; set; }
        public int sEcho { get; set; }
        public List<bool> bSortable { get; set; }
        public List<bool> bSearchable { get; set; }
        public List<string> sSearchColumns { get; set; }
        public List<int> iSortCol { get; set; }
        public List<string> sSortDir { get; set; }
        public List<bool> bEscapeRegexColumns { get; set; }

        public DataTablesParam()
        {
            bSortable = new List<bool>();
            bSearchable = new List<bool>();
            sSearchColumns = new List<string>();
            iSortCol = new List<int>();
            sSortDir = new List<string>();
            bEscapeRegexColumns = new List<bool>();
        }
    }
}