using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Commerce.DemoStore.Models;
using Umbraco.Extensions;

namespace Umbraco.Commerce.DemoStore;

public static partial class PublishedContentExtensions
{
    public static IPublishedContent? GetNextStep(this ICheckoutStepPage content)
    {
        return content.GetCheckoutPage()?.Children().SkipWhile(x => !x.Id.Equals(content.Id)).Skip(1).FirstOrDefault();
    }
    
    public static IPublishedContent? GetPreviousStep(this ICheckoutStepPage content)
    {
        return content.GetCheckoutPage()?.Children().TakeWhile(x => !x.Id.Equals(content.Id)).LastOrDefault();
    }
}
