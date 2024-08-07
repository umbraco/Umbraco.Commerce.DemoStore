using System.Collections.Generic;
using System.Linq;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public partial class CheckoutShippingMethodPage
    {
        public CountryReadOnly ShippingCountry => Order.ShippingInfo.CountryId.HasValue
            ? UmbracoCommerceApi.Instance.GetCountry(Order.ShippingInfo.CountryId.Value)
            : null;

        public RegionReadOnly ShippingRegion => Order.ShippingInfo.RegionId.HasValue
            ? UmbracoCommerceApi.Instance.GetRegion(Order.ShippingInfo.RegionId.Value)
            : null;

        public IEnumerable<ShippingMethodReadOnly> ShippingMethods
            => ShippingCountry != null
            ? UmbracoCommerceApi.Instance.GetShippingMethodsAllowedIn(ShippingCountry.Id, ShippingRegion?.Id)
            : Enumerable.Empty<ShippingMethodReadOnly>();
    }
}
