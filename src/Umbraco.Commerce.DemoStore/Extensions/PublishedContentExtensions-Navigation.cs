using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using Umbraco.Commerce.DemoStore.Models;

namespace Umbraco.Commerce.DemoStore;

public static partial class PublishedContentExtensions
{
    // Navigation
    public static HomePage GetHomePage(this IPublishedContent content) => content.AncestorOrSelf<HomePage>()!;
    public static SearchPage? GetSearchPage(this IPublishedContent content) => content.GetHomePage().Children<SearchPage>()?.FirstOrDefault();
    public static CartPage? GetCartPage(this IPublishedContent content) => content.GetHomePage().Children<CartPage>()?.FirstOrDefault();
    public static IEnumerable<CategoryPage> GetCategoryPages(this IPublishedContent content) => content.GetHomePage().Children().FirstOrDefault(x => x.ContentType.Alias == CategoriesPage.ModelTypeAlias)?.Children<CategoryPage>() ?? [];
    public static CheckoutPage? GetCheckoutPage(this IPublishedContent content) => content.GetHomePage().Children<CheckoutPage>()?.FirstOrDefault();
}
