using KnowledgeBase.OpenAI;
using KnowledgeBase.ReportGenerator.Prompts;

namespace KnowledgeBase.ReportGenerator
{
    public interface ICodeGuideGenService
    {
        Task<string> GenerateMenuItemsAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GeneratePagesAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GenerateDataModelsAsync(string reportId, string requirement = "no additional requirement");
    }

    public class CodeGuideGenService(
        IReportRepo reportRepo,
        IReportCodeGuideRepo rcgRepo,
        IOpenAiChatService openaiChatService,
        IAntropicChatService antropicChatService) : ICodeGuideGenService
    {
        public async Task<string> GenerateMenuItemsAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetGuidAsync(reportId);
            string prompt = SpecPageGenPrompts.MenuItems(spec, rcg);
            //string result = await openaiChatService.CompleteChatAsync(prompt, false);
            string result = await antropicChatService.CompleteChatAsync(prompt, false);
            await rcgRepo.UpsertGuideAsync("", "", result, reportId);
            return result;
        }

        public async Task<string> GeneratePagesAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            string prompt = SpecPageGenPrompts.PagesV1(spec, requirement);
            //string result = await openaiChatService.CompleteChatAsync(prompt, false);
            string result = await antropicChatService.CompleteChatAsync(prompt, false);
            await rcgRepo.UpsertGuideAsync("", result, "", reportId);
            return result;
        }

        public async Task<string> GenerateDataModelsAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            string prompt = SpecModelGenPrompts.Models(spec);
            //string result = await openaiChatService.CompleteChatAsync(prompt, false);
            string result = await antropicChatService.CompleteChatAsync(prompt, false);
            await rcgRepo.UpsertGuideAsync(result, "", "", reportId);
            return result;
        }
    }
}
