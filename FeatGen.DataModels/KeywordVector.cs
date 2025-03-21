using Pgvector;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeatGen.Models
{
    public class KeywordVector
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("document_id")]
        public Guid DocumentId { get; set; }

        [Column("keywords")]
        public string Keywords { get; set; }

        [Column("vector_op_txt_emb_3_lg", TypeName = "vector(3072)")]
        public Vector? OpTextEmb3LgVector { get; set; }

        [Column("vector_op_txt_emb_3_sm", TypeName = "vector(1536)")]
        public Vector? OpTextEmb3SmVector { get; set; }

        [Column("vector_ge_txt_emb_004", TypeName = "vector(768)")]
        public Vector? GeTextEmb004Vector { get; set; }
    }
}
