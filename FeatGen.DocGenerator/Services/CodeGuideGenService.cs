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
        Task<string> ExtractImportantMemoryDBCodeAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GenerateApiCodeAsync(string reportId, string pageId, string requirement = "no additional requirement");
        Task<string> GeneratePageComponentFilesAsync(string reportId, string pageId, string apiCode, string requirement = "no additional requirement");
        Task<string> GenerateComponentCodeAsync(string reportId, string pageId, string pageComponentName, string apiCode, string cssCode, string requirement = "no additional requirement");
        Task<string> GeneratePageApiDbInterfacesAsync(string reportId, string pageId, string menuItem, string apiCode, string memoryDBCode, string pageCode, string requirement = "no additional requirement");
        Task<string> GenerateUserManualByPage(string reportId, string pageId, string pageComponent, string requirement = "no additional requirement");
        Task<string> GenerateApplicationForm(string reportId);
    }

    public class CodeGuideGenService(
        IReportRepo reportRepo,
        IReportCodeGuideRepo rcgRepo,
        IOpenAiChatService openaiChatService,
        IAntropicChatService antropicChatService,
        IGeminiChatService geminiChatService) : ICodeGuideGenService
    {

        public async Task<string> GeneratePagesAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            string prompt = GuideDataGenPages.PagesV1(spec, requirement);
            //string result = await openaiChatService.CompleteChatAsync(prompt, false);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            //string result = await antropicChatService.CompleteChatWithJsonAsync(prompt);
            string result = await geminiChatService.CompleteChatAsync(prompt);
            result = result.CleanJsonCodeQuote();
            await rcgRepo.UpsertGuideAsync(
                reportId,
                pages: result,
                menuItems: "",
                models: "",
                fake_data_base: "",
                extract_db_ds: "");
            return result;
        }

        public async Task<string> GenerateMenuItemsAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideDataGenPages.MenuItems(spec, rcg);
            //string result = await openaiChatService.CompleteChatAsync(prompt, true);
            //string result = await antropicChatService.CompleteChatAsync(prompt, true);
            string result = await geminiChatService.CompleteChatAsync(prompt);
            result = result.CleanJsonCodeQuote();
            await rcgRepo.UpsertGuideAsync(
                reportId,
                pages: "",
                menuItems: result.CleanJsCodeQuote(),
                models: "",
                fake_data_base: "",
                extract_db_ds: "");
            return result;
        }

        public async Task<string> GenerateMenuItemsCodeAysnc(string reportId)
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideCodeGenMenuItems.V1(rcg, spec.Title);
            //string result = await antropicChatService.CompleteChatAsync(prompt, true);
            string result = await geminiChatService.CompleteChatAsync(prompt);
            result = result.CleanJsCodeQuote();
            return result;
        }

        public async Task<string> GenerateDataModelsAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideDataGenModels.ModelsV2(spec, rcg);
            //string result = await openaiChatService.CompleteChatAsync(prompt, false);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsonCodeQuote();
            await rcgRepo.UpsertGuideAsync(
                reportId,
                pages: "",
                menuItems: "",
                models: result,
                fake_data_base: "",
                extract_db_ds: "");
            return result;
        }

        public async Task<string> GenerateFakeDataBaseAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            //string prompt = GuideDataGenModels.FakeData(spec, rcg);
            string prompt = GuideDataGenModels.MemoryDB(spec, rcg);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            await rcgRepo.UpsertGuideAsync(
                reportId,
                pages: "", 
                menuItems: "",
                models: "",
                fake_data_base: result,
                extract_db_ds: "");
            return result;
        }

        public async Task<string> ExtractImportantMemoryDBCodeAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideDataGenModels.ExtractImportantMemoryDBCode(rcg);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            await rcgRepo.UpsertGuideAsync(
                reportId,
                pages: "",
                menuItems: "",
                models: "",
                fake_data_base: "",
                extract_db_ds: result);
            return result;
        }

        

        public async Task<string> GenerateApiCodeAsync(string reportId, string pageId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = pageId == "login" ?
                GuideCodeGenPageComponentApi.V1Login(spec, rcg, pageId) :
                GuideCodeGenPageComponentApi.V2WithMemoryDB(spec, rcg, pageId);
            //GuideCodeGenPageComponentApi.V1(spec, rcg, pageId);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            //GeminiResponse result = await geminiChatService.CompleteChatAsync(prompt, false);
            //return result.Message;
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            if (result.ToLower().Contains("../../db/memoryDB"))
                result = result.Replace("../../db/memoryDB", "@/app/db/memoryDB").Replace("../../db/MemoryDB", "@/app/db/memoryDB"); ;
            return result;
        }

        public async Task<string> GeneratePageComponentFilesAsync(string reportId, string pageId, string apiCode, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideCodeGenPageComponentsFiles.V1(spec, rcg, pageId, apiCode);
            string result = await antropicChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsonCodeQuoteV2();
            return result;
        }

        
        public async Task<string> GenerateComponentCodeAsync(string reportId, string pageId, string pageComponentName, string apiCode, string cssCode, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = pageId == "login" ?
                GuideCodeGenPageComponent.V1Login(spec, rcg, pageId, apiCode, cssCode) :
                GuideCodeGenPageComponent.V1(spec, rcg, pageId, pageComponentName, apiCode, cssCode);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            return result;
        }

        public async Task<string> GeneratePageApiDbInterfacesAsync(string reportId, string pageId, string menuItem, string apiCode, string memoryDBCode, string pageCode, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideCodeGenPageDedicatedMemoryDb.GenerateInterfaces(spec, rcg, pageId, menuItem, apiCode, memoryDBCode, pageCode);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsonCodeQuote();
            return result;
        }

        

        public async Task<string> GenerateUserManualByPage(string reportId, string pageId, string pageComponent, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            //string prompt = GuideSpecGenHeading2.V1(spec, rcg, pageId, pageComponent);
            string prompt = GuideSpecGenHeading2.V2UseGeneratedPageFeatureFunctionalityDescription(spec, rcg, pageId, pageComponent);
            string result = await antropicChatService.CompleteChatAsync(prompt, false);
            //string result = await openaiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanMarkdownCodeQuote();
            return result;
        }

        public async Task<string> GenerateApplicationForm(string reportId)
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = GuideApplyFormGen.V1(spec, rcg);
            string result = await openaiChatService.CompleteChatAsync(prompt, false);
            return result;
        }

    }
}
