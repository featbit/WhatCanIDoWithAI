using FeatGen.OpenAI;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using FeatGen.ReportGenerator.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FeatGen.ReportGenerator.Services
{
    public interface ICodeFixService
    {
        Task<string> DbCodeFixing(string fileCode, string requirementPrompt);
        Task<string> ApiCodeFixing(string fileCode, string requirementPrompt);
        Task<string> PageCodeFixing(string fileCode, string requirementPrompt);
        Task<string> ChooseFiles(string reportId, string menuItem, string dbCode, string apiCode, string pageCode, string humanInput);
    }
    public class CodeFixService(
        IGeminiChatService geminiChatService, 
        IAntropicChatService antropicChatService,
        IReportRepo reportRepo,
        IReportCodeGuideRepo rcgRepo) : ICodeFixService
    {
        public async Task<string> FixingForCodeError(FixingCodeRequest request)
        {
            return "";
        }
        public async Task<string> DbCodeFixing(string fileCode, string requirementPrompt)
        {
            string prompt = PostSteps1CodeFixing.DbCodeFixingPrompt(fileCode, requirementPrompt);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            return result;
        }
        public async Task<string> ApiCodeFixing(string fileCode, string requirementPrompt)
        {
            string prompt = PostSteps1CodeFixing.ApiCodeFixingPrompt(fileCode, requirementPrompt);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            return result;
        }
        public async Task<string> PageCodeFixing(string fileCode, string requirementPrompt)
        {
            string prompt = PostSteps1CodeFixing.PageCodeFixingPrompt(fileCode, requirementPrompt);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
            result = result.CleanJsCodeQuote();
            return result;
        }
        public async Task<string> ChooseFiles(string reportId, string menuItem, string dbCode, string apiCode, string pageCode, string humanInput)
        {
            var spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            var rcg = await rcgRepo.GetRCGAsync(reportId);

            var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(rcg.MenuItems, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            var menuItemObj = menuItems?.FirstOrDefault(p => p.menu_item == menuItem);
            var pageId = menuItemObj?.page_id;

            var pagesString = rcg.Pages;
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var subPages = allPages.Where(p =>
                    mainPage.related_pages.Any(p => p.page_id == pageId && p.direction == "forward") &&
                    menuItems.All(m => m.page_id != p.page_id)).ToList();
            var pages = new List<GuidePageItem>() { mainPage };
            pages.AddRange(subPages);
            string pageDesc = JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            string prompt = PostSteps1CodeFixing.DecideWhichFileToModifiedPrompt(menuItem, pageDesc, dbCode, apiCode, pageCode, humanInput);
            string result = await geminiChatService.CompleteChatAsync(prompt, false, "decide-which-file-to-modify");
            result = result.CleanJsCodeQuote();
            return result;
        }
    }

    public class FixingCodeReturn
    {
        public string FileName { get; set; }
        public string UpdatedCode { get; set; }
    }

    public class FixingCodeRequest
    {
        public string ApiFileName { get; set; }
        public string ApiFileCode { get; set; }
        public string DbFileName { get; set; }
        public string DbFileContent { get; set; }
        public string PageFileName { get; set; }
        public string PageFileContent { get; set; }
        public string CssCode { get; set; }
        public List<FixingCodeRequestFile> OtherFiles { get; set; }
        public string Prompt { get; set; }
    }

    public class FixingCodeRequestFile
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }
        public string FileDescription { get; set; }
    }
}
