using Examine;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Commerce.DemoStore.Models;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents;

[ViewComponent]
public class FeaturedProductsViewComponent(
    IExamineManager examineManager,
    IUmbracoContextFactory umbracoContextFactory)
    : ProductViewComponentBase(examineManager, umbracoContextFactory)
{
    public IViewComponentResult Invoke(IPublishedContent currentPage)
    {
        IEnumerable<ProductPage> featuredProducts = currentPage.GetHomePage()
            .FeaturedProducts!.OfType<ProductPage>();

        return View("ProductList", featuredProducts);
    }
}
