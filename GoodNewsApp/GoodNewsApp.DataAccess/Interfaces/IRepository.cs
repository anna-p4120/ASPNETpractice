
using GoodNewsApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsApp.DataAccess.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity //DbSet<TEntity>

    {
        Task<List<TEntity>> GetAllAsync(CancellationToken token);
        
        Task<TEntity> GetByIdAsync(Guid Id, CancellationToken token);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> searchPredicate, params Expression<Func<TEntity, object>>[] includesPredicate);

        Task AddAsync(TEntity obj);

        void Add(TEntity obj);
        
        Task AddRangeAsync(IEnumerable<TEntity> obj);

        void Update(TEntity obj);

        Task Delete(Guid id);

        
    }
}
