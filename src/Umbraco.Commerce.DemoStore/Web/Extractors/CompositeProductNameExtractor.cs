using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Commerce.Cms.Extractors;
using Umbraco.Commerce.Cms.Helpers;

namespace Umbraco.Commerce.DemoStore.Web.Extractors;

public class CompositeProductNameExtractor(PublishedContentHelper publishedContentHelper,
    IDocumentNavigationQueryService documentNavigationQueryService,
    IUmbracoContextFactory umbracoContextFactory)
    : UmbracoProductNameExtractor(publishedContentHelper)
{
    public override string ExtractProductName(IPublishedContent content, IPublishedElement variant, string languageIsoCode)
    {
        if (documentNavigationQueryService.TryGetParentKey(content.Key,out var parentKey))
        {
            var umbContextRef = umbracoContextFactory.EnsureUmbracoContext();
            var parent = umbContextRef.UmbracoContext.Content.GetById(parentKey!.Value)!;

            var productNamePrefix = parent.Name;

            if (content.ContentType.Alias == ProductVariant.ModelTypeAlias && documentNavigationQueryService.TryGetParentKey(parent.Key, out var grandParentKey))
            {
                var grandParent = umbContextRef.UmbracoContext.Content.GetById(grandParentKey!.Value)!;
                productNamePrefix = $"{grandParent.Name} - {parent.Name}";
            }

            return $"{productNamePrefix} - {base.ExtractProductName(content, variant, languageIsoCode)}";
        }

        return base.ExtractProductName(content, variant, languageIsoCode);
    }
}
