using Umbraco.Commerce.Common;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public interface IOrderReviewPage
    {
        AsyncLazy<OrderReadOnly> Order { get; }

        AsyncLazy<CountryReadOnly> PaymentCountry { get; }

        AsyncLazy<CountryReadOnly> ShippingCountry { get; }
    }
}
