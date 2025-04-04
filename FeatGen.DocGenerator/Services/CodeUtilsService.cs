using FeatGen.Models;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using FeatGen.ReportGenerator.Prompts;
using System.Text.Json;

namespace FeatGen.ReportGenerator
{
    public interface ICodeUtilsService
    {
        Task<string> GetReportIdByTitleAsync(string title);
    }

    public class CodeUtilsService(
        IReportRepo reportRepo,
        IReportCodeGuideRepo rcgRepo,
        IOpenAiChatService openaiChatService,
        IAntropicChatService antropicChatService) : ICodeUtilsService
    {
        public async Task<string> GetReportIdByTitleAsync(string title)
        {
            return await reportRepo.GetReportIdByTitleAsync(title);
        }
    }
}
