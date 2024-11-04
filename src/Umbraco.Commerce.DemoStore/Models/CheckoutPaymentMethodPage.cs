using System.Collections.Generic;
using System.Linq;
using Umbraco.Commerce.Common;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models;

public partial class CheckoutPaymentMethodPage
{
    public AsyncLazy<CountryReadOnly?> PaymentCountry => new(async () =>
    {
        var countryId = (await Order)?.PaymentInfo.CountryId;
        return countryId.HasValue
            ? await UmbracoCommerceApi.Instance.GetCountryAsync(countryId.Value)
            : null;
    });

    public AsyncLazy<RegionReadOnly?> PaymentRegion => new(async () =>
    {
        var regionId = (await Order)?.PaymentInfo.RegionId;
        return regionId.HasValue
            ? await UmbracoCommerceApi.Instance.GetRegionAsync(regionId.Value)
            : null;
    });

    public AsyncLazy<IEnumerable<PaymentMethodReadOnly>> PaymentMethods => new(async () =>
    {
        var paymentCountry = await PaymentCountry;
        var paymentRegion = await PaymentRegion;
        return paymentCountry != null
            ? await UmbracoCommerceApi.Instance.GetPaymentMethodsAllowedInAsync(paymentCountry.Id, paymentRegion?.Id)
            : [];
    });
}
