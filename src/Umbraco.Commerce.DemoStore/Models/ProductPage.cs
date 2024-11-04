using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Umbraco.Commerce.DemoStore.Models;

public partial class ProductPage
{
    public CollectionPage Collection => this.Parent<CollectionPage>()!;

    public IPublishedContent? PrimaryImage => this.Images?.FirstOrDefault();

    public IEnumerable<ProductVariant> ChildVariants => this.Children<ProductVariant>() ?? [];
}
