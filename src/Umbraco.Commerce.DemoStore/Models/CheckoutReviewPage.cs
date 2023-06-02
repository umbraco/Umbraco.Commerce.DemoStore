using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public partial class CheckoutReviewPage : IOrderReviewPage
    {
        public CountryReadOnly PaymentCountry => CommerceApi.Instance.GetCountry(this.Order.PaymentInfo.CountryId.Value);

        public CountryReadOnly ShippingCountry => CommerceApi.Instance.GetCountry(this.Order.ShippingInfo.CountryId.Value);
    }
}
