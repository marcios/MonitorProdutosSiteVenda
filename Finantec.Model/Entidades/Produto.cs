using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finantec.Modelo.Entidades
{
    public class Produto
    {

        public Produto()
        {
            this.FotosProduto = new List<FotoProduto>();
            this.Anuncios = new List<Anuncio>();
        }
        public int Id { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string TipoDescricao { get; set; }
        public decimal Preco { get; set; }

        public virtual ICollection<FotoProduto> FotosProduto { get; set; }
        public virtual ICollection<Anuncio> Anuncios { get; set; }


    }
}
