using Domain.Contracts;
using Domain.Models;
using Persistence.Data;
using Persistence.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext context;
        private readonly Dictionary<string, object> repositories;
        private readonly ConcurrentDictionary<string, object> concurrentRepositories;
        public UnitOfWork(StoreDbContext _context)
        {
            context = _context;
            repositories = new Dictionary<string, object>();
            concurrentRepositories = new ConcurrentDictionary<string, object>();
        }
        //public IGenricRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        //{
        //    var type = typeof(TEntity).Name;
        //    if(!repositories.ContainsKey(type))
        //    {
        //        var repository = new GenericRepository<TEntity, Tkey>(context);
        //        repositories.Add(type, repository);
        //    }
        //    return (IGenricRepository<TEntity, Tkey>) repositories[type];
        //}
        public IGenricRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            return (IGenricRepository<TEntity, Tkey>) concurrentRepositories.GetOrAdd(typeof(TEntity).Name, (type) =>
            {
                return new GenericRepository<TEntity, Tkey>(context);
            });
        }
        public async Task<int> SaveChanges()
        {
            return await context.SaveChangesAsync();
        }
    }
}
