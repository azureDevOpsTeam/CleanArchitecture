#region Usings

using System.Globalization;
using System.Reflection;
using System.Text;

#endregion

namespace ApplicationLayer.Extensions.Utilities
{
    public class PropertyReflector
    {
        #region Fields

        private const char PropertyNameSeparator = '.';

        private static readonly object[] _noParams = new object[0];
        private static readonly Type[] _noTypeParams = new Type[0];

        private readonly IDictionary<Type, PropertyInfoCache> _propertyCache = new Dictionary<Type, PropertyInfoCache>();
        private readonly IDictionary<Type, ConstructorInfo> _constructorCache = new Dictionary<Type, ConstructorInfo>();
        private readonly object _lockObject = new object();

        #endregion Fields

        #region Get

        public Type GetType(Type targetType, string propertyName)
        {
            if (propertyName.IndexOf(PropertyNameSeparator) > -1)
            {
                var propertyList = propertyName.Split(PropertyNameSeparator);
                return propertyList.Aggregate(targetType, GetTypeImpl);
            }
            return GetTypeImpl(targetType, propertyName);
        }

        public object GetValue(object target, string propertyName)
        {
            if (propertyName.IndexOf(PropertyNameSeparator) > -1)
            {
                var propertyList = propertyName.Split(PropertyNameSeparator);
                foreach (var currentProperty in propertyList)
                {
                    target = GetValueImpl(target, currentProperty);
                    if (target == null)
                        return null;
                }
                return target;
            }
            return GetValueImpl(target, propertyName);
        }

        private object GetValueImpl(object target, string propertyName)
        {
            return GetPropertyInfo(target.GetType(), propertyName).GetValue(target, _noParams);
        }

        private Type GetTypeImpl(Type targetType, string propertyName)
        {
            return GetPropertyInfo(targetType, propertyName).PropertyType;
        }

        private PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            var propertyInfoCache = GetPropertyInfoCache(type);
            if (!propertyInfoCache.ContainsKey(propertyName))
            {
                var propertyInfo = GetBestMatchingProperty(propertyName, type);
                if (propertyInfo == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unable to find public property named {0} on type {1}", propertyName, type.FullName), propertyName);
                }
                propertyInfoCache.Add(propertyName, propertyInfo);
            }
            return propertyInfoCache[propertyName];
        }

        private static PropertyInfo GetBestMatchingProperty(string propertyName, Type type)
        {
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            PropertyInfo bestMatch = null;
            var bestMatchDistance = int.MaxValue;
            foreach (var info in propertyInfos)
            {
                if (info.Name == propertyName)
                {
                    int distance = CalculateDistance(type, info.DeclaringType);
                    if (distance == 0)
                        return info;
                    if (distance > 0 && distance < bestMatchDistance)
                        bestMatch = info;
                    bestMatchDistance = distance;
                }
            }
            return bestMatch;
        }

        private static string GetPropertyNameString(IReadOnlyList<string> propertyList, int level)
        {
            var currentFullPropertyName = new StringBuilder();
            for (var index = 0; index <= level; index++)
            {
                if (index > 0) currentFullPropertyName.Append(PropertyNameSeparator);
                currentFullPropertyName.Append(propertyList[index]);
            }
            return currentFullPropertyName.ToString();
        }

        private PropertyInfoCache GetPropertyInfoCache(Type type)
        {
            if (!_propertyCache.ContainsKey(type))
                lock (_lockObject)
                    if (!_propertyCache.ContainsKey(type))
                    {
                        _propertyCache.Add(type, new PropertyInfoCache());
                    }
            return _propertyCache[type];
        }

        #endregion Get

        #region Set

        public void SetValue(object target, string propertyName, object value)
        {
            if (propertyName.IndexOf(PropertyNameSeparator) > -1)
            {
                var originalTarget = target;
                var propertyList = propertyName.Split(PropertyNameSeparator);
                for (var index = 0; index < propertyList.Length - 1; index++)
                {
                    propertyName = propertyList[index];
                    target = GetValueImpl(target, propertyName);
                    if (target == null)
                    {
                        var currentFullPropertyNameString = GetPropertyNameString(propertyList, index);
                        target = Construct(GetType(originalTarget.GetType(), currentFullPropertyNameString));
                        SetValue(originalTarget, currentFullPropertyNameString, target);
                    }
                }
                propertyName = propertyList[propertyList.Length - 1];
            }
            SetValueImpl(target, propertyName, value);
        }

        private void SetValueImpl(object target, string propertyName, object value)
        {
            GetPropertyInfo(target.GetType(), propertyName).SetValue(target, value, _noParams);
        }

        #endregion Set

        #region Utility

        private static int CalculateDistance(Type targetObjectType, Type baseType)
        {
            if (!baseType.GetTypeInfo().IsInterface)
            {
                var currType = targetObjectType;
                var level = 0;
                while (currType != null)
                {
                    if (baseType == currType) return level;
                    currType = currType.GetTypeInfo().BaseType;
                    level++;
                }
            }
            return -1;
        }

        private object Construct(Type type)
        {
            if (!_constructorCache.ContainsKey(type))
            {
                lock (_lockObject)
                {
                    if (!_constructorCache.ContainsKey(type))
                    {
                        var constructorInfo = type.GetConstructor(_noTypeParams);
                        if (constructorInfo == null)
                            throw new Exception(string.Format(CultureInfo.InvariantCulture, "Unable to construct instance, no parameterless constructor found in type {0}", type.FullName));
                        _constructorCache.Add(type, constructorInfo);
                    }
                }
            }
            return _constructorCache[type].Invoke(_noParams);
        }

        #endregion Utility
    }
}