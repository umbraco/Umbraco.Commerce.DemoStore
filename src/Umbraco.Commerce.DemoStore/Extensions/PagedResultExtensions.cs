using System.Linq;
using Umbraco.Commerce.Common.Models;

namespace Umbraco.Commerce.DemoStore
{
    public static class PagedResultExtensions
    {
        public static PagedResult<TTo> Cast<TFrom, TTo>(this PagedResult<TFrom> pagedReuslt)
        {
            return new PagedResult<TTo>(pagedReuslt.TotalItems, pagedReuslt.PageNumber, pagedReuslt.PageSize)
            {
                Items = pagedReuslt.Items.Cast<TTo>()
            };
        }
    }
}
