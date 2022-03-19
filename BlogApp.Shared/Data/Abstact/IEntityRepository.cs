using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlogApp.Shared.Entities.Abstract;

namespace BlogApp.Shared.Data.Abstact
{
    public interface IEntityRepository<TEntity>
    where TEntity:class,IEntity,new()
    {
        Task<TEntity> GetAsync(Expression<Func<TEntity,bool>> predicate, params Expression<Func<TEntity,object>>[] includeProperties);

        Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity,bool>> predicate = null, params Expression<Func<TEntity,object>>[] includeProperties);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity,bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity,bool>> predicate = null);
    }
}