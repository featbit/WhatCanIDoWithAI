using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using KnowledgeBase.Models.ReportGenerator;
using System.Text.Json.Serialization;

namespace KnowledgeBase.Models
{
    public class ReportCodeGuide
    {
        [Key]
        [Column("id")]
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [Column("report_id")]
        [JsonPropertyName("ReportId")]
        public Guid ReportId { get; set; }

        [Column("models")]
        [JsonPropertyName("Models")]
        public string Models { get; set; }

        [Column("pages")]
        [JsonPropertyName("Pages")]
        public string Pages { get; set; }

        [Column("menu_items")]
        [JsonPropertyName("MenuItems")]
        public string MenuItems { get; set; }
    }
}
