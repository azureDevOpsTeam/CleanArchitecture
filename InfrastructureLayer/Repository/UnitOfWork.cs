using ApplicationLayer;
using ApplicationLayer.Extensions;
using DNTPersianUtils.Core;
using DomainLayer;
using InfrastructureLayer.Context;
using InfrastructureLayer.Extensions;
using InfrastructureLayer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace InfrastructureLayer.Repository
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;

        #region Fields

        private IDbContextTransaction _dbContextTransaction;

        private const string UserId = nameof(UserId);

        #endregion Fields

        #region Properties

        private bool ActiveFilterEnabled { get; set; } = true;

        private bool SoftDeleteFilterEnabled { get; set; } = true;

        private bool OperatorFilterEnabled { get; set; }

        private bool OperatorEntityEnabled { get; set; }

        private int OperatorEntityId { get; set; }

        #endregion Properties

        #region SaveChanges

        public int SaveChanges()
        {
            BeforeSaveTriggers();

            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            var result = _context.SaveChanges();
            _context.ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaveTriggers();

            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            var result = _context.SaveChanges(acceptAllChangesOnSuccess);
            _context.ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            BeforeSaveTriggers();

            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            var result = await _context.SaveChangesAsync(cancellationToken);
            _context.ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            BeforeSaveTriggers();

            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            var result = await _context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            _context.ChangeTracker.AutoDetectChangesEnabled = true;

            return result;
        }

        #endregion SaveChanges

        #region Utility

        private void BeforeSaveTriggers()
        {
            ValidateEntities();
            SetShadowProperties();
            SetCurrentUserId();
            this.ApplyCorrectYeKe();
        }

        private void ValidateEntities()
        {
            var errors = _context.GetValidationErrors();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                var loggerFactory = _context.GetService<ILoggerFactory>();
                loggerFactory.CheckArgumentIsNull();
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(errors);
                throw new InvalidOperationException(errors);
            }
        }

        private void SetShadowProperties()
        {
            var httpContextAccessor = _context.GetService<IHttpContextAccessor>();
            httpContextAccessor.CheckArgumentIsNull();
            _context.ChangeTracker.SetAuditableEntityPropertyValues(httpContextAccessor);
        }

        private void SetCurrentUserId()
        {
            if (!OperatorEntityEnabled) return;

            var userId = GetCurrentUserId();
            if (userId == 0) return;

            var entities = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var entity in entities)
            {
                var propertyInfo = entity.GetType().GetProperty("UserId");
                if (propertyInfo != null && propertyInfo.PropertyType == typeof(int))
                {
                    propertyInfo.SetValue(entity, userId);
                }
            }
        }



        private int GetCurrentUserId()
        {
            var httpContextAccessor = _context.GetService<IHttpContextAccessor>();
            httpContextAccessor.CheckArgumentIsNull();
            return httpContextAccessor.HttpContext?.User.Identity.GetUserId<int>() ?? 0;
        }

        #endregion Utility

        #region Transaction

        public async Task BeginTransactionAsync()
        {
            if (_dbContextTransaction == null)
                _dbContextTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                if (_dbContextTransaction != null)
                {
                    await _dbContextTransaction.CommitAsync();
                    await DisposeTransactionAsync();
                }
            }
            catch (Exception)
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            if (_dbContextTransaction != null)
            {
                await _dbContextTransaction.RollbackAsync();
                await DisposeTransactionAsync();
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_dbContextTransaction != null)
            {
                await _dbContextTransaction.DisposeAsync();
                _dbContextTransaction = null;
            }
        }

        public void Dispose()
        {
            _dbContextTransaction?.Dispose();
            _dbContextTransaction = null;
        }

        #endregion Transaction

        #region Enabling and Disabling Filters

        public void EnableActiveFilter()
        {
            ActiveFilterEnabled = true;
        }

        public void DisableDeleteFilter()
        {
            SoftDeleteFilterEnabled = false;
        }

        public void EnableOperatorFilter()
        {
            OperatorEntityId = GetCurrentUserId();
            OperatorFilterEnabled = true;
        }

        public void EnableOperatorEntity()
        {
            OperatorEntityEnabled = true;
        }

        #endregion Enabling and Disabling Filters

        #region Disposable

        public IDisposable UseOperatorEntityId(int operatorEntityId)
        {
            var oldOperatorEntityId = OperatorEntityId;
            OperatorEntityId = operatorEntityId;
            return new DisposeAction(() => { OperatorEntityId = oldOperatorEntityId; });
        }

        #endregion Disposable
    }
}