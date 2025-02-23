using Pgvector;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnowledgeBase.BlazorWebApp.Models
{
    public class KeywordsVector
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("document_id")]
        public Guid DocumentId { get; set; }

        [Column("keywords")]
        public string Keywords { get; set; }

        [Column("embedding", TypeName = "vector(3)")]
        public Vector? Embedding { get; set; }
    }
}
