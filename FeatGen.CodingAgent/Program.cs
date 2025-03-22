
using FeatGen.CodingAgent;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Reflection.Emit;


const string projectName = "极新问答服务智能体系统";
const string reportId = "27930335-8764-4921-af24-7a5af46ac9b1";
const string codingRootPath = @"C:/Code/featgen/featgen";
const string generatedFileRootPath = @"C:/Code/featgen/generated-files/" + projectName;
const string nextjsFileRootPath = @"C:/Code/featgen/featgen/src/app";


// z. check folders
// 0. Generate Basic Specification
// 1. Generate Guide Pages with Specifications
// 2. Generate Theme and Theme Code based on 0 & 1
// 3. Generate Global Css code based on 1 & 2
// 4. Generate Guide Menu Items based on 1
// 5. Generate Menu Items Code based on 4 
// 6. Generate Guide Data Models based on 1 
// 7. Generate Guide Fake Data based on 1 & 4
// 8. Generate Folder and Files for each page in menu items based on 4 & 1 (menu path and page id value are not equal, so we need to do something to match them)
// 9. For each page in menu items:
// 9.1 Generate Guide API Endpoints based on filtered 1 and 4
// 9.2 Generate Page Component Code based on filtered 9.1, filtered 1 and filtered 3 


Specification spec = await ApiFetchCaller.GetSpecificationAsync(reportId);
List<GuidePageItem> pages = await ApiFetchCaller.GetGuideGeneratedPagesAsync(reportId);
List<GuideMenuItem> menuItems = await ApiFetchCaller.GetGuideGeneratedMenuItemsAsync(reportId);

FileAgent.CreateFolder(generatedFileRootPath + "/apis");
FileAgent.CreateFolder(generatedFileRootPath + "/css");
FileAgent.CreateFolder(generatedFileRootPath + "/pages");



var cssCode = FileAgent.ReadFileContent(generatedFileRootPath + "/css/global.css");

// special menu item - login
//var loginPageId = "login";
//var loginPage = pages.FirstOrDefault(p => p.page_id == loginPageId);
//var loginPageApiCode = await ApiGenCaller.Step9_1_GenerateGuideAPIEndpoints(reportId, loginPageId);
//var loginPageApiCodePath = generatedFileRootPath + $"/apis/{loginPageId}.js";
//FileAgent.CreateAndInitFile(loginPageApiCodePath, loginPageApiCode);

//var loginPageComponent = await ApiGenCaller.Step9_2_GenerateGuidePageComponent(reportId, loginPageId, loginPageId, loginPageApiCode, cssCode);
//var loginPageComponentFolderPath = generatedFileRootPath + $"/pages/{loginPageId}";
//FileAgent.CreateFolder(loginPageComponentFolderPath);
//FileAgent.CreateAndInitFile(loginPageComponentFolderPath + "/page.js", loginPageComponent);

// menu items
for (int i = 3; i < menuItems.Count; i++)
{
    //if (menuItems[i].menu_item != "login")
    //    continue;


    var menuItem = menuItems[i];

    if (menuItem.sub_menu_items == null || menuItem.sub_menu_items.Count == 0)
    {
        var page = pages.FirstOrDefault(p => p.page_id == menuItem.page_id);

        // 9.1 Generate Guide API Endpoints based on filtered 1 and 4
        //var apiCode = await ApiGenCaller.Step9_1_GenerateGuideAPIEndpoints(reportId, menuItem.page_id);
        var apiCodePath = generatedFileRootPath + $"/apis/{menuItem.menu_item}.js";
        //FileAgent.CreateAndInitFile(apiCodePath, apiCode);

        var savedApiCode = FileAgent.ReadFileContent(apiCodePath);
        await WriteCodeToNextJsProject(nextjsFileRootPath + "/apis", $"{menuItem.menu_item}.js", savedApiCode);

        // 9.2 Generate Page Component Code based on filtered 9.1, filtered 1 and filtered 3
        //var pageComponent = await ApiGenCaller.Step9_2_GenerateGuidePageComponent(reportId, menuItem.page_id, menuItem.menu_item, savedApiCode, cssCode);
        var pageComponentFolderPath = generatedFileRootPath + $"/pages/{menuItem.menu_item}";
        //FileAgent.CreateFolder(pageComponentFolderPath);
        //FileAgent.CreateAndInitFile(pageComponentFolderPath + "/page.js", pageComponent);

        var savedPageComponent = FileAgent.ReadFileContent(pageComponentFolderPath + "/page.js");
        await WriteCodeToNextJsProject(nextjsFileRootPath + $"/pages/{menuItem.menu_item}", "page.js", savedPageComponent);
    }
    else
    {
        var subMenuItems = menuItem.sub_menu_items;
        for (int j = 0; j < subMenuItems.Count; j++)
        {
            var subMenuItem = subMenuItems[j];
            // 9.1 Generate Guide API Endpoints based on filtered 1 and 4
            //var apiCode = await ApiGenCaller.Step9_1_GenerateGuideAPIEndpoints(reportId, subMenuItem.page_id);
            var apiCodePath = generatedFileRootPath + $"/apis/{subMenuItem.menu_item}.js";
            //FileAgent.CreateAndInitFile(apiCodePath, apiCode);

            var savedApiCode = FileAgent.ReadFileContent(apiCodePath);
            await WriteCodeToNextJsProject(nextjsFileRootPath + "/apis", $"{subMenuItem.menu_item}.js", savedApiCode);

            // 9.2 Generate Page Component Code based on filtered 9.1, filtered 1 and filtered 3
            //var pageComponent = await ApiGenCaller.Step9_2_GenerateGuidePageComponent(reportId, subMenuItem.page_id, subMenuItem.menu_item, savedApiCode, cssCode);
            var pageComponentFolderPath = generatedFileRootPath + $"/pages/{subMenuItem.menu_item}";
            //FileAgent.CreateFolder(pageComponentFolderPath);
            //FileAgent.CreateAndInitFile(pageComponentFolderPath + "/page.js", pageComponent);

            var savedPageComponent = FileAgent.ReadFileContent(pageComponentFolderPath + "/page.js");
            await WriteCodeToNextJsProject(nextjsFileRootPath + $"/pages/{subMenuItem.menu_item}", "page.js", savedPageComponent);
        }
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