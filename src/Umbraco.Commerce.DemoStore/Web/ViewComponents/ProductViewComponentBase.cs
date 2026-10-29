using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Search;
using Umbraco.Cms.Core.Search.Querying;
using Umbraco.Cms.Core.Search.Querying.Filtering;
using Umbraco.Cms.Core.Search.Querying.Sorting;
using Umbraco.Cms.Core.Services;
using Umbraco.Commerce.Common.Models;
using Umbraco.Commerce.DemoStore.Models;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents;

public abstract class ProductViewComponentBase(
    ISearcherResolver searcherResolver,
    IPublishedContentCache publishedContentCache,
    IContentTypeService contentTypeService)
    : ViewComponent
{
    protected async Task<PagedResult<IProductPage>> GetPagedProductsAsync(Guid? collectionKey, Guid? categoryKey, int page, int pageSize)
    {
        ISearcher? searcher = searcherResolver.GetSearcher(Constants.Search.IndexAliases.PublishedContent);
        if (searcher == null)
        {
            return new PagedResult<IProductPage>(0, page, pageSize);
        }

        var productTypeKeys = new[] { ProductPage.ModelTypeAlias, MultiVariantProductPage.ModelTypeAlias }
            .Select(x => contentTypeService.Get(x)?.Key.ToString("D"))
            .OfType<string>()
            .ToArray();

        var filters = new List<Filter> { new KeywordFilter(Constants.Search.FieldNames.ContentTypeId, productTypeKeys, false) };

        if (collectionKey.HasValue)
        {
            filters.Add(new KeywordFilter(Constants.Search.FieldNames.PathIds, [collectionKey.Value.ToString("D")], false));
        }

        if (categoryKey.HasValue)
        {
            // The multi node tree picker indexes the keys of the picked nodes in a field named after the property
            filters.Add(new KeywordFilter("categories", [categoryKey.Value.ToString("D")], false));
        }

        SearchResult results = await searcher.SearchAsync(
            Constants.Search.IndexAliases.PublishedContent,
            filters: filters,
            sorters: [new KeywordSorter(Constants.Search.FieldNames.Name, Direction.Ascending)],
            culture: CultureInfo.CurrentUICulture.Name,
            accessContext: AccessContext.BypassProtection(),
            skip: pageSize * (page - 1),
            take: pageSize);

        // Searches only return keys, so read the actual content from the published cache
        var items = new List<IProductPage>();
        foreach (Document document in results.Documents)
        {
            IPublishedContent? content = await publishedContentCache.GetByIdAsync(document.Id);
            if (content is IProductPage product)
            {
                items.Add(product);
            }
        }

        return new PagedResult<IProductPage>(results.Total, page, pageSize)
        {
            Items = items.OrderBy(x => x.SortOrder)
        };
    }
}
