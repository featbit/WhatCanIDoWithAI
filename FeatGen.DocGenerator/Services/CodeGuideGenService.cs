using FeatGen.Models;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator.Prompts;

namespace FeatGen.ReportGenerator
{
    public interface ICodeGuideGenService
    {
        Task<string> GeneratePagesAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GenerateMenuItemsAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GenerateMenuItemsCodeAysnc(string reportId);
        Task<string> GenerateDataModelsAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GenerateFakeDataBaseAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GenerateApiCodeAsync(string reportId, string pageId, string requirement = "no additional requirement");
        Task<string> GenerateComponentCodeAsync(string reportId, string pageId, string pageComponentName, string apiCode, string cssCode, string requirement = "no additional requirement");
    }

    public class CodeGuideGenService(
        IReportRepo reportRepo,
        IReportCodeGuideRepo rcgRepo,
        IOpenAiChatService openaiChatService,
        IAntropicChatService antropicChatService) : ICodeGuideGenService
    {

        public async Task<string> GeneratePagesAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            string prompt = GuideDataGenPages.PagesV1(spec, requirement);
            //string result = await openaiChatService.CompleteChatAsync(prompt, false);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            string result = await antropicChatService.CompleteChatWithJsonAsync(prompt);
            await rcgRepo.UpsertGuideAsync(
                reportId,
                pages: result,
                menuItems: "",
                models: "",
                fake_data_base: "");
            return result;
        }

        public async Task<string> GenerateMenuItemsAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideDataGenPages.MenuItems(spec, rcg);
            //string result = await openaiChatService.CompleteChatAsync(prompt, true);
            string result = await antropicChatService.CompleteChatAsync(prompt, true);
            await rcgRepo.UpsertGuideAsync(
                reportId,
                pages: "",
                menuItems: result.CleanJsCodeQuote(),
                models: "",
                fake_data_base: "");
            return result;
        }

        public async Task<string> GenerateMenuItemsCodeAysnc(string reportId)
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideCodeGenMenuItems.V1(rcg, spec.Title);
            string result = await antropicChatService.CompleteChatAsync(prompt, true);
            result = result.CleanJsCodeQuote();
            return result;
        }

        public async Task<string> GenerateDataModelsAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideDataGenModels.ModelsV2(spec, rcg);
            //string result = await openaiChatService.CompleteChatAsync(prompt, false);
            string result = await antropicChatService.CompleteChatAsync(prompt, false);
            await rcgRepo.UpsertGuideAsync(
                reportId,
                pages: "",
                menuItems: "",
                models: result,
                fake_data_base: "");
            return result;
        }

        public async Task<string> GenerateFakeDataBaseAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideDataGenModels.FakeData(spec, rcg);
            //string result = await openaiChatService.CompleteChatAsync(prompt, false);
            string result = await antropicChatService.CompleteChatAsync(prompt, false);
            await rcgRepo.UpsertGuideAsync(
                reportId,
                pages: "", 
                menuItems: "",
                models: "",
                fake_data_base: result);
            return result;
        }

        public async Task<string> GenerateApiCodeAsync(string reportId, string pageId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = pageId == "login" ?
                GuideCodeGenPageComponentApi.V1Login(spec, rcg, pageId) :
                GuideCodeGenPageComponentApi.V1(spec, rcg, pageId);
            string result = await antropicChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            return result;
        }


        public async Task<string> GenerateComponentCodeAsync(string reportId, string pageId, string pageComponentName, string apiCode, string cssCode, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = pageId == "login" ?
                GuideCodeGenPageComponent.V1Login(spec, rcg, pageId, apiCode, cssCode) :
                GuideCodeGenPageComponent.V1(spec, rcg, pageId, pageComponentName, apiCode, cssCode);
            string result = await antropicChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            return result;
            
        }
        
    }
}
