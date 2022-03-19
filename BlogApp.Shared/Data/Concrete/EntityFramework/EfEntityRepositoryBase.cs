using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlogApp.Shared.Data.Abstact;
using BlogApp.Shared.Entities.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Shared.Data.Concrete.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
    where TEntity:class,IEntity,new()
    {
        protected readonly DbContext _context;

        public EfEntityRepositoryBase(DbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if(predicate != null){
                return await _context.Set<TEntity>().CountAsync(predicate);
            }
            return await _context.Set<TEntity>().CountAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => {
                _context.Set<TEntity>().Remove(entity);
            });
        }

        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();

            if(predicate != null){
                queryable = queryable.Where(predicate);
            }

            if(includeProperties.Any()){
                foreach (var prop in includeProperties)
                {
                    queryable = queryable.Include(prop);
                }
            }

            return await queryable.ToListAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();

            if(predicate != null){
                queryable = queryable.Where(predicate);
            }

            if(includeProperties.Any()){
                foreach (var prop in includeProperties)
                {
                    queryable = queryable.Include(prop);
                }
            }

            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Task.Run(() => {
                _context.Set<TEntity>().Update(entity);
            });

            return entity;
        }
    }
}