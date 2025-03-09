using Dapper;
using KnowledgeBase.Models;
using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.ReportGenerator.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KnowledgeBase.ReportGenerator
{
    public interface IReportCodeRepo
    {
        Task UpsertThemeCodeAsync(
            CodeTheme code, string reportId);
        Task UpsertFunctaionalityCodeAsync(
            string code, string reportId, string featureId, string functionalityId);
        Task UpsertReportCodeMenuItemsAsync(string code, string reportId);
        Task<ReportCode?> GetReportCodeAsync(string id);
        Task<List<Report>> GetReportsAsync();
    }

    public class ReportCodeRepo(KnowledgeBaseDbContext dbContext) : IReportCodeRepo
    {
        public async Task UpsertFunctaionalityCodeAsync(
            string code, string reportId, string featureId, string functionalityId)
        {
            var rc = await dbContext.ReportCodes.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(reportId));

            var feature = rc.Code.CodeFeatures.FirstOrDefault(p => p.FeatureId == featureId);
            if(feature == null)
            {
                feature = new ReportCodeFeature
                {
                    FeatureId = featureId,
                    CodeFunctionalities = new List<ReportCodeFunctionality>()
                };
                rc.Code.CodeFeatures.Add(feature);
            }
            var functionality = feature.CodeFunctionalities.FirstOrDefault(p => p.FunctionalityId == functionalityId);
            if(functionality == null)
            {
                functionality = new ReportCodeFunctionality
                {
                    FunctionalityId = functionalityId
                };
                feature.CodeFunctionalities.Add(functionality);
            }
            functionality.Code = code;


            var connection = dbContext.Database.GetDbConnection();
            const string sql = """
                    UPDATE report_codes
                    SET code = @code::jsonb
                    WHERE id = @id;
                    """;

            object reportCodeObj = new
            {
                id = rc.Id,
                //report_id = rc.ReportId,
                code = JsonSerializer.Serialize(rc.Code),
            };

            await connection.ExecuteAsync(sql, reportCodeObj);

        }

        public async Task<ReportCode?> GetReportCodeAsync(string id)
        {
            return await dbContext.ReportCodes.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(id));
        }

        public async Task UpsertReportCodeMenuItemsAsync(string code, string reportId)
        {
            var rc = await dbContext.ReportCodes.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(reportId));
            if(rc == null)
            {
                rc = new ReportCode
                {
                    Id = Guid.NewGuid(),
                    ReportId = Guid.Parse(reportId),
                    Code = new CodeForReport
                    {
                        CodeMenuItems = code,
                        CodeFeatures = new List<ReportCodeFeature>()
                    }
                };
                dbContext.ReportCodes.Add(rc);
            }
            else
            {
                rc.Code.CodeMenuItems = code;
                dbContext.ReportCodes.Update(rc);
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task UpsertThemeCodeAsync(CodeTheme code, string reportId)
        {
            var rc = await dbContext.ReportCodes.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(reportId));

            if (rc == null)
            {
                rc = new ReportCode
                {
                    Id = Guid.NewGuid(),
                    ReportId = Guid.Parse(reportId),
                    Code = new CodeForReport
                    {
                        CodeMenuItems = "",
                        CodeFeatures = new List<ReportCodeFeature>(),
                    },
                    Theme = code
                };
                dbContext.ReportCodes.Add(rc);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                rc.Theme = code;

                var connection = dbContext.Database.GetDbConnection();
                const string sql = """
                    UPDATE report_codes
                    SET code_theme = @code_theme::jsonb
                    WHERE id = @id;
                    """;

                object reportCodeObj = new
                {
                    id = rc.Id,
                    code_theme = JsonSerializer.Serialize(rc.Theme),
                };

                await connection.ExecuteAsync(sql, reportCodeObj);
            }
        }

        public Task<List<Report>> GetReportsAsync()
        {
            return dbContext.Reports.ToListAsync();
        }
    }
}
