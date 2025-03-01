using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KnowledgeBase.Models
{
    public class KnowledgeBaseDbContext : DbContext
    {
        // Added field for IConfiguration
        private readonly IConfiguration _configuration;

        // Updated constructor to accept IConfiguration
        public KnowledgeBaseDbContext(DbContextOptions<KnowledgeBaseDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<QuestionVector> QuestionVectors { get; set; }
        public DbSet<KeywordVector> KeywordVectors { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)  // Only configure if not already set
            {
                var connectionString = _configuration.GetConnectionString("PostgreSQL");
                optionsBuilder.UseNpgsql(connectionString, o => o.UseVector());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");
            modelBuilder.Entity<Document>().ToTable("documents");
            modelBuilder.Entity<QuestionVector>().ToTable("question_vectors");
            modelBuilder.Entity<KeywordVector>().ToTable("keyword_vectors");
            modelBuilder.Entity<Report>().ToTable("reports");
        }
    }
}
