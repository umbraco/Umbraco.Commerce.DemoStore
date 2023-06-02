using System.Collections.Generic;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public partial class CheckoutPaymentMethodPage
    {
        public CountryReadOnly PaymentCountry => Order.PaymentInfo.CountryId.HasValue
            ? CommerceApi.Instance.GetCountry(Order.PaymentInfo.CountryId.Value)
            : null;

        public IEnumerable<PaymentMethodReadOnly> PaymentMethods => CommerceApi.Instance.GetPaymentMethods(this.GetStore().Id);
    }
}
