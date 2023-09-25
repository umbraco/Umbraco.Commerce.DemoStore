using Examine;
using Examine.Search;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using Umbraco.Commerce.Common.Models;
using Umbraco.Commerce.DemoStore.Models;
using Examine.Lucene;
using Umbraco.Cms.Core;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents
{
    public abstract class ProductViewComponentBase : ViewComponent
    {
        private readonly IExamineManager _examineManager;
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public ProductViewComponentBase(IExamineManager examineManager, IUmbracoContextFactory umbracoContextFactory)
        {
            _examineManager = examineManager;
            _umbracoContextFactory = umbracoContextFactory;
        }

        protected PagedResult<ProductPage> GetPagedProducts(int? collectionId, string category, int page, int pageSize, out IEnumerable<IFacetResult> facets)
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
                        .FacetLongRange("isGiftCard", new Int64Range[] {
                            new Int64Range("no", 0, true, 1, false),
                            new Int64Range("yes", 0, false, 1, true)
                        })
                        .FacetDoubleRange("price_GBP", new DoubleRange[] {
                            new DoubleRange("0-10", 0, true, 10, true),
                            new DoubleRange("11-20", 11, true, 20, true),
                            new DoubleRange("20-30", 21, true, 30, true),
                            new DoubleRange("30-40", 31, true, 40, true),
                            new DoubleRange("40-50", 41, true, 50, true)
                        })) // Get facets of the price field
                    .Execute(QueryOptions.SkipTake(pageSize * (page - 1), pageSize));

                facets = results.GetFacets();

                var totalResults = results.TotalItemCount;

                using (var ctx = _umbracoContextFactory.EnsureUmbracoContext())
                {
                    var items = results.ToPublishedSearchResults(ctx.UmbracoContext.Content)
                        .Select(x => x.Content)
                        .OfType<ProductPage>()
                        .OrderBy(x => x.SortOrder);

                    return new PagedResult<ProductPage>(totalResults, page, pageSize)
                    {
                        Items = items
                    };
                }
            }

            facets = null;

            return new PagedResult<ProductPage>(0, page, pageSize);
        }
    }
}
