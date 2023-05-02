using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public partial class CheckoutConfirmationPage : IOrderReviewPage
    {
        public override OrderReadOnly Order => UmbracoCommerceApi.Instance.GetCurrentFinalizedOrder(this.GetStore().Id);

        public CountryReadOnly PaymentCountry => UmbracoCommerceApi.Instance.GetCountry(this.Order.PaymentInfo.CountryId.Value);

        public CountryReadOnly ShippingCountry => UmbracoCommerceApi.Instance.GetCountry(this.Order.ShippingInfo.CountryId.Value);
    }
}
