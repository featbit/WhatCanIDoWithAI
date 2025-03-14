using System.Text.Json.Serialization;

namespace KnowledgeBase.ReportGenerator.Models
{
    class CodePromptProcessModels
    {
    }

    public class Functionality
    {
        [JsonPropertyName("functionality_code")]
        public string Code { get; set; }
    }

    public class MenuItemFeature
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string MenuItem { get; set; }

        public List<SubMenuItem> SubMenuItems { get; set; }
    }

    public class SubMenuItem
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string MenuItem { get; set; }
    }
}
