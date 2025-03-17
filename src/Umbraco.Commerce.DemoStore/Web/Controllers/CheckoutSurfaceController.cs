using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Commerce.Common.Models;
using Umbraco.Commerce.Common.Validation;
using Umbraco.Commerce.Core;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;
using Umbraco.Commerce.DemoStore.Web.Dtos;
using Umbraco.Commerce.Extensions;

namespace Umbraco.Commerce.DemoStore.Web.Controllers;

public class CheckoutSurfaceController(
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
    public async Task<IActionResult> ApplyDiscountOrGiftCardCode(DiscountOrGiftCardCodeDto model)
    {
        try
        {
            await commerceApi.Uow.ExecuteAsync(async uow =>
            {
                var store = CurrentPage!.GetStore()!;
                var order = await commerceApi.GetCurrentOrderAsync(store.Id)!
                    .AsWritableAsync(uow)
                    .RedeemAsync(model.Code);
                await commerceApi.SaveOrderAsync(order);
                uow.Complete();
            });
        }
        catch (ValidationException ex)
        {
            ModelState.AddModelError("", "Failed to redeem discount code");

            return CurrentUmbracoPage();
        }

        return RedirectToCurrentUmbracoPage();
    }

    public async Task<IActionResult> RemoveDiscountOrGiftCardCode(DiscountOrGiftCardCodeDto model)
    {
        try
        {
            await commerceApi.Uow.ExecuteAsync(async uow =>
            {
                var store = CurrentPage!.GetStore()!;
                var order = await commerceApi.GetCurrentOrderAsync(store.Id)!
                    .AsWritableAsync(uow)
                    .UnredeemAsync(model.Code);
                await commerceApi.SaveOrderAsync(order);
                uow.Complete();
            });
        }
        catch (ValidationException ex)
        {
            ModelState.AddModelError("", "Failed to redeem discount code");

            return CurrentUmbracoPage();
        }

        return RedirectToCurrentUmbracoPage();
    }

    public async Task<IActionResult> UpdateOrderInformation(UpdateOrderInformationDto model)
    {
        try
        {
            await commerceApi.Uow.ExecuteAsync(async uow =>
            {
                var store = CurrentPage!.GetStore()!;
                var order = await commerceApi.GetCurrentOrderAsync(store.Id)!
                    .AsWritableAsync(uow)
                    .SetPropertiesAsync(new Dictionary<string, string>
                    {
                        { Constants.Properties.Customer.EmailPropertyAlias, model.Email },
                        { "marketingOptIn", model.MarketingOptIn ? "1" : "0" },

                        { Constants.Properties.Customer.FirstNamePropertyAlias, model.BillingAddress.FirstName },
                        { Constants.Properties.Customer.LastNamePropertyAlias, model.BillingAddress.LastName },
                        { "billingAddressLine1", model.BillingAddress.Line1 },
                        { "billingAddressLine2", model.BillingAddress.Line2 },
                        { "billingCity", model.BillingAddress.City },
                        { "billingZipCode", model.BillingAddress.ZipCode },
                        { "billingTelephone", model.BillingAddress.Telephone },

                        { "shippingSameAsBilling", model.ShippingSameAsBilling ? "1" : "0" },
                        { "shippingFirstName", model.ShippingSameAsBilling ? model.BillingAddress.FirstName : model.ShippingAddress.FirstName },
                        { "shippingLastName", model.ShippingSameAsBilling ? model.BillingAddress.LastName : model.ShippingAddress.LastName },
                        { "shippingAddressLine1", model.ShippingSameAsBilling ? model.BillingAddress.Line1 : model.ShippingAddress.Line1 },
                        { "shippingAddressLine2", model.ShippingSameAsBilling ? model.BillingAddress.Line2 : model.ShippingAddress.Line2 },
                        { "shippingCity", model.ShippingSameAsBilling ? model.BillingAddress.City : model.ShippingAddress.City },
                        { "shippingZipCode", model.ShippingSameAsBilling ? model.BillingAddress.ZipCode : model.ShippingAddress.ZipCode },
                        { "shippingTelephone", model.ShippingSameAsBilling ? model.BillingAddress.Telephone : model.ShippingAddress.Telephone },

                        { "comments", model.Comments }
                    })
                    .SetPaymentCountryRegionAsync(model.BillingAddress.Country, null)
                    .SetShippingCountryRegionAsync(model.ShippingSameAsBilling ? model.BillingAddress.Country : model.ShippingAddress.Country, null);
                await commerceApi.SaveOrderAsync(order);
                uow.Complete();
            });
        }
        catch (ValidationException ex)
        {
            ModelState.AddModelError("", "Failed to update information");

            return CurrentUmbracoPage();
        }

        if (model.NextStep.HasValue)
        {
            return RedirectToUmbracoPage(model.NextStep.Value);
        }

        return RedirectToCurrentUmbracoPage();
    }

    public async Task<IActionResult> UpdateOrderShippingMethod(UpdateOrderShippingMethodDto model)
    {
        try
        {
            await commerceApi.Uow.ExecuteAsync(async uow =>
            {
                var store = CurrentPage!.GetStore()!;
                var order = await commerceApi.GetCurrentOrderAsync(store.Id)!
                    .AsWritableAsync(uow);

                if (!string.IsNullOrWhiteSpace(model.ShippingOptionId))
                {
                    var shippingMethod = await commerceApi.GetShippingMethodAsync(model.ShippingMethod);
                    Attempt<ShippingRate> shippingRateAttempt = await shippingMethod.TryCalculateRateAsync(model.ShippingOptionId, order);
                    await order.SetShippingMethodAsync(model.ShippingMethod, shippingRateAttempt.Result!.Option);
                }
                else
                {
                    await order.SetShippingMethodAsync(model.ShippingMethod);
                }

                await commerceApi.SaveOrderAsync(order);
                uow.Complete();
            });
        }
        catch (ValidationException ex)
        {
            ModelState.AddModelError("", "Failed to set order shipping method");

            return CurrentUmbracoPage();
        }

        if (model.NextStep.HasValue)
        {
            return RedirectToUmbracoPage(model.NextStep.Value);
        }

        return RedirectToCurrentUmbracoPage();
    }

    public async Task<IActionResult> UpdateOrderPaymentMethod(UpdateOrderPaymentMethodDto model)
    {
        try
        {
            await commerceApi.Uow.ExecuteAsync(async uow =>
            {
                var store = CurrentPage!.GetStore()!;
                var order = await commerceApi.GetCurrentOrderAsync(store.Id)!
                    .AsWritableAsync(uow)
                    .SetPaymentMethodAsync(model.PaymentMethod);
                await commerceApi.SaveOrderAsync(order);
                uow.Complete();
            });
        }
        catch (ValidationException ex)
        {
            ModelState.AddModelError("", "Failed to set order payment method");

            return CurrentUmbracoPage();
        }

        if (model.NextStep.HasValue)
        {
            return RedirectToUmbracoPage(model.NextStep.Value);
        }

        return RedirectToCurrentUmbracoPage();
    }
}
