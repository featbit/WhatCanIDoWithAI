using FeatGen.ReportGenerator.Models.GuidePrompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatGen.CodingAgent
{
    public class Steps
    {
        private static async Task WriteCodeToNextJsProject(string folderPath, string fileName, string code)
        {
            await FileAgent.CreateAndUpsertFolderAndFileAsync(
                folderPath: folderPath,
                fileName: fileName,
                newText: code,
                replaceOldText: true);
        }

        public static async Task Step9_4(string projectName, string generatedFileRootPath, string nextjsFileRootPath, string pageId, string menuItem, string reportId, string sharedMemoryDbCode)
        {
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.4: Generating Interface for APis and DB code ");
            string savedApiCode = "";
            //savedApiCode = FileAgent.ReadFileContent(nextjsFileRootPath + $"/apis/{menuItem}.js");
            var interfaces = await ApiGenCaller.Step9_4_GenerateApiDbInterfaces(reportId, pageId, menuItem, savedApiCode, sharedMemoryDbCode);
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.4: Generated ");
            await WriteCodeToNextJsProject(nextjsFileRootPath + $"/interfaces", $"{menuItem}.js", interfaces);
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.4: File Saved ");
        }

        public static async Task Step9_5(string projectName, string generatedFileRootPath, string nextjsFileRootPath, string pageId, string menuItem, string reportId)
        {
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.5: Generating Dedicated DB file models ");
            string savedApiCode = "";
            //savedApiCode = FileAgent.ReadFileContent(nextjsFileRootPath + $"/apis/{menuItem}.js");
            var savedInterfaceDefinition = FileAgent.ReadFileContent(nextjsFileRootPath + $"/interfaces/{menuItem}.js");
            var models = await ApiGenCaller.Step9_5_GenerateApiDbModels(reportId, pageId, menuItem, savedApiCode, savedInterfaceDefinition);
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.5: Generated ");
            await WriteCodeToNextJsProject(nextjsFileRootPath + $"/db/definitions/", $"{menuItem}.js", models);
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.5: File Saved ");
        }

        public static async Task Step9_6(string projectName, string generatedFileRootPath, string nextjsFileRootPath, string pageId, string menuItem, string reportId)
        {
            // 9.6 Generate dedicated db file code 
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.6 Generate dedicated db file code  ");
            //var savedApiCode = FileAgent.ReadFileContent(nextjsFileRootPath + $"/apis/{menuItem}.js");
            var savedInterfaceDefinition = FileAgent.ReadFileContent(nextjsFileRootPath + $"/interfaces/{menuItem}.js");
            var savedDbModels = FileAgent.ReadFileContent(nextjsFileRootPath + $"/db/definitions/{menuItem}.js");
            var code = await ApiGenCaller.Step9_6_GenerateApiDbCode(reportId, pageId, menuItem, "", savedInterfaceDefinition, savedDbModels);
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.6: Generated ");
            await WriteCodeToNextJsProject(nextjsFileRootPath + $"/db/", $"db-{menuItem.Replace("_", "-").Trim().ToLower()}.js", code);
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.6: File Saved ");
        }

        public static async Task Step9_7(string projectName, string generatedFileRootPath, string nextjsFileRootPath, string pageId, string menuItem, string reportId)
        {                   
            // 9.7 Update API Code 
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.7 Update API Code   ");
            //var savedApiCode = FileAgent.ReadFileContent(nextjsFileRootPath + $"/apis/{menuItem}.js");
            var savedInterfaceDefinition = FileAgent.ReadFileContent(nextjsFileRootPath + $"/interfaces/{menuItem}.js");
            var savedDbCode = FileAgent.ReadFileContent(nextjsFileRootPath + $"/db/db-{menuItem}.js");
            var code = await ApiGenCaller.Step9_7_GenerateApiDbCode(reportId, pageId, menuItem, "", savedInterfaceDefinition, savedDbCode);
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.7: Generated ");
            await WriteCodeToNextJsProject(nextjsFileRootPath + $"/apis/", $"{menuItem.Replace("_", "-").Trim().ToLower()}.js", code);
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.7: File Saved ");
        }

        public static async Task Step9_8(string projectName, string generatedFileRootPath, string nextjsFileRootPath, string pageId, string menuItem, string reportId,
            string cssCode, string themeIconPrompt, string themeChartPrompt)
        {
            // 9.8 Update Page Code 
            Console.WriteLine($"{projectName} - {menuItem} - Step 9.8 Update Page Code");
            var savedApiCode = FileAgent.ReadFileContent(nextjsFileRootPath + $"/apis/{menuItem}.js");
            var savedInterfaceDefinition = FileAgent.ReadFileContent(nextjsFileRootPath + $"/interfaces/{menuItem}.js");
            var savedDbCode = FileAgent.ReadFileContent(nextjsFileRootPath + $"/db/db-{menuItem}.js");
            var savedDbModels = FileAgent.ReadFileContent(nextjsFileRootPath + $"/db/definitions/{menuItem}.js");

            var code = await ApiGenCaller.Step9_8_UpdatePageCode(reportId, pageId, menuItem, savedApiCode, savedDbCode, cssCode, savedDbModels, themeIconPrompt, themeChartPrompt);
            Console.WriteLine($"Generated ");
            await WriteCodeToNextJsProject(nextjsFileRootPath + $"/pages/{menuItem}", "page.js", code);
            Console.WriteLine($"File Saved ");
        }
    }
}
