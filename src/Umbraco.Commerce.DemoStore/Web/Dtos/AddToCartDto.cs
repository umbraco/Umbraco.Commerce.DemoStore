namespace Umbraco.Commerce.DemoStore.Web.Dtos;

public class AddToCartDto
{
    public string ProductReference { get; set; }
    public string ProductVariantReference { get; set; }
    public decimal Quantity { get; set; } = 1;
}