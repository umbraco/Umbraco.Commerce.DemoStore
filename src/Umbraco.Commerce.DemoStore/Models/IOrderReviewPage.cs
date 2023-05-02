using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public interface IOrderReviewPage
    {
        OrderReadOnly Order { get; }

        CountryReadOnly PaymentCountry { get; }

        CountryReadOnly ShippingCountry { get; }
    }
}
