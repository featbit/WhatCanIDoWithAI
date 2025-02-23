using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace KnowledgeBase.BlazorWebApp.Models
{
    public class KnowledgeBaseDbContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }
        public DbSet<KeywordsVector> KeywordsStringVectors { get; set; }
        public DbSet<QuestionVector> QuestionVectors { get; set; }

        public KnowledgeBaseDbContext(DbContextOptions<KnowledgeBaseDbContext> options)
            : base(options)
        {
        }

        // Define your DbSet properties here
        // public DbSet<YourEntity> YourEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");
            modelBuilder.Entity<Document>().ToTable("documents");
            modelBuilder.Entity<KeywordsVector>().ToTable("keywords_vectors");
            modelBuilder.Entity<QuestionVector>().ToTable("question_vectors");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=34.23.104.152:5432;Username=postgres;Password=azerty@123;Database=postgres", o => o.UseVector());
        }
    }
}
