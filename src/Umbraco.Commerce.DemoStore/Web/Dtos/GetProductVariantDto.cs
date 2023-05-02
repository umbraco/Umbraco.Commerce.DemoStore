using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Umbraco.Commerce.DemoStore.Web.Dtos
{
    [DataContract(Name = "getProductVariant", Namespace = "")]
    public class GetProductVariantDto
    {
        [DataMember(Name = "productNodeId")]
        public int ProductNodeId { get; set; }

        [DataMember(Name = "attributes")]
        public IDictionary<string, string> Attributes { get; set; }
    }
}
