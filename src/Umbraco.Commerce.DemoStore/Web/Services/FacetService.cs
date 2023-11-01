using Examine;
using Examine.Lucene;
using Examine.Lucene.Search;
using Examine.Search;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Extensions;

namespace Umbraco.Commerce.DemoStore.Web.Services
{
    public interface IFacetService
    {
        IEnumerable<FacetGroup> GetFacets(int? collectionId, string category);
    }

    public class FacetService : IFacetService
    {
        private readonly IExamineManager _examineManager;

        public FacetService(IExamineManager examineManager)
        {
            _examineManager = examineManager;
        }

        public IEnumerable<FacetGroup> GetFacets(int? collectionId, string category)
        {
            if (_examineManager.TryGetIndex(Constants.UmbracoIndexes.ExternalIndexName, out var index))
            {
                var q = $"+(__NodeTypeAlias:{ProductPage.ModelTypeAlias} __NodeTypeAlias:{MultiVariantProductPage.ModelTypeAlias})";

                if (collectionId.HasValue)
                {
                    q += $" +searchPath:{collectionId.Value}";
                }

                if (!category.IsNullOrWhiteSpace())
                {
                    q += $" +categoryAliases:\"{category}\"";
                }

                var searcher = index.Searcher;
                var query = searcher.CreateQuery().NativeQuery(q);

                var results = query.OrderBy(new SortableField("name", SortType.String))
                    .WithFacets(facets => facets
                        .FacetString("isGiftCard", null, new[] { "1" })
                        //.FacetLongRange("isGiftCard", new Int64Range[] {
                        //    new Int64Range("no", 0, true, 1, false),
                        //    new Int64Range("yes", 0, false, 1, true)
                        //})
                        .FacetDoubleRange("price_GBP", new DoubleRange[] {
                            new DoubleRange("0-10", 0, true, 10, true),
                            new DoubleRange("11-20", 11, true, 20, true),
                            new DoubleRange("20-30", 21, true, 30, true),
                            new DoubleRange("30-40", 31, true, 40, true),
                            new DoubleRange("40-50", 41, true, 50, true)
                        })) // Get facets of the price field
                    .Execute(QueryOptions.SkipTake(0, 1000));

                var facets = results.GetFacets();

                return MapFacets(facets.ToList());
            }

            return Enumerable.Empty<FacetGroup>();
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
                        //Value = f.Value,
                        Count = (long)f.Value
                    })
                    .OrderBy(f => f.Name)
                });

            return mappedFacets;
        }
    }
}
