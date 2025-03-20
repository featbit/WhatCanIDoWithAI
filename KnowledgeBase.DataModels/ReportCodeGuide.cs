using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FeatGen.Models.ReportGenerator;
using System.Text.Json.Serialization;

namespace FeatGen.Models
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

        [Column("fake_data_base")]
        [JsonPropertyName("FakeDataBase")]
        public string FakeDataBase { get; set; }
    }
}
