using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finantec.Modelo.Entidades
{
    public class Anuncio
    {     

       
        public int Id { get; set; }

        public string CodigoAnuncio { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataPublicacao { get; set; }
        public string Url { get; set; }

        public int ProdutoId { get; set; }
        public int? AnuncianteId { get; set; }
        public int EnderecoId { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual Endereco Localizacao { get; set; }

        public virtual Pessoa Anunciante { get; set; }


        public int PesquisaId { get; set; }
        public virtual Pesquisa Pesquisa { get; set; }
    
    }
}
