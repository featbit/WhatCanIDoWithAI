﻿using System.Text.Json.Serialization;

namespace FeatGen.ReportGenerator.Models
{
    public class SaaSFeature
    {
        [JsonPropertyName("feature_description")]
        public string Feature { get; set; }
        [JsonPropertyName("feature_name")]
        public string FeatureName { get; set; }
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

    public class FeatureFunctionalities
    {
        [JsonPropertyName("feature_description")]
        public string FeatureDescription { get; set; }

        [JsonPropertyName("menu_item")]
        public string MenuItem { get; set; }

        [JsonPropertyName("feature_name")]
        public string FeatureName { get; set; }
        [JsonPropertyName("feature_functionalities")]
        public List<string> Functionalities { get; set; }
    }

    public class Definition
    {
        [JsonPropertyName("service_description")]
        public string ServiceDescription { get; set; }

        [JsonPropertyName("saas_features")]
        public List<string> SaasFeatures { get; set; }
    }

    public class ModuleDetail
    {
        [JsonPropertyName("module_detail_description")]
        public string DetailDescription { get; set; }
        [JsonPropertyName("module_name")]
        public string Name { get; set; }
    }
}
