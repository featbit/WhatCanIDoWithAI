using FeatGen.OpenAI;
using FeatGen.ReportGenerator.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FeatGen.ReportGenerator.Services
{
    public interface ICodeFixService
    {
        Task<string> FixingSingleFileCodeError(string fileCode, string requirementPrompt);
    }
    public class CodeFixService(IGeminiChatService geminiChatService, IAntropicChatService antropicChatService) : ICodeFixService
    {
        public async Task<string> FixingForCodeError(FixingCodeRequest request)
        {
            return "";
        }
        public async Task<string> FixingSingleFileCodeError(string fileCode, string requirementPrompt)
        {
            string prompt = PostSteps.SingleFileErrorFixingPrompt(fileCode, requirementPrompt);
            //string result = await antropicChatService.CompleteChatAsync(prompt, false);
            string result = await geminiChatService.CompleteChatAsync(prompt, false);
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
