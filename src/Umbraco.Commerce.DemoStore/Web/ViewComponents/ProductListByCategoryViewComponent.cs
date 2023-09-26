using Examine;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Commerce.DemoStore.Web.Extensions;
using Umbraco.Commerce.DemoStore.Web.Services;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents
{
    [ViewComponent]
    public class ProductListByCategoryViewComponent : ProductViewComponentBase
    {
        public ProductListByCategoryViewComponent(IExamineManager examineManager, IUmbracoContextFactory umbracoContextFactory)
            : base(examineManager, umbracoContextFactory)
        {
        }

        public IViewComponentResult Invoke(string category)
        {
            var p = Request.Query.GetInt("p", 1);
            var ps = Request.Query.GetInt("ps", 12);

            var products = GetPagedProducts(null, category, p, ps);

            return View("PagedProductList", products);
        }
    }
}
