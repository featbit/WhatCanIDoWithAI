using Pgvector;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeatGen.Models
{

    public class Document
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("document_type")]
        public string DocumentType { get; set; }

        [Column("online_url")]
        public string OnlineUrl { get; set; }

        [Column("text")]
        public string Text { get; set; }
    }

    public enum DocumentType
    {
        Article
    }
}
