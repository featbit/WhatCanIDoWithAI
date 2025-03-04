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
        Task<string> FunctionalityGenAsync(
            Specification spec, string selectedFeatureId, string selectedFunctionalityId,
            string primaryColor = "#4a90e2", string secondaryColor = "#f8f8f8",
            string fontFamily = "'roboto': ['Roboto', 'sans-serif'],");

        Task<string> MenuItemCodeGenAsync(Specification spec);
    }

    public class CodePromptGenService(
        ILogger<CodePromptGenService> logger,
        IOpenAiChatService openaiChatService,
        IAntropicChatService antropicChatService) : ICodePromptGenService
    {
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

                You should generate the code for the Functionality described above. The code you will generate will be in a restricted .js file "path-planning.js" that have already the code below:

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

                ## Output Format

                Return the pure code only without any explaination, markdown symboles and other characters.

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

            logger.LogDebug($"CodePromptGenService: {prompt}");

            string code = await antropicChatService.CompleteChatAsync(prompt);
            return code;
            //return JsonSerializer.Deserialize<Functionality>(codeJson);
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

            //return await openaiChatService.CompleteChatAsync(prompt);
            return await antropicChatService.CompleteChatAsync(prompt);
        }
    }
}
