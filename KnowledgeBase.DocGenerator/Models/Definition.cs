using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeBase.SpecGenerator.Models
{
    public class SaaSFeature
    {
        [JsonPropertyName("feature")]
        public string Feature { get; set; }
        [JsonPropertyName("sub_features")]
        public List<string> SubFeatures { get; set; }
        [JsonPropertyName("menu_item")]
        public string MenuItem { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("service_description")]
        public string ServiceDescription { get; set; }

        [JsonPropertyName("saas_features")]
        public List<SaaSFeature> SaasFeatures { get; set; }
    }
}
