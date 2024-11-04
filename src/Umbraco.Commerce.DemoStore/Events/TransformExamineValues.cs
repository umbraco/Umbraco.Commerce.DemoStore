﻿using Examine;
using System.Linq;
using System.Collections.Generic;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using Umbraco.Commerce.DemoStore.Models;
using System.Text;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Navigation;

namespace Umbraco.Commerce.DemoStore.Events;

public class TransformExamineValues(
    IExamineManager examineManager,
    IUmbracoContextFactory umbracoContextFactory,
    IDocumentUrlService documentUrlService,
    ILanguageService languageService)
    : INotificationAsyncHandler<UmbracoApplicationStartingNotification>
{
    public async Task HandleAsync(UmbracoApplicationStartingNotification notification, CancellationToken cancellationToken)
    {
        var defaultCulture = await languageService.GetDefaultIsoCodeAsync();

        // Listen for nodes being reindexed in the external index set
        if (examineManager.TryGetIndex("ExternalIndex", out IIndex? index))
        {
            ((BaseIndexProvider)index).TransformingIndexValues += (sender, e) =>
            {
                var values = e.ValueSet.Values.ToDictionary(x => x.Key, x => (IEnumerable<object>)x.Value);

                // ================================================================
                // Make product categories searchable
                // ================================================================

                // Make sure node is a product page node
                if (e.ValueSet.ItemType.InvariantEquals(ProductPage.ModelTypeAlias)
                    || e.ValueSet.ItemType.InvariantEquals(MultiVariantProductPage.ModelTypeAlias))
                {
                    // Make sure some categories are defined
                    if (e.ValueSet.Values.ContainsKey("categories"))
                    {
                        // Prepare a new collection for category aliases
                        var categoryAliases = new List<string>();

                        // Parse the comma separated list of category UDIs
                        var categoryIds = e.ValueSet.GetValue("categories").ToString()!.Split(',')
                            .Select(x => UdiParser.TryParse<GuidUdi>(x, out GuidUdi? id) ? id : null)
                            .Where(x => x != null)
                            .Cast<GuidUdi>()
                            .ToList();

                        // Fetch the category nodes and extract the category alias, adding it to the aliases collection
                        using (UmbracoContextReference ctx = umbracoContextFactory.EnsureUmbracoContext())
                        {
                            foreach (GuidUdi categoryId in categoryIds)
                            {
                                IPublishedContent? category = ctx.UmbracoContext.Content!.GetById(categoryId.Guid);
                                if (category != null)
                                {
                                    var urlSegment = documentUrlService.GetUrlSegment(category.Key, defaultCulture , false);

                                    categoryAliases.Add(urlSegment!);
                                }
                            }
                        }

                        // If we have some aliases, add these to the lucene index in a searchable way
                        if (categoryAliases.Count > 0)
                        {
                            values.Add("categoryAliases", new[] { string.Join(" ", categoryAliases) });
                        }
                    }
                }

                // ================================================================
                // Do some generally usefull modifications
                // ================================================================

                // Create searchable path
                if (e.ValueSet.Values.ContainsKey("path") && !e.ValueSet.Values.ContainsKey("searchPath"))
                {
                    values.Add("searchPath", new[] { e.ValueSet.GetValue("path").ToString()!.Replace(',', ' ') });
                }

                // Stuff all the fields into a single field for easier searching
                var combinedFields = new StringBuilder();

                foreach (KeyValuePair<string, IReadOnlyList<object>> kvp in e.ValueSet.Values)
                {
                    foreach (var value in kvp.Value)
                    {
                        combinedFields.AppendLine(value.ToString());
                    }
                }

                values.Add("contents", new[] { combinedFields.ToString() });

                // Update the value
                e.SetValues(values);
            };
        }
    }
}
