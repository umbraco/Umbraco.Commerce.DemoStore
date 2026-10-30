using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Search;
using Umbraco.Cms.Core.Services;
using Umbraco.Commerce.DemoStore.Web.Extensions;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents;

[ViewComponent]
public class ProductListByCollectionViewComponent(
    ISearcherResolver searcherResolver,
    IPublishedContentCache publishedContentCache,
    IContentTypeService contentTypeService)
    : ProductViewComponentBase(searcherResolver, publishedContentCache, contentTypeService)
{
    public async Task<IViewComponentResult> InvokeAsync(Guid collectionKey)
    {
        var p = Request.Query.GetInt("p", 1);
        var ps = Request.Query.GetInt("ps", 12);
        var model = await GetPagedProductsAsync(collectionKey, null, p, ps);
        return View("PagedProductList", model);
    }
}
