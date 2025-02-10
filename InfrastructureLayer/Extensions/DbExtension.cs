#region Usings

using ApplicationLayer.Extensions.Utilities;
using DNTPersianUtils.Core;
using DomainLayer;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

#endregion

namespace InfrastructureLayer.Extensions
{
    public static class DbExtension
    {
        #region Methods

        #region GetValidationErrors & ApplyCorrectYeKe

        public static string GetValidationErrors(this DbContext context)
        {
            var errors = new StringBuilder();
            var entities = context.ChangeTracker.Entries().Where(row => row.State == EntityState.Added || row.State == EntityState.Modified).Select(e => e.Entity);
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(entity, validationContext, validationResults, validateAllProperties: true))
                    foreach (var validationResult in validationResults)
                    {
                        var name = validationResult.MemberNames.Aggregate((s1, s2) => $"{s1}, {s2}");
                        errors.AppendFormat("{0}: {1}", name, validationResult.ErrorMessage);
                    }
            }

            return errors.ToString();
        }

        public static void ApplyCorrectYeKe(this DbContext dbContext)
        {
            if (dbContext == null) return;

            var changedEntities = dbContext.ChangeTracker.Entries().Where(row => row.State == EntityState.Added || row.State == EntityState.Modified);

            foreach (var item in changedEntities)
            {
                var entity = item.Entity;
                if (item.Entity == null) continue;

                var propertyInfos = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(row => row.CanRead && row.CanWrite && row.PropertyType == typeof(string));

                var propertyReflector = new PropertyReflector();

                foreach (var propertyInfo in propertyInfos)
                {
                    var propName = propertyInfo.Name;
                    var value = propertyReflector.GetValue(entity, propName);
                    if (value != null)
                    {
                        var strValue = value.ToString();
                        var newVal = strValue.ApplyCorrectYeKe();
                        if (newVal == strValue) continue;
                        propertyReflector.SetValue(entity, propName, newVal);
                    }
                }
            }
        }

        #endregion GetValidationErrors & ApplyCorrectYeKe

        #region Can Delete

        public static bool CanDelete(this object entity) => entity.GetType().GetProperties().Where(row => row.IsDefined(typeof(SoftDelete))).Select(row => row.GetValue(entity)).OfType<IEnumerable<object>>().All(row => !row.Any());

        #endregion Can Delete

        #endregion Methods
    }
}