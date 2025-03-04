using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KnowledgeBase.Models.ReportGenerator
{
    public class Specification
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; }
        [JsonPropertyName("Definition")]
        public string Definition { get; set; }

        [JsonPropertyName("Features")]
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {
        [JsonPropertyName("FeatureId")]
        public string FeatureId { get; set; }
        [JsonPropertyName("Description")]
        public string Description { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Modules")]
        public List<Functionality> Modules { get; set; }
        [JsonPropertyName("MenuItem")]
        public string MenuItem { get; set; }
    }

    public class Functionality
    {
        [JsonPropertyName("ShortDescription")]
        public string ShortDescription { get; set; }
        [JsonPropertyName("DetailDescription")]
        public string DetailDescription { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Id")]
        public string Id { get; set; }
    }
}
