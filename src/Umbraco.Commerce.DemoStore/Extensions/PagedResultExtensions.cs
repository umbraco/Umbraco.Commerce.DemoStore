using System.Linq;
using Umbraco.Commerce.Common.Models;

namespace Umbraco.Commerce.DemoStore;

public static class PagedResultExtensions
{
    public static PagedResult<TTo> CastItems<TFrom, TTo>(this PagedResult<TFrom> pagedResult) =>
        new(pagedResult.TotalItems, pagedResult.PageNumber, pagedResult.PageSize)
        {
            Items = pagedResult.Items.Cast<TTo>()
        };
}
