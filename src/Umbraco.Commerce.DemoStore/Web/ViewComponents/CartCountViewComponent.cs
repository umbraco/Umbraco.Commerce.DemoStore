using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Web.ViewComponents;

[ViewComponent]
public class CartCountViewComponent(IUmbracoCommerceApi commerceApi) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(IPublishedContent currentPage)
    {
        var store = currentPage.GetStore()!;
        var order = await commerceApi.GetCurrentOrderAsync(store.Id);
        return View("CartCount", (int)(order?.TotalQuantity ?? 0));
    }
}