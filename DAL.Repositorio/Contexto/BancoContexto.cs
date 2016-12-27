using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finantec.Modelo.Entidades;
using System.Data.Entity.ModelConfiguration.Conventions;
using DAL.Contexto.Map;

namespace DAL.Contexto
{
    public class BancoContexto : DbContext
    {
        public BancoContexto() : base("ConnDB")
        {

            //Database.SetInitializer(new DropCreateDatabaseAlways<BancoContexto>());
        }


        public DbSet<Anuncio> Anuncios { get; set; }

        public DbSet<Endereco> Enderecos { get; set; }

        public DbSet<FotoProduto> FotosAnuncio { get; set; }
        public DbSet<Pesquisa> Pesquisas { get; set; }

        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Produto> Produtos { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            /*Desabilitamos o delete em cascata em relacionamentos 1:N evitando
             ter registros filhos     sem registros pai*/
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //Basicamente a mesma configuração, porém em relacionamenos N:N
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            /*Toda propriedade do tipo string na entidade POCO
             seja configurado como VARCHAR no SQL Server*/
            //modelBuilder.Properties<string>()
            //          .Configure(p => p.HasColumnType("varchar"));

            /*Toda propriedade do tipo string na entidade POCO seja configurado como VARCHAR (150) no banco de dados */
            //modelBuilder.Properties<string>()
            //       .Configure(p => p.HasMaxLength(150));

            /*Definimos usando reflexão que toda propriedade que contenha
           "Nome da classe" + Id como "CursoId" por exemplo, seja dada como
           chave primária, caso não tenha sido especificado*/
            //modelBuilder.Properties()
            //   .Where(p => p.Name == p.ReflectedType.Name + "Id")
            //   .Configure(p => p.IsKey());Ma


            modelBuilder.Configurations.Add(new AnuncioMap());
            modelBuilder.Configurations.Add(new EnderecoMap());
            modelBuilder.Configurations.Add(new FotoProdutoMap());
            modelBuilder.Configurations.Add(new PesquisaMap());
            modelBuilder.Configurations.Add(new PessoaMap());
            modelBuilder.Configurations.Add(new ProdutoMap());

            base.OnModelCreating(modelBuilder);
        }

    }
}
