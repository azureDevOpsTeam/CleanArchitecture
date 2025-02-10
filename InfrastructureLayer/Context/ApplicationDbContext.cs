#region Usings

using DomainLayer;
using DomainLayer.Common.BaseEntities;
using DomainLayer.Entities;
using InfrastructureLayer.Convention;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace InfrastructureLayer.Context
{
    public class ApplicationDbContext : DbContext
    {
        #region Constructors

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #endregion Constructors

        #region ModelCreating & OnConfiguring

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Ignore<BaseInformationLocalizationProperty>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserAccount).GetTypeInfo().Assembly);
            modelBuilder.SetGlobalConvention();
            modelBuilder.AddAuditableShadowProperties();

            SetGlobalFilter.ApplyQueryFilters(modelBuilder);
        }

        #endregion ModelCreating & OnConfiguring
    }
}