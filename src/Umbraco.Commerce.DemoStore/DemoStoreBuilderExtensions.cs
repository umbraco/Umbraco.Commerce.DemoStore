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
            umbracoBuilder.AddUmbracoCommerce(v =>
            {
                // Enable SQLite support
                v.AddSQLite();

                // Replace the umbraco product name extractor with one that supports child variants
                v.Services.AddUnique<IUmbracoProductNameExtractor, CompositeProductNameExtractor>();

                // Register event handlers
                v.WithNotificationEvent<OrderProductAddingNotification>()
                    .RegisterHandler<OrderProductAddingHandler>();

                v.WithNotificationEvent<OrderLineChangingNotification>()
                    .RegisterHandler<OrderLineChangingHandler>();

                v.WithNotificationEvent<OrderLineRemovingNotification>()
                    .RegisterHandler<OrderLineRemovingHandler>();

                v.WithNotificationEvent<OrderPaymentCountryRegionChangingNotification>()
                    .RegisterHandler<OrderPaymentCountryRegionChangingHandler>();

                v.WithNotificationEvent<OrderShippingCountryRegionChangingNotification>()
                    .RegisterHandler<OrderShippingCountryRegionChangingHandler>();

                v.WithNotificationEvent<OrderShippingMethodChangingNotification>()
                    .RegisterHandler<OrderShippingMethodChangingHandler>();

            });

            umbracoBuilder.AddNotificationHandler<UmbracoApplicationStartingNotification, TransformExamineValues>();

            return umbracoBuilder;
        }
    }
}
