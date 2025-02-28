using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeBase.SpecGenerator.Models
{
    public class Definition
    {
        [JsonPropertyName("service_description")]
        public string ServiceDescription { get; set; }

        [JsonPropertyName("saas_features")]
        public List<string> SaasFeatures { get; set; }
    }
}
