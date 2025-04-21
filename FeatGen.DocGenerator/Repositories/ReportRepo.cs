using Dapper;
using FeatGen.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using FeatGen.Models.ReportGenerator;

namespace FeatGen.ReportGenerator
{
    public interface IReportRepo
    {
        // Updated comment to clarify the upsert functionality
        /// <summary>
        /// Adds a new report or updates an existing one if the ID already exists
        /// </summary>
        Task AddReportAsync(Specification spec, string id = null);
        Task<Specification?> GetSpecificationByReportIdAsync(string reportId);
        Task<Report?> GetReportByIdAsync(string id);
        Task<string> GetReportIdByTitleAsync(string title);
    }

    public class ReportRepo(FeatGenDbContext dbContext) : IReportRepo
    {
        public async Task AddReportAsync(Specification spec, string id = null)
        {
            Report report = new()
            {
                Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid() : new Guid(id),
                Specification = spec,
                CreatedAt = DateTime.UtcNow,
            };

            var connection = dbContext.Database.GetDbConnection();
            
            // Modified SQL to use upsert (INSERT ... ON CONFLICT ... DO UPDATE)
            const string sql = """
                           INSERT INTO reports (id, created_at, specification) 
                           VALUES (@id, @created_at, @specification::jsonb)
                           ON CONFLICT (id)
                           DO UPDATE SET specification = @specification::jsonb 
                           WHERE reports.id = @id;
                           """;

            object reportObj = new
            {
                id = report.Id,
                created_at = report.CreatedAt,
                specification = JsonSerializer.Serialize(report.Specification),
            };
            await connection.ExecuteAsync(sql, reportObj);
        }

        public async Task<Specification?> GetSpecificationByReportIdAsync(string id)
        {
            //dbContext.Reports.Where(p => p.Id == Guid.Parse(id)).Load();

            Report? report = await dbContext.Reports.FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return report?.Specification;
        }

        public async Task<Report?> GetReportByIdAsync(string id)
        {
            dbContext.Reports.Where(p => p.Id == Guid.Parse(id)).Load();

            Report? report = await dbContext.Reports.FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return report;
        }

        public async Task<string> GetReportIdByTitleAsync(string title)
        {
            var report = await dbContext.Reports.FirstOrDefaultAsync(p => p.Specification.Title == title);
            return report.Id.ToString();
        }
    }
}
