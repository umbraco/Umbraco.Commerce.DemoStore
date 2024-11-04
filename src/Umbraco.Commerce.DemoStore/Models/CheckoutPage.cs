using Umbraco.Commerce.Common;
using Umbraco.Commerce.Core.Models;
using Umbraco.Extensions;

namespace Umbraco.Commerce.DemoStore.Models;

public partial class CheckoutPage
{
    public AsyncLazy<OrderReadOnly?> Order => new(this.GetCurrentOrderAsync);

    public IEnumerable<CheckoutStepPage> Steps => this.Children<CheckoutStepPage>() ?? [];
    
}