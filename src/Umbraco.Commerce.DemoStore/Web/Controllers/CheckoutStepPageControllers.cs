using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Commerce.Core.Models;
using Umbraco.Extensions;

namespace Umbraco.Commerce.DemoStore.Web.Controllers;

/// <summary>
/// Renders a checkout step page, or sends the customer back to the cart when there is nothing to check out:
/// no current order, or an order without lines (on the confirmation page, no finalized order).
/// </summary>
public abstract class CheckoutStepPageControllerBase(
    ILogger<UmbracoPageController> logger,
    ICompositeViewEngine compositeViewEngine)
    : UmbracoPageController(logger, compositeViewEngine), IRenderController
{
    public virtual async Task<IActionResult> Index()
    {
        IPublishedContent page = CurrentPage!;
        OrderReadOnly? order = await page.GetCurrentOrderAsync();
        if (order == null || order.OrderLines.Count == 0)
        {
            return Redirect(page.GetCartPage()?.Url() ?? "/");
        }

        return CurrentTemplate(new ContentModel(page));
    }
}

public class CheckoutInformationPageController(ILogger<UmbracoPageController> logger, ICompositeViewEngine compositeViewEngine)
    : CheckoutStepPageControllerBase(logger, compositeViewEngine);

public class CheckoutShippingMethodPageController(ILogger<UmbracoPageController> logger, ICompositeViewEngine compositeViewEngine)
    : CheckoutStepPageControllerBase(logger, compositeViewEngine);

public class CheckoutPaymentMethodPageController(ILogger<UmbracoPageController> logger, ICompositeViewEngine compositeViewEngine)
    : CheckoutStepPageControllerBase(logger, compositeViewEngine);

public class CheckoutReviewPageController(ILogger<UmbracoPageController> logger, ICompositeViewEngine compositeViewEngine)
    : CheckoutStepPageControllerBase(logger, compositeViewEngine);

public class CheckoutPaymentPageController(ILogger<UmbracoPageController> logger, ICompositeViewEngine compositeViewEngine)
    : CheckoutStepPageControllerBase(logger, compositeViewEngine);

public class CheckoutConfirmationPageController(ILogger<UmbracoPageController> logger, ICompositeViewEngine compositeViewEngine)
    : CheckoutStepPageControllerBase(logger, compositeViewEngine);
