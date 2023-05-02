using System.Collections.Generic;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;

namespace Umbraco.Commerce.DemoStore.Models
{
    public partial class CheckoutInformationPage
    {
        public IEnumerable<CountryReadOnly> Countries => UmbracoCommerceApi.Instance.GetCountries(this.GetStore().Id);
    }
}
