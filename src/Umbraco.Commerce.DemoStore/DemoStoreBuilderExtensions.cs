using Umbraco.Commerce.Core.Events.Notification;
using Umbraco.Commerce.DemoStore.Events;
using Umbraco.Commerce.DemoStore.Web.Extractors;
using Umbraco.Commerce.Cms.Extractors;
using Umbraco.Commerce.Extensions;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Notifications;

namespace Umbraco.Commerce.DemoStore
{
    public static class DemoStoreBuilderExtensions
    {
        public static IUmbracoBuilder AddDemoStore(this IUmbracoBuilder umbracoBuilder)
        {
            umbracoBuilder.AddUmbracoCommerce(b =>
            {
                // Enable SQLite support
                b.AddSQLite();

                // Replace the umbraco product name extractor with one that supports child variants
                b.Services.AddUnique<IUmbracoProductNameExtractor, CompositeProductNameExtractor>();

                // Register event handlers
                b.WithNotificationEvent<OrderProductAddingNotification>()
                    .RegisterHandler<OrderProductAddingHandler>();

                b.WithNotificationEvent<OrderLineChangingNotification>()
                    .RegisterHandler<OrderLineChangingHandler>();

                b.WithNotificationEvent<OrderLineRemovingNotification>()
                    .RegisterHandler<OrderLineRemovingHandler>();

                b.WithNotificationEvent<OrderPaymentCountryRegionChangingNotification>()
                    .RegisterHandler<OrderPaymentCountryRegionChangingHandler>();

                b.WithNotificationEvent<OrderShippingCountryRegionChangingNotification>()
                    .RegisterHandler<OrderShippingCountryRegionChangingHandler>();

                b.WithNotificationEvent<OrderShippingMethodChangingNotification>()
                    .RegisterHandler<OrderShippingMethodChangingHandler>();

            });

            umbracoBuilder.AddNotificationHandler<UmbracoApplicationStartingNotification, TransformExamineValues>();

            return umbracoBuilder;
        }
    }
}
