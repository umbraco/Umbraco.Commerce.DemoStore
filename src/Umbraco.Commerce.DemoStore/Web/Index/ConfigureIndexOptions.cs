using Examine;
using Examine.Lucene;
using Lucene.Net.Facet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Umbraco.Cms.Core;
using Umbraco.Commerce.Core.Configuration.Models;

namespace Umbraco.Commerce.DemoStore.Web.Index
{
    public sealed class ConfigureIndexOptions : IConfigureNamedOptions<LuceneDirectoryIndexOptions>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly UmbracoCommerceSettings _settings;

        public ConfigureIndexOptions(ILoggerFactory loggerFactory, IOptions<UmbracoCommerceSettings> settings)
        {
            _loggerFactory = loggerFactory;
            _settings = settings.Value;
        }

        public void Configure(string name, LuceneDirectoryIndexOptions options)
        {
            switch (name)
            {
                case Constants.UmbracoIndexes.ExternalIndexName:

                    var priceFields = new List<string>
                    {
                        "price_GBP"
                    };

                    // Create a config
                    var facetsConfig = new FacetsConfig();

                    foreach (var field in priceFields)
                    {
                        options.FieldDefinitions.TryAdd(new FieldDefinition(field, FieldDefinitionTypes.FacetDouble));
                        facetsConfig.SetIndexFieldName(field, $"facet_{field}");
                    }

                    options.FacetsConfig = facetsConfig;
                    //options.UseTaxonomyIndex = true;

                    break;
            }
        }

        public void Configure(LuceneDirectoryIndexOptions options)
            => Configure(string.Empty, options);
    }
}
