using Dapper;
using FeatGen.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using FeatGen.Models.ReportGenerator;

namespace FeatGen.ReportGenerator
{
    public interface IReportRepo
    {
        Task AddReportAsync(Specification spec);
        Task<Specification?> GetSpecificationByReportIdAsync(string reportId);
        Task<Report?> GetReportByIdAsync(string id);
        Task<string> GetReportIdByTitleAsync(string title);
    }

    public class ReportRepo(FeatGenDbContext dbContext) : IReportRepo
    {
        public async Task AddReportAsync(Specification spec)
        {
            Report report = new()
            {
                Id = Guid.NewGuid(),
                Specification = spec,
                CreatedAt = DateTime.UtcNow,
            };

            var connection = dbContext.Database.GetDbConnection();
            const string sql = """
                           insert into reports (id, created_at, specification) 
                           values (@id, @created_at, @specification::jsonb)
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
