using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Commerce.DemoStore.Models;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents;

[ViewComponent]
public class FeaturedProductsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPublishedContent currentPage)
    {
        var featuredProducts = currentPage.GetHomePage()
            .FeaturedProducts!.OfType<IProductPage>();

        return View("ProductList", featuredProducts);
    }
}
