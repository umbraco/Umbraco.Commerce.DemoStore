using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Commerce.Common.Validation;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;
using Umbraco.Commerce.DemoStore.Web.Dtos;
using Umbraco.Commerce.Extensions;

namespace Umbraco.Commerce.DemoStore.Web.Controllers;

public class CartSurfaceController(
    IUmbracoContextAccessor umbracoContextAccessor,
    IUmbracoDatabaseFactory databaseFactory,
    ServiceContext services,
    AppCaches appCaches,
    IProfilingLogger profilingLogger,
    IPublishedUrlProvider publishedUrlProvider,
    IUmbracoCommerceApi commerceApi)
    : SurfaceController(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger,
        publishedUrlProvider)
{
    [HttpPost]
    public async Task<IActionResult> AddToCart(AddToCartDto postModel)
    {
        try
        {
            await commerceApi.Uow.ExecuteAsync(async uow =>
            {
                var store = CurrentPage!.GetStore()!;
                var order = await commerceApi.GetOrCreateCurrentOrderAsync(store.Id)!
                    .AsWritableAsync(uow)
                    .AddProductAsync(postModel.ProductReference, postModel.ProductVariantReference, postModel.Quantity);
                await commerceApi.SaveOrderAsync(order);
                uow.Complete();
            });
        }
        catch (ValidationException ex)
        {
            ModelState.AddModelError("productReference", "Failed to add product to cart");

            return CurrentUmbracoPage();
        }

        TempData["addedProductReference"] = postModel.ProductReference;

        return RedirectToCurrentUmbracoPage();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCart(UpdateCartDto postModel)
    {
        try
        {
            await commerceApi.Uow.ExecuteAsync(async uow =>
            {
                var store = CurrentPage!.GetStore()!;
                var order = await commerceApi.GetOrCreateCurrentOrderAsync(store.Id)!
                    .AsWritableAsync(uow);

                foreach (var orderLine in postModel.OrderLines)
                {
                    await order.WithOrderLine(orderLine.Id)
                        .SetQuantityAsync(orderLine.Quantity);
                }

                await commerceApi.SaveOrderAsync(order);
                uow.Complete();
            });
        }
        catch (ValidationException ex)
        {
            ModelState.AddModelError("productReference", "Failed to update cart");

            return CurrentUmbracoPage();
        }

        TempData["cartUpdated"] = "true";

        return RedirectToCurrentUmbracoPage();
    }

    [HttpGet]
    public async Task<IActionResult> RemoveFromCart(RemoveFromCartDto postModel)
    {
        try
        {
            await commerceApi.Uow.ExecuteAsync(async uow =>
            {
                var store = CurrentPage!.GetStore()!;
                var order = await commerceApi.GetOrCreateCurrentOrderAsync(store.Id)!
                    .AsWritableAsync(uow)
                    .RemoveOrderLineAsync(postModel.OrderLineId);
                await commerceApi.SaveOrderAsync(order);
                uow.Complete();
            });
        }
        catch (ValidationException ex)
        {
            ModelState.AddModelError("productReference", "Failed to remove cart item");

            return CurrentUmbracoPage();
        }

        return RedirectToCurrentUmbracoPage();
    }
}
