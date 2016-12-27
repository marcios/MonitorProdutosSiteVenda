using System.Collections.Generic;

namespace Finantec.Modelo.Entidades
{
    public class Endereco
    {

        public Endereco()
        {
            this.Anuncios = new List<Anuncio>();
        }
        public int Id { get; set; }
        public string Municipio { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<Anuncio>Anuncios { get; set; }
    }
}