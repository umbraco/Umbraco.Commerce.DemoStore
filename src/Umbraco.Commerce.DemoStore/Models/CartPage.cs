using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public partial class CartPage
    {
        public CheckoutPage CheckoutPage => this.GetHomePage().CheckoutPage;

        public OrderReadOnly Order => this.GetCurrentOrder();
    }
}
