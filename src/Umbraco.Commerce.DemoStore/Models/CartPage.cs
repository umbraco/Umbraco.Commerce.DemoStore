using Umbraco.Commerce.Common;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models;

public partial class CartPage
{
    public CheckoutPage? CheckoutPage => this.GetHomePage().CheckoutPage;

    public AsyncLazy<OrderReadOnly?> Order => new(this.GetCurrentOrderAsync);
}
