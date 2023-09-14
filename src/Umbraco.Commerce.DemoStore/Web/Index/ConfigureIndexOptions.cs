using Examine.Lucene;
using Examine;
using Microsoft.Extensions.Options;
using Lucene.Net.Facet;
using System.Collections.Generic;
using Umbraco.Cms.Core;

namespace Umbraco.Commerce.DemoStore.Web.Index
{
    public sealed class ConfigureIndexOptions : IConfigureNamedOptions<LuceneDirectoryIndexOptions>
    {
        public void Configure(string name, LuceneDirectoryIndexOptions options)
        {
            switch (name)
            {
                case Constants.UmbracoIndexes.ExternalIndexName:

                    var priceFields = new List<string>
                    {
                        "price_GBP"
                    };

                    foreach (var field in priceFields)
                    {
                        options.FieldDefinitions.TryAdd(new FieldDefinition(field, FieldDefinitionTypes.Double));
                    }

                    break;
            }
        }

        public void Configure(LuceneDirectoryIndexOptions options)
            => Configure(string.Empty, options);
    }
}
