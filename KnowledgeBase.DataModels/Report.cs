using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using KnowledgeBase.DataModels.ReportGenerator;

namespace KnowledgeBase.Models
{
    public class Report
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }


        [Column("specification")]
        public Specification Specification { get; set; }
    }
}
