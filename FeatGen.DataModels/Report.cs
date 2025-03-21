using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FeatGen.Models.ReportGenerator;
using System.Text.Json.Serialization;

namespace FeatGen.Models
{
    public class Report
    {
        [Key]
        [Column("id")]
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [Column("created_at")]
        [JsonPropertyName("CreatedAt")]
        public DateTime? CreatedAt { get; set; }


        [Column("specification")]
        [JsonPropertyName("Specification")]
        public Specification Specification { get; set; }
    }
}
