using AtaRK.Core.Interfaces;
using AtaRK.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtaRK.DAL.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly TimeSpan _defaultTokenTime = new TimeSpan(0, 0, 30);

        private readonly DbContext _context;

        public Repository(DbContext dbContext)
        {
            this._context = dbContext;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition)
        {
            using (CancellationTokenSource source = new CancellationTokenSource(this._defaultTokenTime))
            {
                return await this._context.Set<TEntity>().AnyAsync(condition, source.Token);
            }
        }

        public Task<int> CountAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(TEntity entity)
        {
            using (CancellationTokenSource source = new CancellationTokenSource(this._defaultTokenTime))
            {
                await this._context.Set<TEntity>().AddAsync(entity, source.Token);
            }
        }

        public Task DeleteAsync(TEntity entity)
        {
            using (CancellationTokenSource source = new CancellationTokenSource(this._defaultTokenTime))
            {
                return Task.Run(() =>
                {
                    this._context.Set<TEntity>().Remove(entity);
                },
                source.Token);
            }
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> condition)
        {
            using (CancellationTokenSource source = new CancellationTokenSource(this._defaultTokenTime))
            {
                return Task.Run(() =>
                {
                    this._context.Set<TEntity>()
                        .RemoveRange(this._context.Set<TEntity>().Where(condition));
                },
                source.Token);
            }
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> condition)
        {
            using (CancellationTokenSource source = new CancellationTokenSource(this._defaultTokenTime))
            {
                return await this._context.Set<TEntity>().FirstOrDefaultAsync(condition, source.Token);
            }
        }

        public async Task SaveAsync()
        {
            using (CancellationTokenSource source = new CancellationTokenSource(this._defaultTokenTime))
            {
                await this._context.SaveChangesAsync();
            }
        }

        public Task<IEnumerable<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> condition, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TOut>> SelectAsync<TOut>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TOut>> selector)
        {
            using (CancellationTokenSource source = new CancellationTokenSource(this._defaultTokenTime))
            {
                return Task.Run(() =>
                {
                    return this._context.Set<TEntity>().Where(condition).Select(selector).AsEnumerable();
                },
                source.Token);
            }
        }

        public Task<IEnumerable<TOut>> SelectAsync<TOut>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TOut>> selector, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity)
        {
            using (CancellationTokenSource source = new CancellationTokenSource(this._defaultTokenTime))
            {
                return Task.Run(() =>
                {
                    this._context.Set<TEntity>().Update(entity);
                },
                source.Token);
            }
        }
    }
}
