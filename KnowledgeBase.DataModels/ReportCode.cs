using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using KnowledgeBase.Models.ReportGenerator;
using System.Text.Json.Serialization;

namespace KnowledgeBase.Models
{
    public class ReportCode
    {
        [Key]
        [Column("id")]
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [Column("report_id")]
        [JsonPropertyName("ReportId")]
        public Guid ReportId { get; set; }

        [Column("code")]
        [JsonPropertyName("Code")]
        public CodeForReport Code { get; set; }


    }
}
