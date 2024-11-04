namespace Umbraco.Commerce.DemoStore.Models;
using Umbraco.Extensions;

public partial class HomePage
{
    public SearchPage? SearchPage => this.Children<SearchPage>()?.FirstOrDefault();

    public CartPage? CartPage => this.Children<CartPage>()?.FirstOrDefault();

    public IEnumerable<CategoryPage> CategoryPages => this.Children().FirstOrDefault(x => x.ContentType.Alias == CategoriesPage.ModelTypeAlias)?.Children<CategoryPage>() ?? [];

    public CheckoutPage? CheckoutPage => this.Children<CheckoutPage>()?.FirstOrDefault();
}
