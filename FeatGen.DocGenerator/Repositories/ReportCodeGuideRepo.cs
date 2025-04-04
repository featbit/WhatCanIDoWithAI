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
        Task<ReportCodeGuide> GetRCGAsync(string reportId);
        Task UpsertGuideAsync(
            string reportId, string pages = "", string menuItems = "", string models = "", string fake_data_base = "", string extract_db_ds = "");
    }

    public class ReportCodeGuideRepo(FeatGenDbContext dbContext) : IReportCodeGuideRepo
    {
        public async Task UpsertGuideAsync(
            string reportId, string pages = "", string menuItems = "", string models = "", string fake_data_base = "", string extract_db_ds = "")
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
                    ExtractDBDataStructure = extract_db_ds ?? "",
                    ReportId = Guid.Parse(reportId)
                };
                dbContext.ReportCodeGuides.Add(rcg);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(models))
                    rcg.Models = models;
                if (!string.IsNullOrWhiteSpace(pages))
                {
                    if(pages.Contains("\"page_id\": \"login-page\","))
                    {
                        pages = pages.Replace("\"page_id\": \"login-page\",", "\"page_id\": \"login\",");
                    }
                    rcg.Pages = pages;
                }
                if (!string.IsNullOrWhiteSpace(menuItems))
                    rcg.MenuItems = menuItems;
                if (!string.IsNullOrWhiteSpace(fake_data_base))
                    rcg.FakeDataBase = fake_data_base;
                if (!string.IsNullOrWhiteSpace(extract_db_ds))
                    rcg.ExtractDBDataStructure = extract_db_ds;
                dbContext.ReportCodeGuides.Update(rcg);
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task<ReportCodeGuide> GetRCGAsync(string reportId)
        {
            var rcg = await dbContext.ReportCodeGuides.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(reportId));
            return rcg;
        }
    }
}
