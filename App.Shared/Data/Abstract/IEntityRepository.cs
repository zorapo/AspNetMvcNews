using App.Shared.Entities.Abstract;
using System.Linq.Expressions;


namespace App.Shared.Data.Abstract
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
	{
		Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties); //bir parametre de gelebilir, birden çok da gelebilir(include)
		Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
		IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null);
		Task<T> AddAsync(T entity);
		Task<T> UpdateAsync(T entity);
		Task DeleteAsync(T entity);
		Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
		Task<int> CountAsync(Expression<Func<T, bool>> predicate=null);
		Task<IList<T>> SearchAsync(IList<Expression<Func<T, bool>>> predicates, params Expression<Func<T, object>>[] includeProperties);

	}
}
