using System.Collections.Generic;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public partial class CheckoutShippingMethodPage
    {
        public CountryReadOnly ShippingCountry => Order.ShippingInfo.CountryId.HasValue
            ? UmbracoCommerceApi.Instance.GetCountry(Order.ShippingInfo.CountryId.Value)
            : null;

        public IEnumerable<ShippingMethodReadOnly> ShippingMethods => UmbracoCommerceApi.Instance.GetShippingMethods(this.GetStore().Id);
    }
}
