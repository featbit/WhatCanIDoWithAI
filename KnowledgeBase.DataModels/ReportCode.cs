using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FeatGen.Models.ReportGenerator;
using System.Text.Json.Serialization;

namespace FeatGen.Models
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

        [Column("code_theme")]
        [JsonPropertyName("Theme")]
        public CodeTheme Theme { get; set; }
    }

    public class CodeTheme
    {
        [JsonPropertyName("DarkMode")]
        public string DarkMode { get; set; }

        [JsonPropertyName("FontFamily")]
        public string FontFamily { get; set; }

        [JsonPropertyName("PrimaryColor")]
        public string PrimaryColor { get; set; }

        [JsonPropertyName("SecondaryColor")]
        public string SecondaryColor { get; set; }

        [JsonPropertyName("BodyBgColor")]
        public string BodyBgColor { get; set; }

        [JsonPropertyName("BodyBgColorDrakMode")]
        public string BodyBgColorDrakMode { get; set; }

        [JsonPropertyName("TextColor")]
        public string TextColor { get; set; }

        [JsonPropertyName("TextColorDarkMode")]
        public string TextColorDarkMode { get; set; }
    }
}
