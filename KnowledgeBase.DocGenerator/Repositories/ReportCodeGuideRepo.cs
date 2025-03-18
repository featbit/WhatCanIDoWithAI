using Dapper;
using KnowledgeBase.Models;
using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.ReportGenerator.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KnowledgeBase.ReportGenerator
{
    public interface IReportCodeGuideRepo
    {
        Task UpsertModelsAsync(string models, string pages, string reportId);
    }

    public class ReportCodeGuideRepo(KnowledgeBaseDbContext dbContext) : IReportCodeGuideRepo
    {
        public async Task UpsertModelsAsync(string models, string pages, string reportId)
        {
            var rcg = await dbContext.ReportCodeGuides.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(reportId));
            if(rcg == null)
            {
                rcg = new ReportCodeGuide
                {
                    Id = Guid.NewGuid(),
                    Models = models,
                    Pages = pages,
                    ReportId = Guid.Parse(reportId)
                };
                dbContext.ReportCodeGuides.Add(rcg);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(models))
                    rcg.Models = models;
                if (!string.IsNullOrWhiteSpace(pages))
                    rcg.Pages = pages;
                dbContext.ReportCodeGuides.Update(rcg);
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
