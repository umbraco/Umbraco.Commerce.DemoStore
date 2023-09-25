using Examine;
using Examine.Search;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Web;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Commerce.DemoStore.Web.Extensions;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents
{
    [ViewComponent]
    public class ProductListByCategoryViewComponent : ProductViewComponentBase
    {
        public ProductListByCategoryViewComponent(IExamineManager examineManager, IUmbracoContextFactory umbracoContextFactory)
            : base(examineManager, umbracoContextFactory)
        { }

        public IViewComponentResult Invoke(string category)
        {
            var p = Request.Query.GetInt("p", 1);
            var ps = Request.Query.GetInt("ps", 12);

            var products = GetPagedProducts(null, category, p, ps, out IEnumerable<IFacetResult> facets);

            var model = new ProductListViewModel
            {
                Facets = MapFacets(facets.Select(x => x as FacetResult).ToList()),
                Products = products
            };

            return View("PagedProductList", model);
        }

        private static IEnumerable<FacetGroup> MapFacets(IList<FacetResult> facets)
        {
            var mappedFacets = facets
                .Select(x => new FacetGroup()
                {
                    Name = "", //GetFacetName(x),
                    Facets = x.Select(f => new Facet()
                    {
                        Name = f.Label,
                        //Value = f.Value,
                        //Count = f.Count
                    })
                    .OrderBy(f => f.Name)
                });

            return mappedFacets;
        }
    }
}
