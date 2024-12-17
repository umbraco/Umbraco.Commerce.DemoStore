using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
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

            v.AddStorefrontApi();

            // Replace the umbraco product name extractor with one that supports child variants
            v.Services.AddUnique<IUmbracoProductNameExtractor, CompositeProductNameExtractor>();

            // Register event handlers
            // v.WithNotificationEvent<OrderProductAddingNotification>()
            //     .RegisterHandler<OrderProductAddingHandler>();
            //
            // v.WithNotificationEvent<OrderLineChangingNotification>()
            //     .RegisterHandler<OrderLineChangingHandler>();
            //
            // v.WithNotificationEvent<OrderLineRemovingNotification>()
            //     .RegisterHandler<OrderLineRemovingHandler>();
            //
            // v.WithNotificationEvent<OrderPaymentCountryRegionChangingNotification>()
            //     .RegisterHandler<OrderPaymentCountryRegionChangingHandler>();
            //
            // v.WithNotificationEvent<OrderShippingCountryRegionChangingNotification>()
            //     .RegisterHandler<OrderShippingCountryRegionChangingHandler>();
            //
            // v.WithNotificationEvent<OrderShippingMethodChangingNotification>()
            //     .RegisterHandler<OrderShippingMethodChangingHandler>();

            if (v.Config.GetValue<bool>("Umbraco:Commerce:DemoStore:LoadTest"))
            {
                // If we are running a load test, we don't want to send emails
                v.WithNotificationEvent<OrderFinalizedNotification>()
                    .RemoveHandler<SendFinalizedOrderEmail>()
                    .RemoveHandler<SendGiftCardEmails>();
            }
            
            if (!v.Config.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING").IsNullOrWhiteSpace())
            {
                v.Services.AddOpenTelemetry()
                    .UseAzureMonitor()
                    .WithMetrics(opts => opts
                        .AddRuntimeInstrumentation()
                    );
            }

        });

        umbracoBuilder.AddNotificationAsyncHandler<UmbracoApplicationStartingNotification, TransformExamineValues>();

        return umbracoBuilder;
    }
}
