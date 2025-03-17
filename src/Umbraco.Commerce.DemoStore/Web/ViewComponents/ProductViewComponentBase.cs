using Examine;
using Examine.Search;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using Umbraco.Commerce.Common.Models;
using Umbraco.Commerce.DemoStore.Models;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents;

public abstract class ProductViewComponentBase(
    IExamineManager examineManager,
    IUmbracoContextFactory umbracoContextFactory)
    : ViewComponent
{
    protected PagedResult<IProductPage> GetPagedProducts(int? collectionId, string category, int page, int pageSize)
    {
        if (examineManager.TryGetIndex("ExternalIndex", out var index))
        {
            var q = $"+(__NodeTypeAlias:{ProductPage.ModelTypeAlias} __NodeTypeAlias:{MultiVariantProductPage.ModelTypeAlias})";

            if (collectionId.HasValue)
            {
                q += $" +searchPath:{collectionId.Value}";
            }

            if (!category.IsNullOrWhiteSpace())
            {
                q += $" +categoryAliases:\"{category}\"";
            }

            var searcher = index.Searcher;
            var query = searcher.CreateQuery().NativeQuery(q);
            var results = query.OrderBy(new SortableField("name", SortType.String))
                .Execute(QueryOptions.SkipTake(pageSize * (page - 1), pageSize));
            var totalResults = results.TotalItemCount;

            using var ctx = umbracoContextFactory.EnsureUmbracoContext();

            var items = results.ToPublishedSearchResults(ctx.UmbracoContext.Content)
                .Select(x => x.Content)
                .OfType<IProductPage>()
                .OrderBy(x => x.SortOrder);

            return new PagedResult<IProductPage>(totalResults, page, pageSize)
            {
                Items = items
            };
        }

        return new PagedResult<IProductPage>(0, page, pageSize);
    }
}
