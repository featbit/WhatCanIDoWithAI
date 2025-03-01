using Dapper;
using KnowledgeBase.Models.Components.SpecGenerator;
using KnowledgeBase.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KnowledgeBase.SpecGenerator
{
    public interface ISpecificationGenRepo
    {
        Task AddReportAsync(Specification spec);
    }

    public class SpecificationGenRepo(KnowledgeBaseDbContext dbContext) : ISpecificationGenRepo
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
    }
}
