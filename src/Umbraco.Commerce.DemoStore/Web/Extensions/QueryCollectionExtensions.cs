using Microsoft.AspNetCore.Http;

namespace Umbraco.Commerce.DemoStore.Web.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static int GetInt(this IQueryCollection query, string key, int? fallback)
        {
            if (query.ContainsKey(key))
                return int.Parse("0" + query[key]);

            return fallback ?? 0;
        }
    }
}
