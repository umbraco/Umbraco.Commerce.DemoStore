using Microsoft.Extensions.Configuration;
using Umbraco.Commerce.Core.Events.Notification;
using Umbraco.Commerce.DemoStore.Events;
using Umbraco.Commerce.DemoStore.Web.Extractors;
using Umbraco.Commerce.Cms.Extractors;
using Umbraco.Commerce.Extensions;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Commerce.Core.Events.Notification.Handlers.Order;

namespace Umbraco.Commerce.DemoStore;

public static class DemoStoreBuilderExtensions
{
    public static IUmbracoBuilder AddDemoStore(this IUmbracoBuilder umbracoBuilder)
    {
        umbracoBuilder.AddUmbracoCommerce(v =>
        {
            // Enable SQLite support
            v.AddSQLite();

            // Enable the Storefront API
            v.AddStorefrontApi();

            // Replace the umbraco product name extractor with one that supports child variants
            v.Services.AddUnique<IUmbracoProductNameExtractor, CompositeProductNameExtractor>();

            // If we are running a load test, we don't want to send emails
            if (v.Config.GetValue<bool>("Umbraco:Commerce:DemoStore:LoadTest"))
            {
                v.WithNotificationEvent<OrderFinalizedNotification>()
                    .RemoveHandler<SendFinalizedOrderEmail>()
                    .RemoveHandler<SendGiftCardEmails>();
            }

        });

        umbracoBuilder.AddNotificationAsyncHandler<UmbracoApplicationStartingNotification, TransformExamineValues>();

        return umbracoBuilder;
    }
}
