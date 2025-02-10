using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApplicationLayer
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query();

        DbSet<TEntity> GetDbSet();

        TEntity GetById(int id);

        Task<TEntity> GetByIdAsync(int id);

        TEntity GetByIdIncludeNavigation(int id, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> GetByIdAsyncIncludeNavigation(int id, params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAllIncludeNavigation(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAllAsyncIncludeNavigation(params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        Task AddAsync(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        void DeleteFromDatabase(TEntity entity);

        void DeleteRangeFromDatabase(IEnumerable<TEntity> entities);

        IEnumerable<TResult> ExecuteQuery<TResult>(string sql, params object[] parameters);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        bool Any(Expression<Func<TEntity, bool>> predicate);
    }
}