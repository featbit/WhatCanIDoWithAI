
using KnowledgeBase.CodingAgent;
using System.Threading.Tasks;

string codingRootPath = @"C:/Code/aicoding/softwarepatent";
string menuItemsFilePath = codingRootPath + "/components/menuitems.js";

FileAgent.RewriteFileContent(menuItemsFilePath,
    """
    window.menuItems = [
        { id: 'path-planning', label: '路径规划', icon: 'cog' },
        { id: 'data-monitoring', label: '数据监测', icon: 'cog' },
        { id: 'treatment-progress', label: '治疗进度', icon: 'cog' },
        { id: 'data-analysis', label: '数据分析', icon: 'cog' }
    ];
    """);


List<FileObject> featureFilePaths = new List<FileObject>()
{
    new FileObject
    {
        Id = "path-planning",
        FeatureName = "路径规划",
        FunctionName = "renderPathPlanningPage"
    },
    new FileObject
    {
        Id = "data-monitoring",
        FeatureName = "数据监测",
        FunctionName = "renderDataMonitoringPage"
    },
    new FileObject
    {
        Id = "treatment-progress",
        FeatureName = "治疗进度",
        FunctionName = "renderTreatmentProgressPage"
    },
    new FileObject
    {
        Id = "data-analysis",
        FeatureName = "数据分析",
        FunctionName = "renderDataAnalysisPage"
    },
};

var subPageInitTasks = featureFilePaths.Select(async f =>
{
    string featureFilePath = codingRootPath + "/components/pages/" + f.Id + ".js";
    string fileContent = """
        window.render###{feature_render_name}###Page = function(container) {
            container.innerHTML = `
                <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
                    <h1 class="text-2xl font-bold mb-6 text-primary">###{feature_name}###</h1>
                    <div id="module1" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
                    ###{module1}###
                    </div>
                    <div id="module2" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
                    ###{module2}###
                    </div>
                    <div id="module3" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
                    ###{module3}###
                    </div>
                </div>
            `;
        }
        
        """;
    fileContent = fileContent.Replace(
        "###{feature_render_name}###", f.Id.Trim().Replace("-", "").ToUpper());
    fileContent = fileContent.Replace(
        "###{feature_name}###", f.FeatureName);

    await FileAgent.CreateAndInitFileAsync(featureFilePath, fileContent);
    return f;
});

(await Task.WhenAll(subPageInitTasks)).ToList();


string dataMonitorFilePath = codingRootPath + "/components/pages/data-monitoring.js";
await FileAgent.ReplaceFileTextAsync(
    dataMonitorFilePath, "###{module1}###",
    """
    <div class="max-w-4xl mx-auto p-4">
        <h1 class="text-2xl font-bold mb-4">智能化路径规划模块</h1>
        <div class="mb-6">
            <h2 class="text-lg font-semibold mb-2">患者信息录入</h2>
            <form>
                <div class="mb-4">
                    <label for="age" class="block text-gray-700 mb-1">年龄</label>
                    <input id="age" type="number" class="w-full border border-gray-300 p-2 rounded" />
                </div>
                <div class="mb-4">
                    <label for="gender" class="block text-gray-700 mb-1">性别</label>
                    <select id="gender" class="w-full border border-gray-300 p-2 rounded">
                        <option>男</option>
                        <option>女</option>
                    </select>
                </div>
                <div class="mb-4">
                    <label for="medicalHistory" class="block text-gray-700 mb-1">病史及过敏史</label>
                    <textarea id="medicalHistory" class="w-full border border-gray-300 p-2 rounded" rows="3"></textarea>
                </div>
                <button type="submit" class="bg-blue-500 text-white px-4 py-2 rounded">生成治疗方案</button>
            </form>
        </div>
        <div>
            <h2 class="text-lg font-semibold mb-2">治疗方案展示</h2>
            <div class="border border-gray-300 p-4 rounded">
                <p>系统会根据患者信息自动计算并生成适合的光疗治疗路径，此处显示具体方案详情。</p>
            </div>
        </div>
    </div>
    """);