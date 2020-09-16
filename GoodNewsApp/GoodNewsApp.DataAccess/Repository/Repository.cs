using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using GoodNewsApp.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsApp.DataAccess.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity

    {
        private readonly GoodNewsAppContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly TEntity _entity;

        protected Repository(GoodNewsAppContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>(); 
        }

        public async Task<List<TEntity>> GetAllAsync(CancellationToken token)
        {
            return await _dbSet.ToListAsync(token);
        }

        public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _dbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id), token);
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> searchPredicate, params Expression<Func<TEntity, object>>[] includesPredicate)
        {
            var result = _dbSet.Where(searchPredicate);
            if (includesPredicate.Any())
            {
                result = includesPredicate
                    .Aggregate(result, (current, include) => current.Include(include));

            }
            return result;
        }


        public async Task AddAsync(TEntity obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public void Add(TEntity obj)
        {
            _dbSet.Add(obj);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> obj)
        {
            await _dbSet.AddRangeAsync(obj);
        }

        public void Update(TEntity obj)
        {
            _dbSet.Update(obj);
        }

        public async Task Delete(Guid id)
        {
            _dbSet.Remove(await _dbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id)));
        }

        /*public Task<int> SaveChangesAsync()
       {
           return await _context.SaveChangesAsync();
       };*/
    }

}
