using FeatGen.Models.ReportGenerator;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator.Models;
using FeatGen.ReportGenerator.Prompts;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Functionality = FeatGen.Models.ReportGenerator.Functionality;

namespace FeatGen.ReportGenerator
{
    public interface ICodePromptGenService
    {
        Task<string> LoginPageGenAsync(
            Specification spec, 
            string primaryColor = "#4a90e2", string secondaryColor = "#f8f8f8");

        Task<string> FeatureHomePageGenAsync(
            Specification spec,
            string selectedFeatureId,
            string primaryColor,
            string secondaryColor,
            string genVersion,
            string additionalSpec = "No additional spec",
            string existingCode = "");

        Task<string> FunctionalityGenAsync(
            Specification spec, string selectedFeatureId, string selectedFunctionalityId,
            string primaryColor = "#4a90e2", string secondaryColor = "#f8f8f8",
            string genVersion = "V1",
            string additionalSpec = "No additional spec",
            string existingCode = "",
            string fontFamily = "'roboto': ['Roboto', 'sans-serif'],");

        Task<string> MenuItemCodeGenAsync(Specification spec);

        Task<string> ThemeGenAsync(Specification spec);
    }

    public class CodePromptGenService(
        ILogger<CodePromptGenService> logger,
        IOpenAiChatService openaiChatService,
        IAntropicChatService antropicChatService) : ICodePromptGenService
    {
        public async Task<string> LoginPageGenAsync(
            Specification spec,
            string primaryColor, string secondaryColor)
        {
            string prompt = LoginPagePrompts.V1(spec, primaryColor, secondaryColor);

            string code = await antropicChatService.CompleteChatAsync(prompt);
            code = code.CleanJsCodeQuote();
            return code;
        }

        public async Task ApplyFormGenAsync()
        {
            string rawPrompt = """
                我有如下文件示例，需要根据我的软件信息将下面的txt内容替换

                ```txt
                【软件全称】混合云环境下的容器编排与管理平台 
                【版本号】V1.0
                【软件分类】应用软件
                【开发的硬件环境】CPU：Core i7 3.2GHz，内存：32GB，联想电脑
                【运行的硬件环境】CPU主频： 3.0GHz及以上，内存频率： 1333MHz，内存容量： 16GB
                【开发该软件的操作系统】windows10
                【软件开发环境/开发工具】vs2010
                【该软件的运行平台/操作系统】windows10
                【软件运行支撑环境/支持软件】Windows 10；Oracle, MySQL
                【编程语言】C语言
                【源程序量】13227行
                【开发目的】为了进行容器编排的综合服务
                【面向领域/行业】容器编排
                【软件的主要功能】混合云环境下的容器编排与管理平台，系统内可以完成相关的电容器监测的数据信息处理，建立合理的电容器监测模式数据信息的处理，电容器监测的模式和数据信息也可以在系统内进行记录和管理，信息化模式高效，管理模式便利，随时都可以处理数据信息，搭载良好的管理需求。
                【软件的技术特点】云计算软件。功能强大，网络安全机制健全，可以支持高效的数据库运维方式，可以有效的进行数据的维护和展示。
                ```

                我的软件信息：

                软件全称：光疗临床路径智能规划系统
                版本号：V1.0 
                软件分类：应用软件
                使用语言和框架：Javascript
                程序代码行：超过1万行
                软件的定义与模块：光疗临床路径智能规划系统是一款基于人工智能技术的医疗辅助软件，旨在帮助医生和医疗机构制定个性化、科学化的光疗治疗路径，提高治疗效率和病患的康复效果。软件包括主要模块：智能路径推荐，治疗过程跟踪，数据分析与报告生成与用户管理模块。
                系统运行环境：可在 linux, windows server, macos 上运行和假设；用户需使用浏览器访问服务
                开发IDE：vscode
                开发机：可以运行 javascript, npm 和 node.js 的电脑都可以，请根据案例帮我生成

                请只返回核心文本内容。注：【软件的主要功能】、的内容应该在100-180中文字之间。

                
                """;
            throw new NotImplementedException();
        }

        public async Task<string> FeatureHomePageGenAsync(
            Specification spec,
            string selectedFeatureId,
            string primaryColor, string secondaryColor,
            string genVersion,
            string additionalSpec = "No additional spec",
            string existingCode = "")
        {
            string code = "", prompt = "";
            if(genVersion == "V1")
            {
                prompt = FeaturePagePrompts.V1(spec, selectedFeatureId, primaryColor, secondaryColor, additionalSpec);
            }
            else if(genVersion == "V2")
            {
                prompt = FeaturePagePrompts.V2OnlyFeaturePage(spec, selectedFeatureId, primaryColor, secondaryColor, additionalSpec);
            }
            else if(genVersion == "Modify")
            {
                prompt = FeaturePagePrompts.ModifyExistingCode(
                    spec, selectedFeatureId, primaryColor, secondaryColor, additionalSpec, existingCode);
            }
            code = await antropicChatService.CompleteChatAsync(prompt);
            code = code.CleanJsCodeQuote();
            return code;
        }

        public async Task<string> FunctionalityGenAsync(
            Specification spec, string selectedFeatureId, string selectedFunctionalityId,
            string primaryColor = "#4a90e2", string secondaryColor = "#f8f8f8",
            string genVersion = "V1",
            string additionalSpec = "No additional spec",
            string existingCode = "",
            string fontFamily = "'roboto': ['Roboto', 'sans-serif'],")
        {
            string prompt = "";
            if(genVersion == "V1")
            {
                prompt = FunctionalityPrompts.V1(
                    spec, selectedFeatureId, selectedFunctionalityId,
                    primaryColor, secondaryColor, additionalSpec);
            }
            else if(genVersion == "Modify")
            {

                prompt = FunctionalityPrompts.ModifyExistingCode(
                    spec, selectedFeatureId, selectedFunctionalityId,
                    primaryColor, secondaryColor, additionalSpec, existingCode);
            }

            string code = await antropicChatService.CompleteChatAsync(prompt);
            code = code.CleanJsCodeQuote();
            return code;
        }

        public async Task<string> MenuItemCodeGenAsync(Specification spec)
        {
            //string prompt = MenuItemCodePrompts.VBefor20250314(spec);
            string prompt = MenuItemCodePrompts.V2After20250314(spec);

            return await antropicChatService.CompleteChatAsync(prompt);
        }

        public async Task<string> ThemeGenAsync(Specification spec)
        {
            string rawPrompt = """

                ## Task

                In a javascript + html + tailwind project, you need to define the them based on the project  information (name, definition and features).

                Project Information:

                - Software: ###{service_name}###. ###{service_desc}###
                - Features:
                ###{feature_data}###


                Here're the code snippet that will use the theme:
                ```javascript
                tailwind.config = {
                    darkMode: 'class', // example of theme dark mode in tailwind
                    theme: {
                        extend: {
                            colors: {
                                primary: '#4a90e2', // example of primary color
                                secondary: '#f8f8f8', // example of secondary color
                            },
                            fontFamily: {
                                'roboto': ['Roboto', 'sans-serif'], // example of font family
                            },
                        }
                    }
                }

                <!-- 
                example of body background color in tailwind: bg-white 
                example of body dark mode background color in tailwind: dark:bg-gray-900
                example of text color in tailwind: text-gray-800
                example of dark mode text color in tailwind: dark:text-gray-200 
                -->
                <body class="bg-white dark:bg-gray-900 text-gray-800 dark:text-gray-200">
                    <div class="flex flex-col h-screen">
                        <!-- Top Bar -->
                        <div id="topbar" />
                        <div class="flex flex-1 overflow-hidden">
                            <!-- Left Bar -->
                            <div id="leftbar" class="flex-shrink-0" />
                            <!-- Main Content -->
                            <div id="main" class="flex-1 overflow-y-auto p-6" />
                        </div>
                        <!-- Footer -->
                        <div id="footer" />
                    </div>

                    <!-- Import component scripts -->
                    <script src="components/menuitems.js"></script>
                    <script src="components/topbar.js"></script>
                    <script src="components/leftbar.js"></script>
                    <script src="components/main.js"></script>
                    <script src="components/footer.js"></script>
                </body>
                ```

                So you should generate the theme data below:

                - theme dark mode in tailwind
                - theme primary color
                - theme secondary color
                - theme font family
                - body dark mode background color in tailwind
                - body dark mode background color in tailwind
                - text color in tailwind
                - dark mode text color in tailwind

                ## Output Format

                Return the code in json format without any explaination, markdown symboles and other characters. :

                {
                    "DarkMode": "", // theme dark mode in tailwind                   
                    "FontFamily": "", // theme font family
                    "PrimaryColor": "", // theme primary color
                    "SecondaryColor": "", // theme secondary color
                    "BodyBgColor": "", // body background color in tailwind
                    "BodyBgColorDrakMode": "", // body dark mode background color in tailwind
                    "TextColor": "",  // text color in tailwind
                    "TextColorDarkMode": "" // dark mode text color in tailwind
                }

                ## Output Example

                {
                    "DarkMode": "class",            
                    "FontFamily": "'roboto': ['Roboto', 'sans-serif']",
                    "PrimaryColor": "#4a90e2",
                    "SecondaryColor": "#f8f8f8",
                    "BodyBgColor": "bg-white",
                    "BodyBgColorDrakMode": "dark:bg-gray-900",
                    "TextColor": "text-gray-800",
                    "TextColorDarkMode": "dark:text-gray-200"
                }

                """;


            List<MenuItemFeature> menuItems = spec.Features.Select(f => new MenuItemFeature
            {
                MenuItem = f.MenuItem,
                Name = f.Name,
                Description = f.Description
            }).ToList();
            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{feature_data}###", JsonSerializer.Serialize<List<MenuItemFeature>>(
                            menuItems, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) }));

            string code = await antropicChatService.CompleteChatAsync(prompt);
            return code;
        }
    }
}
