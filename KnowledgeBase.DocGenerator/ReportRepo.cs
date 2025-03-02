using Dapper;
using KnowledgeBase.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using KnowledgeBase.DataModels.ReportGenerator;

namespace KnowledgeBase.ReportGenerator
{
    public interface IReportRepo
    {
        Task AddReportAsync(Specification spec);
        Task<Specification?> GetSpecificationByIdAsync(string id);
    }

    public class ReportRepo(KnowledgeBaseDbContext dbContext) : IReportRepo
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

        public async Task<Specification?> GetSpecificationByIdAsync(string id)
        {
            dbContext.Reports.Where(p => p.Id == Guid.Parse(id)).Load();

            Report? report = await dbContext.Reports.FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return report?.Specification;
        }
    }
}
