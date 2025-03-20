using Dapper;
using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FeatGen.ReportGenerator
{
    public interface IReportCodeGuideRepo
    {
        Task<ReportCodeGuide> GetGuidAsync(string reportId);
        Task UpsertGuideAsync(string reportId, string pages = "", string menuItems = "", string models = "", string fake_data_base = "");
    }

    public class ReportCodeGuideRepo(KnowledgeBaseDbContext dbContext) : IReportCodeGuideRepo
    {
        public async Task UpsertGuideAsync(string reportId, string pages = "", string menuItems = "", string models = "", string fake_data_base = "")
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
                    FakeDataBase = fake_data_base ?? "",
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
                if (!string.IsNullOrWhiteSpace(fake_data_base))
                    rcg.FakeDataBase = fake_data_base;
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
