using DAL.Repositorio;
using Finantec.Modelo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finantec.BLL.Servico
{
    public class PesquisaService
    {

        private PesquisaRepositorio _pesquisaRepositorio = new PesquisaRepositorio();

        public void SalvarPesquisa(Pesquisa pesquisa)
        {
            this._pesquisaRepositorio.Adicionar(pesquisa);
            this._pesquisaRepositorio.SalvarTodos();
        }
    }
}
