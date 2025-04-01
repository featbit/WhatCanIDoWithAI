
using FeatGen.CodingAgent;
using FeatGen.CodingAgent.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Text.Json;



// z. check folders
// 0. Generate Basic Specification
// 1. Generate Guide Pages with Specifications
// 2. Generate Theme and Theme Code based on 0 & 1
// 3. Generate Global Css code based on 1 & 2
// 4. Generate Guide Menu Items based on 1
// 5. Generate Menu Items Code based on 4 
// 6. Generate Guide Data Models based on 1 
// 7. Generate Guide Fake Data based on 1 & 4
// 8. Generate login page
// 9. For each page in menu items:
// 9.1 Generate Guide API Endpoints based on filtered 1 and 4
// 9.2 Generate Page Component Code based on filtered 9.1, 6, filtered 1 and filtered 3 
// 10. Generate New User Manual based on 1, 4, 9
// 11. Generate Application Form based on 1, 4, 9



const string projectName = "鸿云物业管理系统 - 完成";
int startStepAt = 9, stopStepAt = 9;

if(startStepAt <= 0 && stopStepAt >= 0)
{
    await ApiGenCaller.Step0_SpecificationGen(projectName);
}

const string reportId = "e25fc8b3-eb37-4e8c-8238-5b9e050ceed1";
const string codingRootPath = @"C:/Code/featgen/featgen";
const string generatedFileRootPath = @"C:/Code/featgen/generated-files/" + projectName;
const string nextjsFileRootPath = @"C:/Code/featgen/generated-files/" + projectName +"/featgen/src/app";
const string userManualFilePath = @"C:/Code/featgen/generated-files/" + projectName + "/user-manual.md";


if (startStepAt <= 1 && stopStepAt >= 1)
{
    //step 1
    await ApiGenCaller.Step1_GuidePages(reportId);
}

if (startStepAt <= 2 && stopStepAt >= 3)
{
    //step 2,3
    //Currently work for it in vscode with prompt:
    // 
    //  Please update globals.css for the new theme of service "鸿云物业管理系统". Please update colors(primary, secondary, background, text), font size, font famaliy, and so on existing in the current css file
}

if (startStepAt <= 4 && stopStepAt >= 4)
{
    //step 4
    await ApiGenCaller.Step4_GuideMenuItems(reportId);
}

Specification spec = await ApiFetchCaller.GetSpecificationAsync(reportId);
List<GuidePageItem> pages = await ApiFetchCaller.GetGuideGeneratedPagesAsync(reportId);
List<GuideMenuItem> menuItems = await ApiFetchCaller.GetGuideGeneratedMenuItemsAsync(reportId);

FileAgent.CreateFolder(generatedFileRootPath + "/apis");
FileAgent.CreateFolder(generatedFileRootPath + "/css");
FileAgent.CreateFolder(generatedFileRootPath + "/pages");


var cssCode = FileAgent.ReadFileContent(nextjsFileRootPath + "/globals.css");

if (startStepAt <= 5 && stopStepAt >= 5)
{
    // step 5
    string guideMenuItemsCode = await ApiGenCaller.Step5_GuideMenuItemsCode(reportId);
    FileAgent.RewriteFileContent(generatedFileRootPath + "/menuitems/code.js", guideMenuItemsCode);
}

if (startStepAt <= 6 && stopStepAt >= 6)
{
    // step 6
    await ApiGenCaller.Step6_GenerateDataModel(reportId);
}

if (startStepAt <= 7 && stopStepAt >= 7)
{
    // step 7
    await ApiGenCaller.Step7_GenerateFakeDataBase(reportId);
    await ApiGenCaller.Step7_ExtractFakeDataBase(reportId);
}


if (startStepAt <= 8 && stopStepAt >= 8)
{
    // step 8 special menu item - login
    await GenSpecialMenuItemLogin(reportId, generatedFileRootPath, pages, cssCode);
}


if (startStepAt <= 9 && stopStepAt >= 9)
{
    // step 9
    await GenMenuItems(generatedFileRootPath, nextjsFileRootPath, pages, menuItems, cssCode);
}

if (startStepAt <= 10 && stopStepAt >= 10)
{
    // step 10
    await UpdateUserManual(userManualFilePath, nextjsFileRootPath, spec);
}

if (startStepAt <= 11 && stopStepAt >= 11)
{
    // step 11
    string formString = await ApiGenCaller.GenerateApplicationForm(reportId);
    FileAgent.CreateAndInitFile(generatedFileRootPath + "/application-form.txt", formString);
}


async Task WriteCodeToNextJsProject(string folderPath, string fileName, string code)
{
    await FileAgent.CreateAndUpsertFolderAndFileAsync(
        folderPath: folderPath,
        fileName: fileName,
        newText: code,
        replaceOldText: true);
}

async Task UpdateUserManual(string filePath, string nextjsFileRootPath, Specification spec)
{
    using (var writer = new StreamWriter(filePath, append: true))
    {
        await writer.WriteLineAsync($"--------------------------------------------------------");
        await writer.WriteLineAsync($"Generated at UTC time: {DateTime.UtcNow}");
        await writer.WriteLineAsync($"--------------------------------------------------------");

        // Add title and header for the user manual
        await writer.WriteLineAsync($"# {spec.Title}");
        await writer.WriteLineAsync();

        // Handle specification definition with potential newlines
        if (!string.IsNullOrEmpty(spec.Definition))
        {
            string[] definitionLines = spec.Definition.Split(new[] { "\r\n", "\n", "\n\r" }, StringSplitOptions.None);
            foreach (var line in definitionLines)
            {
                await writer.WriteLineAsync(line);
            }
        }

        await writer.WriteLineAsync();

        await writer.WriteLineAsync($"## 用户登录");
        await writer.WriteLineAsync();
        await writer.WriteLineAsync();

        for (int i = 0; i < menuItems.Count; i++)
        {
            var menuItem = menuItems[i];

            //if (menuItem.menu_item != "target-recognition-alarm")
            //    continue;

            if (menuItem.sub_menu_items != null && menuItem.sub_menu_items.Count > 0)
            {
                var subMenuItems = menuItem.sub_menu_items;
                for (int j = 0; j < subMenuItems.Count; j++)
                {
                    try
                    {
                        var subMenuItem = subMenuItems[j];
                        string pageComponentFilePath = nextjsFileRootPath + $"/pages/{subMenuItem.menu_item}/page.js";
                        var savedPageComponent = FileAgent.ReadFileContent(pageComponentFilePath);
                        var doc = await ApiGenCaller.GenerateGuideUserManualByPage(reportId, subMenuItem.page_id, savedPageComponent);
                        string[] docLines = doc.Split(new[] { "\r\n", "\n", "\n\r" }, StringSplitOptions.None);
                        foreach (var line in docLines)
                        {
                            await writer.WriteLineAsync(line);
                        }
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine($"Error: PageId: {menuItem.page_id};");
                        Console.WriteLine(exp.Message);
                        return;
                    }
                }
            }
            else
            {
                try
                {
                    string pageComponentFilePath = nextjsFileRootPath + $"/pages/{menuItem.menu_item}/page.js";
                    var savedPageComponent = FileAgent.ReadFileContent(pageComponentFilePath);
                    var doc = await ApiGenCaller.GenerateGuideUserManualByPage(reportId, menuItem.page_id, savedPageComponent);
                    string[] docLines = doc.Split(new[] { "\r\n", "\n", "\n\r" }, StringSplitOptions.None);
                    foreach (var line in docLines)
                    {
                        await writer.WriteLineAsync(line);
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine($"Error: PageId: {menuItem.page_id};");
                    Console.WriteLine(exp.Message);
                    break;
                }
            }
        }
    }
}

static async Task GenSpecialMenuItemLogin(string reportId, string generatedFileRootPath, List<GuidePageItem> pages, string cssCode)
{
    var loginPageId = "login";
    var loginPage = pages.FirstOrDefault(p => p.page_id == loginPageId);
    var loginPageApiCode = await ApiGenCaller.Step9_1_GenerateGuideAPIEndpoints(reportId, loginPageId);
    var loginPageApiCodePath = generatedFileRootPath + $"/apis/{loginPageId}.js";
    FileAgent.CreateAndInitFile(loginPageApiCodePath, loginPageApiCode);
    //var loginPageApiCode = FileAgent.ReadFileContent(loginPageApiCodePath);

    var loginPageComponent = await ApiGenCaller.Step9_2_GenerateGuidePageComponent(reportId, loginPageId, loginPageId, loginPageApiCode, cssCode);
    var loginPageComponentFolderPath = generatedFileRootPath + $"/pages/{loginPageId}";
    FileAgent.CreateFolder(loginPageComponentFolderPath);
    FileAgent.CreateAndInitFile(loginPageComponentFolderPath + "/page.js", loginPageComponent);
}

async Task GenMenuItems(string generatedFileRootPath, string nextjsFileRootPath, List<GuidePageItem> pages, List<GuideMenuItem> menuItems, string cssCode)
{
    for (int i = 0; i < menuItems.Count; i++)
    {
        var menuItem = menuItems[i];

        if (menuItem.sub_menu_items == null || menuItem.sub_menu_items.Count == 0)
        {
            //var page = pages.FirstOrDefault(p => p.page_id == menuItem.page_id);

            //// 9.1 Generate Guide API Endpoints based on filtered 1 and 4
            //var apiCode = await ApiGenCaller.Step9_1_GenerateGuideAPIEndpoints(reportId, menuItem.page_id);
            //var apiCodePath = generatedFileRootPath + $"/apis/{menuItem.menu_item}.js";
            //FileAgent.CreateAndInitFile(apiCodePath, apiCode);

            //var savedApiCode = FileAgent.ReadFileContent(apiCodePath);
            //await WriteCodeToNextJsProject(nextjsFileRootPath + "/apis", $"{menuItem.menu_item}.js", savedApiCode);

            // 9.2 
            var savedApiCode = FileAgent.ReadFileContent(generatedFileRootPath + $"/apis/{menuItem.menu_item}.js");
            var pcFiles = await ApiGenCaller.Step9_2_GenerateGuidePageComponentFiles(reportId, menuItem.page_id, savedApiCode);
            var pcFsCodePath = generatedFileRootPath + $"/apis/{menuItem.menu_item}_files.json";
            FileAgent.CreateAndInitFile(pcFsCodePath, pcFiles);
            var savedPCFsCode = FileAgent.ReadFileContent(pcFsCodePath);
            var pcfo = JsonSerializer.Deserialize<PageComponentFilesObject>(savedPCFsCode);

            //// 9.3 Generate Page Component Code based on filtered 9.1, filtered 1 and filtered 3
            //var pageComponent = await ApiGenCaller.Step9_2_GenerateGuidePageComponent(reportId, menuItem.page_id, menuItem.menu_item, savedApiCode, cssCode);
            //var pageComponentFolderPath = generatedFileRootPath + $"/pages/{menuItem.menu_item}";
            //FileAgent.CreateFolder(pageComponentFolderPath);
            //FileAgent.CreateAndInitFile(pageComponentFolderPath + "/page.js", pageComponent);

            //var savedPageComponent = FileAgent.ReadFileContent(pageComponentFolderPath + "/page.js");
            //await WriteCodeToNextJsProject(nextjsFileRootPath + $"/pages/{menuItem.menu_item}", "page.js", savedPageComponent);
        }
        else
        {
            //var subMenuItems = menuItem.sub_menu_items;
            //for (int j = 0; j < subMenuItems.Count; j++)
            //{
            //    var subMenuItem = subMenuItems[j];
            //    // 9.1 Generate Guide API Endpoints based on filtered 1 and 4
            //    var apiCode = await ApiGenCaller.Step9_1_GenerateGuideAPIEndpoints(reportId, subMenuItem.page_id);
            //    var apiCodePath = generatedFileRootPath + $"/apis/{subMenuItem.menu_item}.js";
            //    FileAgent.CreateAndInitFile(apiCodePath, apiCode);

            //    var savedApiCode = FileAgent.ReadFileContent(apiCodePath);
            //    await WriteCodeToNextJsProject(nextjsFileRootPath + "/apis", $"{subMenuItem.menu_item}.js", savedApiCode);

            //    // 9.2 Generate Page Component Code based on filtered 9.1, filtered 1 and filtered 3
            //    var pageComponent = await ApiGenCaller.Step9_2_GenerateGuidePageComponent(reportId, subMenuItem.page_id, subMenuItem.menu_item, savedApiCode, cssCode);
            //    var pageComponentFolderPath = generatedFileRootPath + $"/pages/{subMenuItem.menu_item}";
            //    FileAgent.CreateFolder(pageComponentFolderPath);
            //    FileAgent.CreateAndInitFile(pageComponentFolderPath + "/page.js", pageComponent);

            //    var savedPageComponent = FileAgent.ReadFileContent(pageComponentFolderPath + "/page.js");
            //    await WriteCodeToNextJsProject(nextjsFileRootPath + $"/pages/{subMenuItem.menu_item}", "page.js", savedPageComponent);
            //}
        }
    }
}

void AnalysePageComponentFilesObject(PageComponentFilesObject pcfo)
{
    foreach(var item in pcfo.main_page_description.behaviors_direction)
    {
        if(item.direction == "forward")
        {
            var component = pcfo.components.FirstOrDefault(p => p.component_id == item.component_id);
        }
    }
    
}