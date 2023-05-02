using Examine;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Commerce.DemoStore.Models;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents
{
    [ViewComponent]
    public class FeaturedProductsViewComponent : ProductViewComponentBase
    {
        public FeaturedProductsViewComponent(IExamineManager examineManager, IUmbracoContextFactory umbracoContextFactory)
            : base(examineManager, umbracoContextFactory)
        { }

        public IViewComponentResult Invoke(IPublishedContent currentPage)
        {
            var featuredProducts = currentPage.GetHomePage()
                .FeaturedProducts.OfType<ProductPage>();

            return View("ProductList", featuredProducts);
        }
    }
}
