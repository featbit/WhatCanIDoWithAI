using Pgvector;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KnowledgeBase.BlazorWebApp.Models
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

        /// <summary>
        /// For the first version, we save document content in the databse
        /// We may move it to a file storage in the future
        /// </summary>
        [Column("text")]
        public string Text { get; set; }
    }
}
