using System;

namespace Umbraco.Commerce.DemoStore.Web.Dtos
{
    public class UpdateOrderShippingMethodDto
    {
        public Guid ShippingMethod { get; set; }
        public string ShippingOptionId { get; set; }

        public Guid? NextStep { get; set; }
    }
}
