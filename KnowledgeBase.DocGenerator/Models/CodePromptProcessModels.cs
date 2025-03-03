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
}
