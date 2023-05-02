using System.Collections.Generic;
using System.Linq;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public partial class CheckoutPage
    {
        public OrderReadOnly Order => this.GetCurrentOrder();

        public IEnumerable<CheckoutStepPage> Steps => Children.OfType<CheckoutStepPage>();
    }
}
