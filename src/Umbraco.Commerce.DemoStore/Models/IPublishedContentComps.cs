using Umbraco.Cms.Core.Models.PublishedContent;

namespace Umbraco.Commerce.DemoStore.Models;

// We create extra interfaces for the combinations of compositions we use in our models
// This allows us to do minimal casting between compositions and content models

public interface IPage : IPageComp, IPublishedContent
{ }

// General
public partial class HomePage : IPage
{ }
public partial class StandardPage : IPage
{ }
public partial class SearchPage : IPage
{ }
public partial class CartPage : IPage
{ }

// Checkout
public partial class CheckoutPage : IPage
{ }
public interface ICheckoutStepPage : ICheckoutStepComp, IPage
{ }
public partial class CheckoutInformationPage : ICheckoutStepPage
{ }
public partial class CheckoutShippingMethodPage : ICheckoutStepPage
{ }
public partial class CheckoutPaymentMethodPage : ICheckoutStepPage
{ }
public partial class CheckoutConfirmationPage : ICheckoutStepPage
{ }
public partial class CheckoutReviewPage : ICheckoutStepPage
{ }

// Product Groups
public interface IProductGroupPage : IProductGroupComp, IPage
{ }
public partial class CategoriesPage : IProductGroupPage
{ }
public partial class CategoryPage : IProductGroupPage
{ }
public partial class CollectionPage : IProductGroupPage
{ }
public partial class ProductsPage : IProductGroupPage
{ }

// Product Pages
public interface IProductPage : IProductComp, IProductContentComp, IPage
{ }
public partial class ProductPage : IProductPage
{ }
public partial class MultiVariantProductPage : IProductPage
{ }

