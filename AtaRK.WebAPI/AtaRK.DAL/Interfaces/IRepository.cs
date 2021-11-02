using AtaRK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> condition);

        Task<IEnumerable<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> condition);

        Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> condition,
            int skip,
            int take);

        Task<IEnumerable<TOut>> SelectAsync<TOut>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TOut>> selector);

        Task<IEnumerable<TOut>> SelectAsync<TOut>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TOut>> selector,
            int skip,
            int take);

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<int> CountAsync(TEntity entity);

        Task SaveAsync();
    }
}
