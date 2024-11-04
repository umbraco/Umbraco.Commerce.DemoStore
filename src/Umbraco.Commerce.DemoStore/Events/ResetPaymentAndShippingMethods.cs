using Umbraco.Commerce.Common.Events;
using Umbraco.Commerce.Core.Events.Notification;

namespace Umbraco.Commerce.DemoStore.Events;

// In this store configuration, if someone goes through the checkout flow
// but then back tracks and modifies the order or previous checkout details
// then we'll keep clearing out the steps ahead so that the order calculation
// doesn't show a potential incorrect calculation should the options previously
// selected no longer be available because of the changes made.

public class OrderProductAddingHandler : NotificationEventHandlerBase<OrderProductAddingNotification>
{
    public override async Task HandleAsync(OrderProductAddingNotification evt)
    {
        if (!evt.Order.IsFinalized)
        {
            await evt.Order.ClearShippingMethodAsync();
            await evt.Order.ClearPaymentMethodAsync();
        }
    }
}

public class OrderLineChangingHandler : NotificationEventHandlerBase<OrderLineChangingNotification>
{
    public override async Task HandleAsync(OrderLineChangingNotification evt)
    {
        if (!evt.Order.IsFinalized)
        {
            await evt.Order.ClearShippingMethodAsync();
            await evt.Order.ClearPaymentMethodAsync();
        }
    }
}

public class OrderLineRemovingHandler : NotificationEventHandlerBase<OrderLineRemovingNotification>
{
    public override async Task HandleAsync(OrderLineRemovingNotification evt)
    {
        if (!evt.Order.IsFinalized)
        {
            await evt.Order.ClearShippingMethodAsync();
            await evt.Order.ClearPaymentMethodAsync();
        }
    }
}

public class OrderPaymentCountryRegionChangingHandler : NotificationEventHandlerBase<OrderPaymentCountryRegionChangingNotification>
{
    public override async Task HandleAsync(OrderPaymentCountryRegionChangingNotification evt)
    {
        if (!evt.Order.IsFinalized)
        {
            await evt.Order.ClearPaymentMethodAsync();
        }
    }
}

public class OrderShippingCountryRegionChangingHandler : NotificationEventHandlerBase<OrderShippingCountryRegionChangingNotification>
{
    public override async Task HandleAsync(OrderShippingCountryRegionChangingNotification evt)
    {
        if (!evt.Order.IsFinalized)
        {
            await evt.Order.ClearShippingMethodAsync();
            await evt.Order.ClearPaymentMethodAsync();
        }
    }
}

public class OrderShippingMethodChangingHandler : NotificationEventHandlerBase<OrderShippingMethodChangingNotification>
{
    public override async Task HandleAsync(OrderShippingMethodChangingNotification evt)
    {
        if (!evt.Order.IsFinalized)
        {
            await evt.Order.ClearPaymentMethodAsync();
        }
    }
}
