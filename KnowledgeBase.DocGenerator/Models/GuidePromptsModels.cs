using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeBase.ReportGenerator.Models.GuidePrompts
{
    public class GuidePageItemMappingFeature
    {
        [JsonPropertyName("feature_name")]
        public string feature_name { get; set; }

        [JsonPropertyName("feature_desc")]
        public string feature_desc { get; set; }

        [JsonPropertyName("feature_id")]
        public string feature_id { get; set; }

        [JsonPropertyName("functionalities")]
        public List<string> functionalities { get; set; }
    }

    public class GuidPageItemRelatedPage
    {
        [JsonPropertyName("page_id")]
        public string page_id { get; set; }

        [JsonPropertyName("direction")]
        public string direction { get; set; }
    }

    public class GuidePageItem
    {
        [JsonPropertyName("page_id")]
        public string page_id { get; set; }

        [JsonPropertyName("page_name")]
        public string page_name { get; set; }

        [JsonPropertyName("page_description")]
        public string page_description { get; set; }

        [JsonPropertyName("mapping_features")]
        public List<GuidePageItemMappingFeature> mapping_features { get; set; }

        [JsonPropertyName("related_pages")]
        public List<GuidPageItemRelatedPage> related_pages { get; set; }

        [JsonPropertyName("page_design")]
        public string page_design { get; set; }
    }

    public class GuideMenuItem
    {
        [JsonPropertyName("reason")]
        public string reason { get; set; }

        [JsonPropertyName("menu_item")]
        public string menu_item { get; set; }

        [JsonPropertyName("menu_name")]
        public string menu_name { get; set; }

        [JsonPropertyName("page_id")]
        public string page_id { get; set; }

        [JsonPropertyName("reason_for_sub_menu")]
        public string reason_for_sub_menu { get; set; }

        [JsonPropertyName("sub_menu_items")]
        public List<GuideSubMenuItem> sub_menu_items { get; set; }
    }

    public class GuideSubMenuItem
    {
        [JsonPropertyName("reason")]
        public string reason { get; set; }

        [JsonPropertyName("menu_item")]
        public string menu_item { get; set; }

        [JsonPropertyName("menu_name")]
        public string menu_name { get; set; }

        [JsonPropertyName("page_id")]
        public string page_id { get; set; }

        [JsonPropertyName("reason_for_sub_menu")]
        public string reason_for_sub_menu { get; set; }
    }
}
