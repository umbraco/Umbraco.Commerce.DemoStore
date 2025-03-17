using Examine;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Commerce.DemoStore.Web.Extensions;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents;

[ViewComponent]
public class ProductListByCollectionViewComponent(
    IExamineManager examineManager,
    IUmbracoContextFactory umbracoContextFactory)
    : ProductViewComponentBase(examineManager, umbracoContextFactory)
{
    public IViewComponentResult Invoke(int collectionId)
    {
        var p = Request.Query.GetInt("p", 1);
        var ps = Request.Query.GetInt("ps", 12);
        var model = GetPagedProducts(collectionId, null, p, ps);
        return View("PagedProductList", model);
    }
}
