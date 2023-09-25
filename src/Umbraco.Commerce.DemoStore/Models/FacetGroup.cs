using System.Collections.Generic;

namespace Umbraco.Commerce.DemoStore.Models
{
    public class FacetGroup
    {
        public string Name { get; set; }

        public IEnumerable<Facet> Facets { get; set; }
    }

    public class Facet
    {
        public string Name { get; set; }

        public float Value { get; set; }

        public int Count { get; set; }
    }
}
