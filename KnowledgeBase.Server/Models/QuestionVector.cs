using Pgvector;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnowledgeBase.Server.Models
{
    public class QuestionVector
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("document_id")]
        public Guid DocumentId { get; set; }

        [Column("question_text")]
        public string QuestionText { get; set; }

        //[Column("embedding", TypeName = "vector(3)")]
        //public Vector? Embedding { get; set; }
    }
}
