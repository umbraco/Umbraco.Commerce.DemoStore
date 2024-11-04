using System.Runtime.Serialization;

namespace Umbraco.Commerce.DemoStore.Web.Dtos;

public class ProductVariantDto
{
    public string ProductVariantReference { get; set; }
    public string Sku { get; set; }
    public string PriceFormatted { get; set; }
    public string ImageUrl { get; set; }
    public string ThumbnailImageUrl { get; set; }
}
