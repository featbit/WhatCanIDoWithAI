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
        Task<ReportCodeGuide> GetGuidAsync(string reportId);
        Task UpsertGuideAsync(string models, string pages, string menuItems, string reportId);
    }

    public class ReportCodeGuideRepo(KnowledgeBaseDbContext dbContext) : IReportCodeGuideRepo
    {
        public async Task UpsertGuideAsync(string models, string pages, string menuItems, string reportId)
        {
            var rcg = await dbContext.ReportCodeGuides.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(reportId));
            if(rcg == null)
            {
                rcg = new ReportCodeGuide
                {
                    Id = Guid.NewGuid(),
                    Models = models ?? "",
                    Pages = pages ?? "",
                    MenuItems = menuItems ?? "",
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
                if (!string.IsNullOrWhiteSpace(menuItems))
                    rcg.MenuItems = menuItems;
                dbContext.ReportCodeGuides.Update(rcg);
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task<ReportCodeGuide> GetGuidAsync(string reportId)
        {
            var rcg = await dbContext.ReportCodeGuides.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(reportId));
            return rcg;
        }
    }
}
