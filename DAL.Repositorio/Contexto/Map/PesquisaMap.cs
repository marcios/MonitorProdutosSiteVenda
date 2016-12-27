using Finantec.Modelo.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contexto.Map
{
    public class PesquisaMap : EntityTypeConfiguration<Pesquisa>
    {
        public PesquisaMap()
        {
            ToTable("Pesquisa");
            HasKey(x => x.Id);


        }
    }
}
