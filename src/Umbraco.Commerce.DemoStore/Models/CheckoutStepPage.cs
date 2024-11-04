using Umbraco.Commerce.Common;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models;

public partial class CheckoutStepPage
{
    public CheckoutPage CheckoutPage => this.GetCheckoutPage();

    public virtual AsyncLazy<OrderReadOnly?> Order => new(this.GetCurrentOrderAsync);

    public CheckoutStepPage? PreviousStep => CheckoutPage.Steps.TakeWhile(x => !x.Id.Equals(this.Id)).LastOrDefault();

    public CheckoutStepPage? NextStep => CheckoutPage.Steps.SkipWhile(x => !x.Id.Equals(this.Id)).Skip(1).FirstOrDefault();

    public AsyncLazy<PaymentMethodReadOnly?> PaymentMethod => new(async () =>
    {
        var paymentMethodId = (await Order)?.PaymentInfo.PaymentMethodId;
        return paymentMethodId.HasValue
            ? await UmbracoCommerceApi.Instance.GetPaymentMethodAsync(paymentMethodId.Value)
            : null;
    });

    public AsyncLazy<ShippingMethodReadOnly?> ShippingMethod => new(async () =>
    {
        var shippingMethodId = (await Order)?.ShippingInfo.ShippingMethodId;
        return shippingMethodId.HasValue
            ? await UmbracoCommerceApi.Instance.GetShippingMethodAsync(shippingMethodId.Value)
            : null;
    });
}
