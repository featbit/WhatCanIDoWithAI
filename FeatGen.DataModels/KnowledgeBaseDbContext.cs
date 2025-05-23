using FeatGen.Models.ReportGenerator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FeatGen.Models
{
    public class FeatGenDbContext : DbContext
    {
        // Added field for IConfiguration
        private readonly IConfiguration _configuration;

        // Updated constructor to accept IConfiguration
        public FeatGenDbContext(DbContextOptions<FeatGenDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<QuestionVector> QuestionVectors { get; set; }
        public DbSet<KeywordVector> KeywordVectors { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportCode> ReportCodes { get; set; }
        public DbSet<ReportCodeGuide> ReportCodeGuides { get; set; }

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
            modelBuilder.Entity<ReportCodeGuide>().ToTable("report_codes_guide");
            modelBuilder.Entity<QuestionVector>().ToTable("question_vectors");
            modelBuilder.Entity<KeywordVector>().ToTable("keyword_vectors");

            // https://www.npgsql.org/efcore/mapping/json.html?tabs=data-annotations%2Cjsondocument#tojson-owned-entity-mapping
            modelBuilder.Entity<Report>()
                .OwnsOne(r => r.Specification, builder =>
                {
                    builder.ToJson("specification");
                    builder.OwnsMany(s => s.Features, featureBuilder =>
                    {
                        featureBuilder.OwnsMany(f => f.Modules, moduleBuilder =>
                        {
                            moduleBuilder.ToJson();
                        });
                    });
                });
            modelBuilder.Entity<Report>().ToTable("reports");

            modelBuilder.Entity<ReportCode>()
                .OwnsOne(r => r.Code, builder =>
                {
                    builder.ToJson("code");
                    builder.OwnsMany(s => s.CodeFeatures, featureBuilder =>
                    {
                        featureBuilder.ToJson("CodeFeatures");
                        //featureBuilder.ToJson();
                        featureBuilder.OwnsMany(f => f.CodeFunctionalities, moduleBuilder =>
                        {
                            moduleBuilder.ToJson("CodeFunctionalities");
                        });
                    });
                })
                .OwnsOne(r => r.Theme, builder =>
                {
                    builder.ToJson("code_theme");
                });
            modelBuilder.Entity<ReportCode>().ToTable("report_codes");
        }
    }
}
