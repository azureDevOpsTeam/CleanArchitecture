#region Usings

using System.Reflection;

#endregion

namespace ApplicationLayer.Extensions.Utilities
{
    internal class PropertyInfoCache
    {
        #region Fields

        private readonly IDictionary<string, PropertyInfo> propertyInfoCache;

        #endregion Fields

        #region Properties

        public PropertyInfo this[string key]
        {
            get => propertyInfoCache[key];
            set => propertyInfoCache[key] = value;
        }

        public bool ContainsKey(string key) => propertyInfoCache.ContainsKey(key);

        #endregion Properties

        #region Methods

        #region Constructors

        public PropertyInfoCache()
        {
            propertyInfoCache = new Dictionary<string, PropertyInfo>();
        }

        #endregion Constructors

        #region Add

        public void Add(string key, PropertyInfo value)
        {
            propertyInfoCache.Add(key, value);
        }

        #endregion Add

        #endregion Methods
    }
}