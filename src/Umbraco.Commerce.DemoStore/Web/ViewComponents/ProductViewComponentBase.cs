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
    protected PagedResult<ProductPage> GetPagedProducts(int? collectionId, string category, int page, int pageSize)
    {
        if (examineManager.TryGetIndex("ExternalIndex", out IIndex? index))
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

            ISearcher? searcher = index.Searcher;
            IBooleanOperation? query = searcher.CreateQuery().NativeQuery(q);
            ISearchResults? results = query.OrderBy(new SortableField("name", SortType.String))
                .Execute(QueryOptions.SkipTake(pageSize * (page - 1), pageSize));
            var totalResults = results.TotalItemCount;

            using UmbracoContextReference ctx = umbracoContextFactory.EnsureUmbracoContext();

            IOrderedEnumerable<ProductPage> items = results.ToPublishedSearchResults(ctx.UmbracoContext.Content)
                .Select(x => x.Content)
                .OfType<ProductPage>()
                .OrderBy(x => x.SortOrder);

            return new PagedResult<ProductPage>(totalResults, page, pageSize)
            {
                Items = items
            };
        }

        return new PagedResult<ProductPage>(0, page, pageSize);
    }
}
