using ApplicationLayer;
using InfrastructureLayer.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace InfrastructureLayer.Repository
{
    public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity>
        where TEntity : class
    {
        private readonly ApplicationDbContext _context = context;
        private readonly DbSet<TEntity> _entities = context.Set<TEntity>();

        public IQueryable<TEntity> Query()
          => _entities.AsQueryable();

        public DbSet<TEntity> GetDbSet()
           => _entities;

        public TEntity GetById(int id)
            => _entities.Find(id);

        public async Task<TEntity> GetByIdAsync(int id)
            => await _entities.FindAsync(id);

        public TEntity GetByIdIncludeNavigation(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _entities;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<TEntity> GetByIdAsyncIncludeNavigation(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _entities;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public IEnumerable<TEntity> GetAll()
            => _entities;

        public IEnumerable<TEntity> GetAllIncludeNavigation(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _entities;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncIncludeNavigation(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _entities;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }
        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
            => _entities.Where(predicate);

        public void Add(TEntity entity)
            => _entities.Add(entity);

        public async Task AddAsync(TEntity entity)
            => await _entities.AddAsync(entity);

        public void AddRange(IEnumerable<TEntity> entities)
           => _entities.AddRange(entities);

        public void Update(TEntity entity)
            => _entities.Update(entity);

        public void UpdateRange(IEnumerable<TEntity> entities)
           => _entities.UpdateRange(entities);

        /// <summary>
        /// Soft Remove
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(TEntity entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;

            PropertyInfo isDeletedProperty = typeof(TEntity).GetProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool))
            {
                isDeletedProperty.SetValue(entity, true);
            }
        }

        /// <summary>
        /// Soft Remove
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Remove(entity);
        }


        /// <summary>
        /// Delete From DataBase Completely
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteFromDatabase(TEntity entity)
        {
            _entities.Remove(entity);
        }

        /// <summary>
        /// Delete From DataBase Completely
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRangeFromDatabase(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                DeleteFromDatabase(entity);
        }

        public IEnumerable<TResult> ExecuteQuery<TResult>(string sql, params object[] parameters)
            => _context.Database.SqlQueryRaw<TResult>(sql, parameters);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.AnyAsync(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Any(predicate);
        }
    }
}