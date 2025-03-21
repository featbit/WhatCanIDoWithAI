using FeatGen.Models;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using FeatGen.ReportGenerator.Prompts;
using System.Text.Json;

namespace FeatGen.ReportGenerator
{
    public interface ICodeGuideFetchService
    {
        Task<List<GuidePageItem>> GetGeneratedPagesAsync(string reportId);
        Task<List<GuideMenuItem>> GetGeneratedMenuItemsAsync(string reportId);
    }

    public class CodeGuideFetchService(
        IReportRepo reportRepo,
        IReportCodeGuideRepo rcgRepo,
        IOpenAiChatService openaiChatService,
        IAntropicChatService antropicChatService) : ICodeGuideFetchService
    {

        public async Task<List<GuidePageItem>> GetGeneratedPagesAsync(string reportId)
        {
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(rcg.Pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            return allPages;
        }

        public async Task<List<GuideMenuItem>> GetGeneratedMenuItemsAsync(string reportId)
        {
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(rcg.MenuItems, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            return menuItems;
        }
    }
}
