using Examine;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Commerce.DemoStore.Web.Extensions;
using Umbraco.Commerce.DemoStore.Web.Services;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents
{
    [ViewComponent]
    public class ProductListByCollectionViewComponent : ProductViewComponentBase
    {
        public ProductListByCollectionViewComponent(IExamineManager examineManager, IUmbracoContextFactory umbracoContextFactory)
            : base(examineManager, umbracoContextFactory)
        {
        }

        public IViewComponentResult Invoke(int collectionId)
        {
            var p = Request.Query.GetInt("p", 1);
            var ps = Request.Query.GetInt("ps", 12);

            var products = GetPagedProducts(collectionId, null, p, ps);

            return View("PagedProductList", products);
        }
    }
}
