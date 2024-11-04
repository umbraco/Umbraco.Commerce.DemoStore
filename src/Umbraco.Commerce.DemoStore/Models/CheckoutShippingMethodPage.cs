using Umbraco.Commerce.Common;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models;

public partial class CheckoutShippingMethodPage
{
    public AsyncLazy<CountryReadOnly?> ShippingCountry => new(async () =>
    {
        var countryId = (await Order)?.ShippingInfo.CountryId;
        return countryId.HasValue
            ? await UmbracoCommerceApi.Instance.GetCountryAsync(countryId.Value)
            : null;
    });

    public AsyncLazy<RegionReadOnly?> ShippingRegion => new(async () =>
    {
        var regionId = (await Order)?.ShippingInfo.RegionId;
        return regionId.HasValue
            ? await UmbracoCommerceApi.Instance.GetRegionAsync(regionId.Value)
            : null;
    });

    public AsyncLazy<IEnumerable<ShippingMethodReadOnly>> ShippingMethods => new(async () =>
    {
        var shippingCountry = await ShippingCountry;
        var shippingRegion = await ShippingRegion;
        return shippingCountry != null
            ? await UmbracoCommerceApi.Instance.GetShippingMethodsAllowedInAsync(shippingCountry.Id, shippingRegion?.Id)
            : [];
    });
}
