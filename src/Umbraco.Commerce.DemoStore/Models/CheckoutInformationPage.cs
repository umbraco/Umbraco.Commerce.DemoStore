using Umbraco.Commerce.Common;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models;

public partial class CheckoutInformationPage
{
    public AsyncLazy<IEnumerable<CountryReadOnly>> Countries => new(() => UmbracoCommerceApi.Instance.GetCountriesAsync(this.GetStore()!.Id));
}
