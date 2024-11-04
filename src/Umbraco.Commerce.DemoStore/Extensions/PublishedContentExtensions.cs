using System.Threading;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Commerce.Extensions;

namespace Umbraco.Commerce.DemoStore;

public static class PublishedContentExtensions
{
    public static Page AsPage(this IPublishedContent content) => (Page)content;

    public static HomePage GetHomePage(this IPublishedContent content) => content.AsPage().HomePage;

    public static StoreReadOnly? GetStore(this IPublishedContent content) => content.AncestorOrSelf<HomePage>()?.Store;

    public static Task<OrderReadOnly?> GetCurrentOrderAsync(this IPublishedContent content) => UmbracoCommerceApi.Instance.GetCurrentOrderAsync(content.GetStore()!.Id);

    public static string GetProductReference(this IProductComp content) => content.Key.ToString();

    public static async Task<IProductSnapshot?> AsProductAsync(this IProductComp content)
    {
        if (content is not IPublishedContent page)
        {
            return null;
        }

        var store = page.GetStore()!;

        return await UmbracoCommerceApi.Instance.GetProductAsync(store.Id, content.GetProductReference(), Thread.CurrentThread.CurrentCulture.Name);
    }

    public static async Task<IProductSnapshot?> AsProductAsync(this IProductComp variant, IProductComp parent)
    {
        var page = parent as IPublishedContent;
        if (page == null)
        {
            page = variant as IPublishedContent;
        }

        if (page == null)
        {
            return null;
        }

        var store = page.GetStore()!;

        return await UmbracoCommerceApi.Instance.GetProductAsync(store.Id, parent.GetProductReference(), variant.GetProductReference(), Thread.CurrentThread.CurrentCulture.Name);
    }

    public static async Task<IPrice> CalculatePriceAsync(this IProductComp content) =>
        (await (await content.AsProductAsync()).TryCalculatePriceAsync()).ResultOrThrow("Unable to calculate price");

    public static async Task<IPrice> CalculatePriceAsync(this IProductComp variant, IProductComp parent) =>
        (await (await variant.AsProductAsync(parent)).TryCalculatePriceAsync()).ResultOrThrow("Unable to calculate price");

    public static CheckoutPage GetCheckoutPage(this CheckoutStepPage content) =>
        content.AncestorOrSelf<CheckoutPage>()!;
}
