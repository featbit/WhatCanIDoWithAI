
using FeatGen.CodingAgent;
using FeatGen.CodingAgent.Models;
using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Text.Json;

Console.OutputEncoding = System.Text.Encoding.UTF8;


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
// 9.2 Generate Page Component Files
// ### For now 9.2 is not the most critical for ruanzhu ###
// 9.3 Generate Page Component Code based on filtered 9.1, 6, filtered 1 and filtered 3 
// 9.4 Refill data - Generate a separate memorydb for the page
// 10. Generate New User Manual based on 1, 4, 9
// 11. Generate Application Form based on 1, 4, 9




var task1 = Task.Run(() => RunGen(
    projectName: "基于光纤传感的公路基础灾害损毁感知原型系统",
    startStepAt: 9.4,
    stopStepAt: 9.4,
    failedMenuItems: null
));

//var task2 = Task.Run(() => RunGen(
//    projectName: "光伏出力秒级临近预报系统",
//    startStepAt: 10,
//    stopStepAt: 11,
//    failedMenuItems: null
//));

try
{
    await Task.WhenAll(task1);
    //await Task.WhenAll(task1, task2);
    Console.WriteLine("All generation tasks completed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error running parallel generation tasks: {ex.Message}");
}

async Task RunGen(string projectName, double startStepAt, double stopStepAt, List<string> failedMenuItems = null)
{
    string codingRootPath = @"C:/Code/featgen/featgen";
    string generatedFileRootPath = @"C:/Code/featgen/generated-files/" + projectName;
    string nextjsFileRootPath = @"C:/Code/featgen/generated-files/" + projectName + "/featgen/src/app";
    string userManualFilePath = @"C:/Code/featgen/generated-files/" + projectName + "/user-manual.md";

    try
    {
        if (startStepAt <= 2 && stopStepAt >= 3)
        {
            //step 2,3
            //Currently work for it in vscode with prompt:
            // 
            // Please update globals.css for the new theme of service "伏出力秒级临近预报系统". Please update colors(primary, secondary, background, text), font size, font famaliy, and so on existing in the current css file
            // For me "伏出力秒级临近预报系统" should be with color of electric and green energy theme
        }

        if (startStepAt <= 0 && stopStepAt >= 0)
        {
            Console.WriteLine($"{projectName} - Step 0: Generating specification...");
            await ApiGenCaller.Step0_SpecificationGen(projectName);
            Console.WriteLine($"Generated");
        }

        string reportId = await ApiFetchCaller.GetReportIdByTitleAsync(projectName);

        if (startStepAt <= 1 && stopStepAt >= 1)
        {
            //step 1
            Console.WriteLine($"{projectName} - Step 1: Generating guide pages...");
            await ApiGenCaller.Step1_GuidePages(reportId);
            Console.WriteLine($"Generated");
        }

        if (startStepAt <= 4 && stopStepAt >= 4)
        {
            //step 4
            Console.WriteLine($"{projectName} - Step 4: Generating guide menu items...");
            await ApiGenCaller.Step4_GuideMenuItems(reportId);
            Console.WriteLine($"Generated");
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
            Console.WriteLine($"{projectName} - Step 5: Generating guide menu items code for...");
            string guideMenuItemsCode = "export const PATH_PREFIX = '/pages';\r\n" + await ApiGenCaller.Step5_GuideMenuItemsCode(reportId);

            FileAgent.RewriteFileContent(generatedFileRootPath + "/menuitems/code.js", guideMenuItemsCode);
            FileAgent.RewriteFileContent(nextjsFileRootPath + "/components/MenuItems.js", guideMenuItemsCode);
            Console.WriteLine($"Generated");
        }

        if (startStepAt <= 6 && stopStepAt >= 6)
        {
            // step 6
            Console.WriteLine($"{projectName} - Step 6: Generating guide data models for...");
            await ApiGenCaller.Step6_GenerateDataModel(reportId);
            Console.WriteLine($"Generated");
        }

        if (startStepAt <= 7 && stopStepAt >= 7)
        {
            // step 7.1
            Console.WriteLine($"{projectName} - Step 7.1: Generating guide fake database...");
            await ApiGenCaller.Step7_GenerateFakeDataBase(reportId);

            Console.WriteLine($"Generated");
            // step 7.2
            Console.WriteLine($"{projectName} - Step 7.2: Extracting fake database...");
            await ApiGenCaller.Step7_ExtractFakeDataBase(reportId);
            Console.WriteLine($"Extracted");
        }


        if (startStepAt <= 8 && stopStepAt >= 8)
        {
            // step 8 special menu item - login
            //await GenSpecialMenuItemLogin(reportId, generatedFileRootPath, pages, cssCode);
        }


        if (startStepAt <= 9.1 && stopStepAt >= 9.1)
        {
            // step 9.1
            await GenAPIsForMainPages(generatedFileRootPath, nextjsFileRootPath, pages, menuItems, cssCode, reportId);
        }

        if (startStepAt <= 9.2 && stopStepAt >= 9.2)
        {
            // step 9.2
            //await GenMenuItems(generatedFileRootPath, nextjsFileRootPath, pages, menuItems, cssCode, reportId);
        }

        if (startStepAt <= 9.3 && stopStepAt >= 9.3)
        {
            // step 9.3
            await GenCodeForMainPages(projectName, generatedFileRootPath, nextjsFileRootPath, pages, menuItems, cssCode, reportId, failedMenuItems);
        }

        if (startStepAt <= 9.4 && stopStepAt >= 9.4)
        {
            // step 9.4 - redefine dedicated memorydb file, define interface between dedicated memorydb and api code
            await GenPageApiDbInterface(projectName, generatedFileRootPath, nextjsFileRootPath, pages, menuItems, reportId, failedMenuItems);
        }


        if (startStepAt <= 10 && stopStepAt >= 10)
        {
            // step 10
            await UpdateUserManual(userManualFilePath, reportId, nextjsFileRootPath, spec, menuItems);
        }

        if (startStepAt <= 11 && stopStepAt >= 11)
        {
            // step 11
            string formString = await ApiGenCaller.GenerateApplicationForm(reportId);
            FileAgent.CreateAndInitFile(generatedFileRootPath + "/application-form.txt", formString);
        }
    }

    catch(AggregateException aggEx)
    {
        foreach (var ex in aggEx.InnerExceptions)
        {
            Console.WriteLine($"Error in RunGen {projectName}: {ex.Message}");
        }
        return;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in RunGen {projectName}: {ex.Message}");
        return;
    }
}

async Task WriteCodeToNextJsProject(string folderPath, string fileName, string code)
{
    await FileAgent.CreateAndUpsertFolderAndFileAsync(
        folderPath: folderPath,
        fileName: fileName,
        newText: code,
        replaceOldText: true);
}

async Task UpdateUserManual(string filePath, string reportId, string nextjsFileRootPath, Specification spec, List<GuideMenuItem> menuItems)
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

    var loginPageComponent = await ApiGenCaller.Step9_3_GenerateGuidePageComponent(reportId, loginPageId, loginPageId, loginPageApiCode, cssCode);
    var loginPageComponentFolderPath = generatedFileRootPath + $"/pages/{loginPageId}";
    FileAgent.CreateFolder(loginPageComponentFolderPath);
    FileAgent.CreateAndInitFile(loginPageComponentFolderPath + "/page.js", loginPageComponent);
}

async Task GenAPIsForMainPages(string generatedFileRootPath, string nextjsFileRootPath, List<GuidePageItem> pages, List<GuideMenuItem> menuItems, string cssCode, string reportId)
{
    for (int i = 0; i < menuItems.Count; i++)
    {
        var menuItem = menuItems[i];

        if (menuItem.sub_menu_items == null || menuItem.sub_menu_items.Count == 0)
        {
            // 9.1 Generate Guide API Endpoints based on filtered 1 and 4
            var apiCode = await ApiGenCaller.Step9_1_GenerateGuideAPIEndpoints(reportId, menuItem.page_id);
            var apiCodePath = generatedFileRootPath + $"/apis/{menuItem.menu_item}.js";
            FileAgent.CreateAndInitFile(apiCodePath, apiCode);

            var savedApiCode = FileAgent.ReadFileContent(apiCodePath);
            await WriteCodeToNextJsProject(nextjsFileRootPath + "/apis", $"{menuItem.menu_item}.js", savedApiCode);
        }
        else
        {
            var subMenuItems = menuItem.sub_menu_items;
            for (int j = 0; j < subMenuItems.Count; j++)
            {
                var subMenuItem = subMenuItems[j];

                // 9.1 Generate Guide API Endpoints based on filtered 1 and 4
                var apiCode = await ApiGenCaller.Step9_1_GenerateGuideAPIEndpoints(reportId, subMenuItem.page_id);
                var apiCodePath = generatedFileRootPath + $"/apis/{subMenuItem.menu_item}.js";
                FileAgent.CreateAndInitFile(apiCodePath, apiCode);

                var savedApiCode = FileAgent.ReadFileContent(apiCodePath);
                await WriteCodeToNextJsProject(nextjsFileRootPath + "/apis", $"{subMenuItem.menu_item}.js", savedApiCode);
            }
        }
    }
}

async Task GenCodeForMainPages(string projectName, string generatedFileRootPath, string nextjsFileRootPath, List<GuidePageItem> pages, List<GuideMenuItem> menuItems, string cssCode, string reportId, List<string> failedMenuItems = null)
{
    for (int i = 0; i < menuItems.Count; i++)
    {
        var menuItem = menuItems[i];

        if (menuItem.sub_menu_items == null || menuItem.sub_menu_items.Count == 0)
        {
            if(failedMenuItems != null && failedMenuItems.Count > 0)
            {
                if (failedMenuItems.All(p => p != menuItem.menu_item))
                    continue;
            }

            // 9.3 Generate Page Component Code based on filtered 9.1, filtered 1 and filtered 3
            Console.WriteLine($"{projectName} - Step 9.3: Generating code for page {menuItem.menu_item} ");
            var savedApiCode = FileAgent.ReadFileContent(generatedFileRootPath + $"/apis/{menuItem.menu_item}.js");
            var pageComponent = await ApiGenCaller.Step9_3_GenerateGuidePageComponent(reportId, menuItem.page_id, menuItem.menu_item, savedApiCode, cssCode);
            var pageComponentFolderPath = generatedFileRootPath + $"/pages/{menuItem.menu_item}";
            FileAgent.CreateFolder(pageComponentFolderPath);
            FileAgent.CreateAndInitFile(pageComponentFolderPath + "/page.js", pageComponent);
            Console.WriteLine($"Generated ");

            var savedPageComponent = FileAgent.ReadFileContent(pageComponentFolderPath + "/page.js");
            await WriteCodeToNextJsProject(nextjsFileRootPath + $"/pages/{menuItem.menu_item}", "page.js", savedPageComponent);
            Console.WriteLine($"File Saved ");
        }
        else
        {
            var subMenuItems = menuItem.sub_menu_items;
            for (int j = 0; j < subMenuItems.Count; j++)
            {
                var subMenuItem = subMenuItems[j];
                if (failedMenuItems != null && failedMenuItems.Count > 0)
                {
                    if (failedMenuItems.All(p => p != subMenuItem.menu_item))
                        continue;
                }

                // 9.3 Generate Page Component Code based on filtered 9.1, filtered 1 and filtered 3
                Console.WriteLine($"{projectName} - Step 9.3: Generating code for page {subMenuItem.menu_item} ");
                var savedApiCode = FileAgent.ReadFileContent(generatedFileRootPath + $"/apis/{subMenuItem.menu_item}.js");
                var pageComponent = await ApiGenCaller.Step9_3_GenerateGuidePageComponent(reportId, subMenuItem.page_id, subMenuItem.menu_item, savedApiCode, cssCode);
                var pageComponentFolderPath = generatedFileRootPath + $"/pages/{subMenuItem.menu_item}";
                FileAgent.CreateFolder(pageComponentFolderPath);
                FileAgent.CreateAndInitFile(pageComponentFolderPath + "/page.js", pageComponent);
                Console.WriteLine($"Generated ");

                var savedPageComponent = FileAgent.ReadFileContent(pageComponentFolderPath + "/page.js");
                await WriteCodeToNextJsProject(nextjsFileRootPath + $"/pages/{subMenuItem.menu_item}", "page.js", savedPageComponent);
                Console.WriteLine($"File Saved ");
            }
        }
    }
}

async Task GenPageApiDbInterface(string projectName, string generatedFileRootPath, string nextjsFileRootPath, List<GuidePageItem> pages, List<GuideMenuItem> menuItems, string reportId, List<string> failedMenuItems = null)
{
    var sharedMemoryDbCode = FileAgent.ReadFileContent(nextjsFileRootPath + $"/db/memoryDB.js");
    for (int i = 0; i < menuItems.Count; i++)
    {
        var menuItem = menuItems[i];

        if (menuItem.sub_menu_items == null || menuItem.sub_menu_items.Count == 0)
        {
            if (failedMenuItems != null && failedMenuItems.Count > 0)
            {
                if (failedMenuItems.All(p => p != menuItem.menu_item))
                    continue;
            }

            // 9.4 
            Console.WriteLine($"{projectName} - {menuItem.menu_item} - Step 9.4: Generating Interface for APis and DB code ");
            var savedApiCode = FileAgent.ReadFileContent(nextjsFileRootPath + $"/apis/{menuItem.menu_item}.js");
            var interfaces = await ApiGenCaller.Step9_4_GenerateApiDbInterfaces(reportId, menuItem.page_id, menuItem.menu_item, savedApiCode, sharedMemoryDbCode);
            Console.WriteLine($"Generated ");
            await WriteCodeToNextJsProject(nextjsFileRootPath + $"/interfaces", $"{menuItem.menu_item}.js", interfaces);
            Console.WriteLine($"File Saved ");
        }
        else
        {
            var subMenuItems = menuItem.sub_menu_items;
            for (int j = 0; j < subMenuItems.Count; j++)
            {
                var subMenuItem = subMenuItems[j];
                if (failedMenuItems != null && failedMenuItems.Count > 0)
                {
                    if (failedMenuItems.All(p => p != subMenuItem.menu_item))
                        continue;
                }

                // 9.4 
                Console.WriteLine($"{projectName} - {subMenuItem.menu_item} - Step 9.4: Generating Interface for APis and DB code ");
                var savedApiCode = FileAgent.ReadFileContent(nextjsFileRootPath + $"/apis/{subMenuItem.menu_item}.js");
                var interfaces = await ApiGenCaller.Step9_4_GenerateApiDbInterfaces(reportId, subMenuItem.page_id, subMenuItem.menu_item, savedApiCode, sharedMemoryDbCode);
                Console.WriteLine($"Generated ");
                await WriteCodeToNextJsProject(nextjsFileRootPath + $"/interfaces", $"{subMenuItem.menu_item}.js", interfaces);
                Console.WriteLine($"File Saved ");
            }
        }
    }
}

async Task GenMenuItems(string generatedFileRootPath, string nextjsFileRootPath, List<GuidePageItem> pages, List<GuideMenuItem> menuItems, string cssCode, string reportId)
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