using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Commerce.Extensions;

namespace Umbraco.Commerce.DemoStore;

public static partial class PublishedContentExtensions
{
    public static StoreReadOnly? GetStore(this IPublishedContent content) => content.AncestorOrSelf<HomePage>()?.Store;

    public static Task<OrderReadOnly?> GetCurrentOrderAsync(this IPublishedContent content) => content is CheckoutConfirmationPage
        ? UmbracoCommerceApi.Instance.GetCurrentFinalizedOrderAsync(content.GetStore()!.Id)
        : UmbracoCommerceApi.Instance.GetCurrentOrderAsync(content.GetStore()!.Id);
    
   
}
