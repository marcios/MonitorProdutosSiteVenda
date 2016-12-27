using System.Collections.Generic;

namespace Finantec.Modelo.Entidades
{
    public class Pessoa
    {

        public Pessoa()
        {
            this.Anuncios = new List<Anuncio>();
                
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string TelefonePrincipal { get; set; }

        public virtual ICollection<Anuncio>Anuncios { get; set; }
    }
}