namespace Umbraco.Commerce.DemoStore.Web.Dtos;

public class GetProductVariantDto
{
    public int ProductNodeId { get; set; }
    public IDictionary<string, string> Attributes { get; set; }
}
