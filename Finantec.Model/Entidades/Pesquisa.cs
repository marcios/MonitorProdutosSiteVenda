using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finantec.Modelo.Entidades
{
    public class Pesquisa
    {

        public Pesquisa()
        {
            this.Anuncios = new List<Anuncio>();
            this.DataCadastro = DateTime.Now;
        }

        public int Id { get; set; }

        public string Descricao { get; set; }

        public int PesquisaPaiId { get; set; }

        public DateTime DataCadastro { get; set; }

        public string FiltroPesquisa { get; set; }

        public int TipoAnuncio { get; set; }
        public decimal TotalAnuncio { get; set; }

        public decimal TotalPagina { get; set; }

        public virtual ICollection<Anuncio> Anuncios { get; set; }
    }
}
