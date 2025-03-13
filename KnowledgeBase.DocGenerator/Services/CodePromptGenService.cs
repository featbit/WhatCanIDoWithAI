using KnowledgeBase.Models;
using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.OpenAI;
using KnowledgeBase.ReportGenerator.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Functionality = KnowledgeBase.Models.ReportGenerator.Functionality;

namespace KnowledgeBase.ReportGenerator
{
    public interface ICodePromptGenService
    {
        Task<string> FeatureHomePageGenAsync(
            Specification spec,
            string selectedFeatureId,
            string primaryColor = "#4a90e2", string secondaryColor = "#f8f8f8",
            string fontFamily = "'roboto': ['Roboto', 'sans-serif'],");

        Task<string> FunctionalityGenAsync(
            Specification spec, string selectedFeatureId, string selectedFunctionalityId,
            string primaryColor = "#4a90e2", string secondaryColor = "#f8f8f8",
            string fontFamily = "'roboto': ['Roboto', 'sans-serif'],");

        Task<string> MenuItemCodeGenAsync(Specification spec);

        Task<string> ThemeGenAsync(Specification spec);
    }

    public class CodePromptGenService(
        ILogger<CodePromptGenService> logger,
        IOpenAiChatService openaiChatService,
        IAntropicChatService antropicChatService) : ICodePromptGenService
    {
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
            string primaryColor = "#4a90e2", string secondaryColor = "#f8f8f8",
            string fontFamily = "'roboto': ['Roboto', 'sans-serif'],")
        {
            string rawPrompt = """
                ## Task

                In our SaaS "光疗临床路径智能规划系统", one of features called "治疗过程跟踪" includes 3 functionalities:

                - 实时治疗数据监控 - #/treatment-tracking-0
                - 治疗日志记录功能 - #/treatment-tracking-1
                - 智能提醒功能 - #/treatment-tracking-2

                Each of them has their own sub page with hash route #/xxxx-xxx-x

                You should rewrite the code 

                ```javascript
                window.renderTREATMENTTRACKINGPage = function(container) {
                    container.innerHTML = `
                        <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
                            <h1 class="text-2xl font-bold mb-6 text-primary">治疗过程跟踪</h1>
                            <div id="function" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
                            </div>
                        </div>`;
                }
                ```

                Theme
                - Primary Color: #4a90e2
                - Secondary Color: #f8f8f8

                To let the feature page contains 3 functionalities, when user click on each functinality, the page can be navigated to its corresponds sub page. Don't generate code for functionalities, only code for feature home page as a guide menu page.

                ## Reference information

                About "治疗过程跟踪", it means "治疗过程跟踪功能能够实时监控患者在光疗治疗过程中的各项数据和治疗细节。系统会自动记录每次治疗的实施情况，包括光疗设备的使用参数、治疗时间、患者的反应以及可能出现的副作用等。同时，系统会根据实时数据分析，提醒医生是否需要调整治疗方案或采取其他措施，以确保患者能够得到最佳治疗效果。该功能有助于提高治疗的精准性和安全性，确保每位患者都能接受最适合的治疗"

                About "实时治疗数据监控", it means "实时监控功能是光疗临床路径智能规划系统中的一项关键功能，旨在确保治疗过程中患者的各项数据能够实时被跟踪和监控。该功能通过连接传感器和医疗设备，实时采集患者在光疗过程中的治疗数据，包括光疗设备的使用参数、治疗时间、患者的反应和副作用等。系统会自动记录每一次治疗的详细情况，并根据采集到的数据进行分析，为医生提供实时反馈。如果系统发现治疗过程中出现异常数据或患者的反应不佳，会及时提醒医生调整治疗方案或采取其他必要措施。通过这种方式，实时监控功能能够提高治疗的精准性和安全性，确保患者接受到最适合的光疗方案，最大限度地提高治疗效果。"

                About "治疗日志记录功能", it means "治疗记录功能是光疗临床路径智能规划系统中的一个关键模块，旨在确保每次治疗的详细情况都能得到精确记录。该功能通过自动化方式记录每次光疗治疗的实施细节，包括光疗设备的使用参数、治疗时间、患者的反应以及可能的副作用等。这些数据会实时上传至系统，并生成详细的治疗日志，医生可以随时查看这些日志，确保治疗的透明性和可追溯性。治疗记录功能有助于医生根据实时数据对治疗方案进行调整，及时发现并处理可能出现的问题，确保患者能够获得最安全、最有效的治疗。该功能还可以根据历史治疗记录进行数据分析，为医生提供优化治疗路径的参考依据。"


                ## Ouput format

                Return the pure code only without any explaination, markdown symboles and other characters.
                """;

            string promptInVscode = """
                In our SaaS "光疗临床路径智能规划系统", one of features called "用户管理模块" includes 2 functionalities:

                用户权限控制与角色权限管理 - #/user-management-0
                历史记录管理功能 - #/user-management-1

                Each of them has their own sub page with hash route #/xxxx-xxx-x

                To let the feature page contains 3 functionalities, when user click on each functinality, the page can be navigated to its corresponds sub page. Don't generate code for functionalities, only code for feature home page as a guide menu page.

                Reference information
                About "用户管理", it means "用户管理模块是光疗临床路径智能规划系统中的核心功能之一，旨在有效地管理系统中的各类用户角色，包括医生、患者和管理员。该模块能够根据用户的身份和权限进行相应的操作控制，确保每位用户只能访问其授权范围内的功能。模块还支持管理用户的历史记录，提供灵活的角色权限配置，以便实现更加个性化和精细化的用户管理。"

                About "用户权限控制与角色权限管理", it means "用户权限控制功能是光疗临床路径智能规划系统中的一个重要功能，旨在根据用户的角色和权限，限制其在系统中的操作范围。通过此功能，管理员可以对不同角色（如医生、患者、管理员等）赋予不同的操作权限，确保系统中的数据和操作安全。例如，医生可以查看和修改患者的光疗治疗路径，但不能访问系统设置或其他用户的敏感信息；而患者只能查看自己的治疗进度和记录，不能修改治疗计划。系统管理员则拥有全权限，能够管理所有用户及其权限设置。此外，用户权限控制还支持对一些敏感操作（如删除患者数据、修改治疗计划等）设置访问限制，以保障数据的完整性与安全性。"

                About "历史记录管理功能", it means "历史记录管理功能旨在记录并存储系统中每个用户的操作历史。该功能会追踪并保存所有与治疗相关的记录、用户操作记录以及任何修改或更新操作。通过这个功能，管理员或医生可以方便地查看每个用户的操作行为，确保所有的操作都能够追溯到源头，从而提升系统的透明度和安全性。这有助于及时发现问题并进行修正，确保治疗方案和操作符合医疗规范，进一步提高系统的管理效能和患者的安全保障。"

                Please update rewrite user-management.js file
                """;

            throw new NotImplementedException();
        }

        public async Task<string> FunctionalityGenAsync(
            Specification spec, string selectedFeatureId, string selectedFunctionalityId,
            string primaryColor = "#4a90e2", string secondaryColor = "#f8f8f8", 
            string fontFamily = "'roboto': ['Roboto', 'sans-serif'],")
        {
            Feature selectedFeature = spec.Features.FirstOrDefault(p => p.FeatureId == selectedFeatureId);
            Functionality selectedFunctionality = selectedFeature.Modules.FirstOrDefault(p => p.Id == selectedFunctionalityId);

            string rawPrompt = """

                ## Task

                In a javascript + html + tailwind project, generate the code for the functionality and feature described below:

                
                - Software: ###{service_name}###. ###{service_desc}###
                - Feature Description: ###{feature_desc}###
                - Functionality Description: ###{functionality_desc}###

                You should generate the code for the Functionality described above. The code should be finished with a closing curly brace. The code you will generate will be in a restricted .js file that have already the code below:

                ```javascript
                window.render###{method_name}###Page = function(container) {
                    container.innerHTML = `
                        <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
                            <h1 class="text-2xl font-bold mb-6 text-primary">###{feature_name}###</h1>
                            <div id="function" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
                                <h2>###{functionality_name}###</h2>
                            </div>
                        </div>
                    `;
                }
                ```

                Here's the theme code will help you to generate the correct theme for the functionality:

                ```javascript
                tailwind.config = {
                    darkMode: 'class',
                    theme: {
                        extend: {
                            colors: {
                                primary: '###{primary_color}###',
                                secondary: '###{secondary_color}###',
                            },
                            fontFamily: {
                                ###{font_family}###
                            },
                        }
                    }
                }
                ```

                Here's the exisitng files in the project:

                - index.html
                - tailwind.config.js
                - components/main.js
                - components/footer.js
                - components/leftbar.js
                - components/topbar.js
                ###{feature_files}###

                Note: Keep your answer under 24000 characters with a finished code (closing curly brace). Don't make the logical and code too complicated.

                ## Output Format

                Return the pure code only without any explaination, markdown symboles and other characters. Keep your answer under 24000 characters with a finished code.

                """;

            
            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{feature_desc}###", selectedFeature.Name + ". " + selectedFeature.Description)
                .Replace("###{feature_name}###", selectedFeature.Name)
                .Replace("###{functionality_desc}###", selectedFunctionality?.Name + ". " + selectedFunctionality?.DetailDescription)
                .Replace("###{functionality_name}###", selectedFunctionality?.Name)
                .Replace("###{method_name}###", selectedFeature.MenuItem.Replace("-", "").ToUpper())
                .Replace("###{feature_files}###", string.Join("\n", spec.Features.Select(f => $"- components/pages/{f.MenuItem}.js")))
                .Replace("###{primary_color}###", primaryColor)
                .Replace("###{secondary_color}###", secondaryColor)
                .Replace("###{font_family}###", fontFamily);

            string code = await antropicChatService.CompleteChatAsync(prompt);
            code = code.CleanJsCodeQuote();
            return code;
        }

        public async Task<string> MenuItemCodeGenAsync(Specification spec)
        {
            string rawPrompt = """
                ## Task

                Modify the existing code, change the menu items with feature data. Each feature corresponds to a menu item.

                Each menu item should has its id, label (in chinese) and icon (svg).
                    
                Return a the new code as output.

                ### Existing code

                ```js
                window.menuItems = [
                    { id: 'home', label: '控制面板', icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" /></svg>' }
                ];
                ```

                ### feature data 

                Here are features information, includes: MenuItem (id), feature name, and feature description
                
                each feature corresponds to a menu item:

                ###{feature_data}###

                ## Output Format
                    
                Return the pure code only without any explaination, markdown symboles and other characters.

                ## Output Example

                window.menuItems = [
                    { id: 'user-login', label: '登录页面', icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" /></svg>' },
                    { id: 'users-management', label: '用户管理', icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>' },
                    { id: 'calendar-schedule', label: '预约管理', icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>' }
                ];

                """;

                //List<string> menuItems = spec.Features.Select(f => "- " + f.MenuItem).ToList();
                List<MenuItemFeature> menuItems = spec.Features.Select(f => new MenuItemFeature
                { 
                    MenuItem = f.MenuItem,
                    Name = f.Name,
                    Description = f.Description
                }).ToList();
            string prompt = rawPrompt
                .Replace("###{feature_data}###", 
                        JsonSerializer.Serialize<List<MenuItemFeature>>(
                            menuItems, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) }));

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
