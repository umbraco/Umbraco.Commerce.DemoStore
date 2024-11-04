using Umbraco.Commerce.Common;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models;

public partial class CheckoutReviewPage : IOrderReviewPage
{
    public AsyncLazy<CountryReadOnly> PaymentCountry => new(async () => await UmbracoCommerceApi.Instance.GetCountryAsync((await Order)!.PaymentInfo.CountryId!.Value));

    public AsyncLazy<CountryReadOnly> ShippingCountry => new(async () => await UmbracoCommerceApi.Instance.GetCountryAsync((await Order)!.ShippingInfo.CountryId!.Value));
}
