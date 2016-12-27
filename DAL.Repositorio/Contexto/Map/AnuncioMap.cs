using Finantec.Modelo.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contexto.Map
{
    public class AnuncioMap: EntityTypeConfiguration<Anuncio>
    {
        public AnuncioMap()
        {
            ToTable("Anuncio");
            HasKey(x => x.Id);   
            HasRequired(anuncio => anuncio.Pesquisa);
            HasRequired(anuncio => anuncio.Produto);
            HasRequired(anuncio => anuncio.Localizacao);

            HasOptional(anuncio => anuncio.Anunciante);


        }
    }
}
