
//using FeatGen.CodingAgent;
//using FeatGen.Models.ReportGenerator;


//const string reportId = "7a11797a-4f40-4fb5-bd43-f31a7e957df4";

//string codingRootPath = @"C:/Code/aicoding/softwarepatent";


//Specification spec = await ApiGenCaller.GetSpecificationAsync(reportId);

//string menuItemsCode = await ApiGenCaller.GenerateMenuItemsCodeAsync(reportId);
//string menuItemsFilePath = codingRootPath + "/components/menuitems.js";
//FileAgent.RewriteFileContent(menuItemsFilePath, menuItemsCode);

//await CreateSubpageFilesAndInitCodeAsync(codingRootPath, spec);


//for (int i = 0; i < spec.Features.Count; i++)
//{
//    if (spec.Features[i].MenuItem == "data-analysis-report")
//        continue;
//    Console.WriteLine("Generating code for " + spec.Features[i].MenuItem);
//    string code = await ApiGenCaller.GenerateFunctionalityCodeAsync(
//        reportId, spec.Features[i].FeatureId, spec.Features[i].Modules[0].Id);
//    string filePath = codingRootPath + "/components/pages/" + spec.Features[i].MenuItem + ".js";
//    FileAgent.RewriteFileContent(filePath, code);
//}

//static async Task CreateSubpageFilesAndInitCodeAsync(string codingRootPath, Specification spec)
//{
//    var subPageInitTasks = spec.Features.Select(async f =>
//    {
//        string featureFilePath = codingRootPath + "/components/pages/" + f.MenuItem + ".js";
//        string fileContent = """
//        window.render###{feature_render_name}###Page = function(container) {
//            container.innerHTML = `
//                <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
//                    <h1 class="text-2xl font-bold mb-6 text-primary">###{feature_name}###</h1>
//                    <div id="module1" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
//                    </div>
//                    <div id="module2" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
//                    </div>
//                    <div id="module3" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
//                    </div>
//                </div>
//            `;
//        }
        
//        """;
//        fileContent = fileContent.Replace(
//            "###{feature_render_name}###", f.MenuItem.Replace("-", "").ToUpper());
//        fileContent = fileContent.Replace(
//            "###{feature_name}###", f.Name);

//        await FileAgent.CreateAndInitFileAsync(featureFilePath, fileContent);
//        return f;
//    });
//    (await Task.WhenAll(subPageInitTasks)).ToList();
//}