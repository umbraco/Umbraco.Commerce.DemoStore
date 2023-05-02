using System.Threading;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Commerce.Extensions;

namespace Umbraco.Commerce.DemoStore
{
    public static class PublishedContentExtensions
    {
        public static Page AsPage(this IPublishedContent content)
        {
            return ((Page)content);
        }

        public static HomePage GetHomePage(this IPublishedContent content)
        {
            return content.AsPage().HomePage;
        }

        public static StoreReadOnly GetStore(this IPublishedContent content)
        {
            return content.AncestorOrSelf<HomePage>()?.Store;
        }

        public static OrderReadOnly GetCurrentOrder(this IPublishedContent content)
        {
            return UmbracoCommerceApi.Instance.GetCurrentOrder(content.GetStore().Id);
        }

        public static string GetProductReference(this IProductComp content)
        {
            return content.Key.ToString();
        }

        public static IProductSnapshot AsProduct(this IProductComp content)
        {
            var page = content as IPublishedContent;
            if (page == null)
                return null;

            var store = page.GetStore();

            return UmbracoCommerceApi.Instance.GetProduct(store.Id, content.GetProductReference(), Thread.CurrentThread.CurrentCulture.Name);
        }

        public static IProductSnapshot AsProduct(this IProductComp variant, IProductComp parent)
        {
            var page = parent as IPublishedContent;
            if (page == null)
                page = variant as IPublishedContent;
            if (page == null)
                return null;

            var store = page.GetStore();

            return UmbracoCommerceApi.Instance.GetProduct(store.Id, parent.GetProductReference(), variant.GetProductReference(), Thread.CurrentThread.CurrentCulture.Name);
        }

        public static Price CalculatePrice(this IProductComp content)
        {
            return content.AsProduct()?.CalculatePrice();
        }

        public static Price CalculatePrice(this IProductComp variant, IProductComp parent)
        {
            return variant.AsProduct(parent)?.CalculatePrice();
        }

        public static CheckoutPage GetCheckoutPage(this CheckoutStepPage content)
        {
            return content.AncestorOrSelf<CheckoutPage>();
        }
    }
}
