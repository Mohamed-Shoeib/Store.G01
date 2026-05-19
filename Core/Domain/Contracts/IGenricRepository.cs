using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenricRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false);
        Task<TEntity?> GetAsync(Tkey id);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> spec, bool trackChanges = false);
        Task<TEntity?> GetAsync(ISpecifications<TEntity,Tkey> spec);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
