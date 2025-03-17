using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Extensions;
using Umbraco.Commerce.Core.Services;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Commerce.DemoStore.Web.Dtos;
using Umbraco.Commerce.Extensions;
using Asp.Versioning;
using Umbraco.Cms.Api.Common.Filters;
using Microsoft.AspNetCore.Http;
using Umbraco.Commerce.Cms.Models;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Web.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Product Variants")]
[JsonOptionsName(Constants.JsonOptionsNames.DeliveryApi)]
[Route("api/v{version:apiVersion}/demo-store/product-variants")]
public class ProductApiController(
    IProductService productService,
    IPublishedContentQuery publishedContentQuery)
    : Controller
{
    [HttpPost]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ProductVariantDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductVariant([FromBody] GetProductVariantDto model)
    {
        // Get the variants for the given node
        var productNode = publishedContentQuery.Content(model.ProductNodeId) as MultiVariantProductPage;
        if (productNode == null)
        {
            return Ok(null);
        }

        // Get the store from the product node
        var store = productNode.GetStore()!;

        // Find the variant with the matching attributes
        var variant = productNode.Variants.FindByAttributes(model.Attributes);

        // If we have a variant, map it's data to our DTO
        if (variant != null)
        {
            // Convert variant into product snapshot
            var snapshot = await productService.GetProductAsync(store.Id, productNode.Key.ToString("D"), variant.Content.Key.ToString("D"), Thread.CurrentThread.CurrentCulture.Name);
            if (snapshot != null)
            {
                var multiVariantContent = variant.Content as ProductMultiVariant;
                Common.Models.Attempt<Price> priceAttempt = await snapshot.TryCalculatePriceAsync();
                return Ok(new ProductVariantDto
                {
                    ProductVariantReference = variant.Content.Key.ToString("D"),
                    Sku = snapshot.Sku,
                    PriceFormatted = priceAttempt.Success ? await priceAttempt.Result.FormattedAsync() : "",
                    ImageUrl = multiVariantContent?.Image?.GetCropUrl(500, 500)!,
                    ThumbnailImageUrl = multiVariantContent?.Image?.GetCropUrl(150, 150)!
                });
            }
        }

        // Couldn't find a variant so return null
        return Ok(null);
    }
}
