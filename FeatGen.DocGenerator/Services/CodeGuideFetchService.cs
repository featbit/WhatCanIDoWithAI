using FeatGen.Models;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using FeatGen.ReportGenerator.Prompts;
using Newtonsoft.Json;
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
            try
            {
                var rcg = await rcgRepo.GetRCGAsync(reportId);
                var allPages = JsonConvert.DeserializeObject<List<GuidePageItem>>(rcg.Pages);
                //var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(rcg.Pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
                return allPages;
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }

        public async Task<List<GuideMenuItem>> GetGeneratedMenuItemsAsync(string reportId)
        {
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            //var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(rcg.MenuItems, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            var menuItems = JsonConvert.DeserializeObject<List<GuideMenuItem>>(rcg.MenuItems);
            return menuItems;
        }
    }
}
