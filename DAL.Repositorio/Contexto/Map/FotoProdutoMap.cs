using Finantec.Modelo.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contexto.Map
{
    public class FotoProdutoMap : EntityTypeConfiguration<FotoProduto>
    {
        public FotoProdutoMap()
        {
            ToTable("FotoProduto");
            HasKey(x => x.Id);


        }
    }
}