using Umbraco.Commerce.Common;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models;

public partial class CheckoutConfirmationPage : IOrderReviewPage
{
    public override AsyncLazy<OrderReadOnly> Order => new(() => UmbracoCommerceApi.Instance.GetCurrentFinalizedOrderAsync(this.GetStore()!.Id));

    public AsyncLazy<CountryReadOnly> PaymentCountry => new(async () => await UmbracoCommerceApi.Instance.GetCountryAsync((await Order).PaymentInfo.CountryId!.Value));

    public AsyncLazy<CountryReadOnly> ShippingCountry => new(async () => await UmbracoCommerceApi.Instance.GetCountryAsync((await Order).ShippingInfo.CountryId!.Value));
}
