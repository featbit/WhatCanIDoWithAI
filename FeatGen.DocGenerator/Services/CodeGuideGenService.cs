using FeatGen.Models;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator.Prompts;

namespace FeatGen.ReportGenerator
{
    public interface ICodeGuideGenService
    {
        Task<string> GeneratePagesAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GenerateCssCodeAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GenerateMenuItemsAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GenerateMenuItemsCodeAysnc(string reportId);
        Task<string> GenerateDataModelsAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GenerateFakeDataBaseAsync(string reportId, string requirement = "no additional requirement");
        Task<string> ExtractImportantMemoryDBCodeAsync(string reportId, string requirement = "no additional requirement");
        Task<string> GeneratePageApiDbInterfacesAsync(string reportId, string pageId, string menuItem, string requirement = "no additional requirement");
        Task<string> DefineDedicatedMemoryDbModel(string reportId, string pageId, string menuItem, string interfaceDefinition, string requirement = "no additional requirement");
        Task<string> GenerateDedicatedMemoryDBCode(string reportId, string pageId, string menuItem, string interfaceDefinition, string dedicatedDbModels, string requirement = "no additional requirement");
        Task<string> GenerateApiCodeWithDedicatedDbCodeAsync(string reportId, string pageId, string menuItem, string interfaceDefinition, string dedicatedDbCode, string requirement = "no additional requirement");
        Task<string> GeneratePageCodeAsync(string reportId, string pageId, string menuItem, string apiCode, string cssCode, string dbCode, string dbModels, string themeIconPrompt, string themeChartPrompt, string requirement = "no additional requirement");
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
            string prompt = Step0GuidePages.Prompt(spec, requirement);
            string result = await geminiChatService.CompleteChatAsync(prompt, false, "pages-gen");
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

        public async Task<string> GenerateCssCodeAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            string prompt = Step2n3ThemeCss.GlobalsCssPrompt(spec);
            string result = await geminiChatService.CompleteChatAsync(prompt);
            result = result.CleanCssCodeQuote();
            return result;
        }

        public async Task<string> GenerateMenuItemsAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = Step4n5MenuItems.GuidePrompt(spec, rcg);
            //string result = await openaiChatService.CompleteChatAsync(prompt, true);
            //string result = await antropicChatService.CompleteChatAsync(prompt, true);
            string result = await geminiChatService.CompleteChatAsync(prompt, false, "menu-items-guid-gen");
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
            string prompt = Step4n5MenuItems.CodePrompt(rcg, spec.Title);
            //string result = await antropicChatService.CompleteChatAsync(prompt, true);
            string result = await geminiChatService.CompleteChatAsync(prompt);
            result = result.CleanJsCodeQuote();
            return result;
        }

        public async Task<string> GenerateDataModelsAsync(string reportId, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = Step6t7DbAbstract.ProjectModelsPrompt(spec, rcg);
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
            string prompt = Step6t7DbAbstract.FakeDatabasePrompt(spec, rcg);
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
            string prompt = Step6t7DbAbstract.DbExtractPrompt(rcg);
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

        public async Task<string> GeneratePageApiDbInterfacesAsync(string reportId, string pageId, string menuItem, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = Step9PageDb.ApiDbInterfacesPrompt(spec, rcg, pageId, menuItem, rcg.FakeDataBase);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsonCodeQuote();
            return result;
        }

        public async Task<string> DefineDedicatedMemoryDbModel(string reportId, string pageId, string menuItem, string interfaceDefinition, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = Step9PageDb.DbModelsPrompt(spec, rcg, pageId, menuItem, interfaceDefinition);
            string result = await antropicChatService.CompleteChatAsync(prompt, false);
            //string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            return result;
        }

        public async Task<string> GenerateDedicatedMemoryDBCode(string reportId, string pageId, string menuItem, string interfaceDefinition, string dedicatedDbModels, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = Step9PageDb.DbCodePrompt(spec, rcg, pageId, menuItem, interfaceDefinition, dedicatedDbModels);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            return result;
        }

        public async Task<string> GenerateApiCodeWithDedicatedDbCodeAsync(string reportId, string pageId, string menuItem, string interfaceDefinition, string dedicatedDbCode, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = Step9PageApi.ApiCodePrompt(spec, rcg, pageId, menuItem, interfaceDefinition, dedicatedDbCode);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            return result;
        }

        public async Task<string> GeneratePageCodeAsync(string reportId, string pageId, string menuItem, string apiCode, string cssCode, string dbCode, string dbModels, string themeIconPrompt, string themeChartPrompt, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = Step9Page.Prompt(spec, rcg, pageId, menuItem, apiCode, cssCode, dbCode, dbModels, themeIconPrompt, themeChartPrompt);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            return result;
            //string toastCorrectPrompt = Step9Page.ToastCorrectPrompt(result); 
            //result = await geminiChatService.CompleteChatAsync(toastCorrectPrompt, false);
            //result = result.CleanJsCodeQuote();
            //return result;
        }


        public async Task<string> GenerateUserManualByPage(string reportId, string pageId, string pageComponent, string requirement = "no additional requirement")
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = Step10UserManual.Prompt(spec, rcg, pageId, pageComponent);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            //string result = await openaiChatService.CompleteChatAsync(prompt, false);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanMarkdownCodeQuote();
            return result;
        }

        public async Task<string> GenerateApplicationForm(string reportId)
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);
            string prompt = Step11ApplyForm.Prompt(spec, rcg);
            string result = await openaiChatService.CompleteChatAsync(prompt, false);
            return result;
        }
    }
}
