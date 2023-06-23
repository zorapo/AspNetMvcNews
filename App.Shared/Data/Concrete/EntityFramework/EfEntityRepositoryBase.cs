using App.Shared.Data.Abstract;
using App.Shared.Entities.Abstract;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace App.Shared.Data.Concrete.EntityFramework
{
	public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity> where TEntity : class, IEntity ,new()
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

		public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate=null)
		{
			return await (predicate==null ? _context.Set<TEntity>().CountAsync() : _context.Set<TEntity>().CountAsync(predicate));
		}

		public async Task DeleteAsync(TEntity entity)
		{
			await Task.Run(() => { _context.Set<TEntity>().Remove(entity); }); //Remove async değil. Biz kendimiz oluşturmuş olduk async'yi
		}

		public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
		{
			IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();
			if (predicate != null)
			{
				query = query.Where(predicate);
			}
			return query.ToList();
		}

		public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();
			if (predicate != null)
			{
				query = query.Where(predicate);
			}
			if (includeProperties.Any())
			{
				foreach (var includeProperty in includeProperties)
				{
					query = query.Include(includeProperty);
				}
			}
			return await query.ToListAsync();
		}

		public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();	
			query = query.Where(predicate);
			
			if (includeProperties.Any())
			{
				foreach (var includeProperty in includeProperties)
				{
					query = query.Include(includeProperty);
				}
			}
			return await query.FirstOrDefaultAsync();
		}

        public async Task<IList<TEntity>> SearchAsync(IList<Expression<Func<TEntity, bool>>> predicates, params Expression<Func<TEntity, object>>[] includeProperties)
        {
			IQueryable<TEntity> query = _context.Set<TEntity>();
			if (predicates.Any())
			{
				var newPredicate = PredicateBuilder.New<TEntity>();
				foreach(var predicate in predicates)
				{
					
					newPredicate.Or(predicate);
				}
                query = query.Where(newPredicate);
            }
            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
			return await query.ToListAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
		{
			await Task.Run(() => { _context.Set<TEntity>().Update(entity); });
			return entity;
		}
	}
}
