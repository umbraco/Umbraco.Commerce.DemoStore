using System;

namespace Umbraco.Commerce.DemoStore.Web.Dtos
{
    public class UpdateOrderPaymentMethodDto
    {
        public Guid PaymentMethod { get; set; }

        public Guid? NextStep { get; set; }
    }
}
