using Microsoft.AspNetCore.Mvc;
using Umbraco.Commerce.DemoStore.Web.Services;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents
{
    [ViewComponent]
    public class FacetViewComponent : ViewComponent
    {
        private readonly IFacetService _facetService;

        public FacetViewComponent(IFacetService facetService)
        {
            _facetService = facetService;
        }

        public IViewComponentResult Invoke(int? collectionId, string category)
        {
            var facets = _facetService.GetFacets(collectionId, category);

            return View("Facets", facets);
        }
    }
}
