using System.Text.Json.Serialization;

namespace FeatGen.Models.ReportGenerator
{
    public class CodeForReport
    {
        [JsonPropertyName("CodeFeatures")]
        public List<ReportCodeFeature> CodeFeatures { get; set; }
        [JsonPropertyName("CodeMenuItems")]
        public string CodeMenuItems { get; set; }
        [JsonPropertyName("CodeLogin")]
        public string CodeLogin { get; set; }
    }

    public class ReportCodeFeature
    {
        [JsonPropertyName("FeatureId")]
        public string FeatureId { get; set; }

        [JsonPropertyName("FeatureCode")]
        public string FeatureCode { get; set; }

        [JsonPropertyName("CodeFunctionalities")]
        public List<ReportCodeFunctionality> CodeFunctionalities { get; set; }
    }

    public class ReportCodeFunctionality
    {
        [JsonPropertyName("FunctionalityId")]
        public string FunctionalityId { get; set; }

        [JsonPropertyName("Code")]
        public string Code { get; set; }
    }

}
