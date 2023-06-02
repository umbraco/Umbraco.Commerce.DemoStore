using System.Collections.Generic;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public partial class CheckoutShippingMethodPage
    {
        public CountryReadOnly ShippingCountry => Order.ShippingInfo.CountryId.HasValue
            ? CommerceApi.Instance.GetCountry(Order.ShippingInfo.CountryId.Value)
            : null;

        public IEnumerable<ShippingMethodReadOnly> ShippingMethods => CommerceApi.Instance.GetShippingMethods(this.GetStore().Id);
    }
}
