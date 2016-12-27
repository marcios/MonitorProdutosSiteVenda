using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Contexto;
using System.Data.Entity;

namespace DAL.Repositorio.Base
{
    public abstract class Repositorio<TEntity> : IDisposable, IRepositorio<TEntity> where TEntity : class
    {
        private BancoContexto ctx = new BancoContexto();
        public void Adicionar(TEntity obj)
        {
            this.ctx.Set<TEntity>().Add(obj);
        }

        public void Atualizar(TEntity obj)
        {
            this.ctx.Entry(obj).State = EntityState.Modified;
        }

        public void Dispose()
        {
            ctx.Dispose();
        }

        public void Excluir(Func<TEntity, bool> predicate)
        {
            this.ctx.Set<TEntity>()
                .Where(predicate)
                .ToList()
                .ForEach(del => ctx.Set<TEntity>().Remove(del));
           
        }

        public TEntity Find(params object[] key)
        {
            return this.ctx.Set<TEntity>().Find(key);
        }

        public IQueryable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return this.GetAll().Where(predicate).AsQueryable();
        }

        public IQueryable<TEntity> GetAll()
        {

            return this.ctx.Set<TEntity>();
        }

        public void SalvarTodos()
        {
            this.ctx.SaveChanges();
        }
    }
}
