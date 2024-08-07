using System.Collections.Generic;
using System.Linq;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public partial class CheckoutPaymentMethodPage
    {
        public CountryReadOnly PaymentCountry => Order.PaymentInfo.CountryId.HasValue
            ? UmbracoCommerceApi.Instance.GetCountry(Order.PaymentInfo.CountryId.Value)
            : null;

        public RegionReadOnly PaymentRegion => Order.PaymentInfo.RegionId.HasValue
            ? UmbracoCommerceApi.Instance.GetRegion(Order.PaymentInfo.RegionId.Value)
            : null;

        public IEnumerable<PaymentMethodReadOnly> PaymentMethods
            => PaymentCountry != null
            ? UmbracoCommerceApi.Instance.GetPaymentMethodsAllowedIn(PaymentCountry.Id, PaymentRegion?.Id)
            : Enumerable.Empty<PaymentMethodReadOnly>();
    }
}
