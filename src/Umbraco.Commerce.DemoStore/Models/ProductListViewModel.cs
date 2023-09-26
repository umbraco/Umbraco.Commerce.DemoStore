using System.Collections.Generic;
using Umbraco.Commerce.Common.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<FacetGroup> Facets { get; set; }

        public PagedResult<ProductPage> Products { get; set; }
    }
}
