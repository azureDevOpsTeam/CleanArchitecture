#region Usings

using System.Globalization;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Security.Principal;

#endregion end of Usings

namespace DomainLayer.Extensions
{
    public static class IdentityExtension
    {
        #region Methods

        #region Utility

        //public static void AddErrorsFromResult(this ModelStateDictionary modelStat, IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //        modelStat.AddModelError(string.Empty, error.Description);
        //}

        //public static string DumpErrors(this IdentityResult result, bool useHtmlNewLine = false)
        //{
        //    var results = new StringBuilder();
        //    if (result.Succeeded) return results.ToString();
        //    foreach (var error in result.Errors)
        //    {
        //        var errorDescription = error.Description;
        //        if (string.IsNullOrWhiteSpace(errorDescription))
        //            continue;

        //        results.AppendLine(!useHtmlNewLine ? errorDescription : $"{errorDescription}<br/>");
        //    }
        //    return results.ToString();
        //}

        public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
        {
            return identity?.FindFirst(claimType)?.Value;
        }

        #endregion Utility

        #region Get

        public static T GetUserId<T>(this IIdentity identity) where T : IConvertible
        {
            var firstValue = identity?.GetUserClaimValue(ClaimTypes.NameIdentifier);
            return firstValue != null ? (T)Convert.ChangeType(firstValue, typeof(T), CultureInfo.InvariantCulture) : default(T);
        }

        public static string GetUserClaimValue(this IIdentity identity, string claimType)
        {
            var currentIdentity = identity as ClaimsIdentity;
            return currentIdentity?.FindFirstValue(claimType);
        }

        public static string GetUserId(this IIdentity identity)
        {
            return identity?.GetUserClaimValue(ClaimTypes.NameIdentifier);
        }

        public static string GetUserFirstName(this IIdentity identity)
        {
            return identity?.GetUserClaimValue(ClaimTypes.GivenName);
        }

        public static string GetUserLastName(this IIdentity identity)
        {
            return identity?.GetUserClaimValue(ClaimTypes.Surname);
        }

        public static string GetUserFullName(this IIdentity identity)
        {
            return $"{GetUserFirstName(identity)} {GetUserLastName(identity)}";
        }

        public static string GetUserDisplayName(this IIdentity identity)
        {
            var fullName = GetUserFullName(identity);
            return string.IsNullOrWhiteSpace(fullName) ? GetUserName(identity) : fullName;
        }

        public static string GetUserName(this IIdentity identity)
        {
            return identity?.GetUserClaimValue(ClaimTypes.Name);
        }

        public static string GetMacAddress()
        {
            var macAddress = string.Empty;
            foreach (var network in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (network.OperationalStatus == OperationalStatus.Up)
                {
                    macAddress += network.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddress;
        }

        #endregion Get

        #endregion Methods
    }
}