using KnowledgeBase.Models;
using KnowledgeBase.Models.ReportGenerator;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.ReportGenerator
{
    public interface IReportCodeRepo
    {
        Task UpsertFunctaionalityCodeAsync(ReportCodeFunctionality code, string reportId);
        Task UpsertReportCodeMenuItemsAsync(string code, string reportId);
        Task<ReportCode?> GetReportCodeAsync(string id);
    }

    public class ReportCodeRepo(KnowledgeBaseDbContext dbContext) : IReportCodeRepo
    {
        public async Task UpsertFunctaionalityCodeAsync(ReportCodeFunctionality code, string reportId)
        {
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
    }
}
