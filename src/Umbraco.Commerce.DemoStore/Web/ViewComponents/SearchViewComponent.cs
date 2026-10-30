using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Search;
using Umbraco.Cms.Core.Search.Querying;
using Umbraco.Extensions;
using Umbraco.Commerce.Common.Models;
using Umbraco.Commerce.DemoStore.Web.Extensions;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents;

[ViewComponent]
public class SearchViewComponent(
    ISearcherResolver searcherResolver,
    IPublishedContentCache publishedContentCache)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var q = Request.Query["q"].ToString();
        var p = Request.Query.GetInt("p", 1);
        var ps = Request.Query.GetInt("ps", 12);

        var result = new PagedResult<IPublishedContent>(0, 1, ps);

        ISearcher? searcher = searcherResolver.GetSearcher(Constants.Search.IndexAliases.PublishedContent);
        if (!q.IsNullOrWhiteSpace() && searcher != null)
        {
            SearchResult results = await searcher.SearchAsync(
                Constants.Search.IndexAliases.PublishedContent,
                query: q,
                culture: CultureInfo.CurrentUICulture.Name,
                accessContext: AccessContext.BypassProtection(),
                skip: ps * (p - 1),
                take: ps);

            // Searches only return keys, so read the actual content from the published cache
            var items = new List<IPublishedContent>();
            foreach (Document document in results.Documents)
            {
                IPublishedContent? content = await publishedContentCache.GetByIdAsync(document.Id);
                // Must have a template and not be hidden. Checked here as umbracoNaviHide is not indexed when no content sets it
                if (content?.TemplateId > 0 && !content.Value<bool>("umbracoNaviHide"))
                {
                    items.Add(content);
                }
            }

            result = new PagedResult<IPublishedContent>(results.Total, p, ps)
            {
                Items = items
            };
        }

        return View("SearchResults", result);
    }
}
