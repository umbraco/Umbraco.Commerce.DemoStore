using Examine;
using Examine.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Web;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Commerce.DemoStore.Web.Extensions;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents
{
    [ViewComponent]
    public class ProductListByCollectionViewComponent : ProductViewComponentBase
    {
        public ProductListByCollectionViewComponent(IExamineManager examineManager, IUmbracoContextFactory umbracoContextFactory)
            : base(examineManager, umbracoContextFactory)
        { }

        public IViewComponentResult Invoke(int collectionId)
        {
            var p = Request.Query.GetInt("p", 1);
            var ps = Request.Query.GetInt("ps", 12);

            var products = GetPagedProducts(collectionId, null, p, ps, out IEnumerable<IFacetResult> facets);

            var model = new ProductListViewModel
            {
                Facets = MapFacets(facets.ToList()),
                Products = products
            };

            return View("PagedProductList", model);
        }

        private static IEnumerable<FacetGroup> MapFacets(IList<IFacetResult> facets)
        {
            var mappedFacets = facets
                .Select((x, i) => new FacetGroup()
                {
                    Name = i == 0 ? "Gift Card" : "Price", //GetFacetName(x),
                    Facets = x.Select(f => new Facet()
                    {
                        Name = f.Label,
                        Value = "",
                        Count = (long)f.Value
                    })
                    .OrderBy(f => f.Name)
                });

            return mappedFacets;
        }

    }
}
