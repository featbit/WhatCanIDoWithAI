using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.OpenAI;
using KnowledgeBase.ReportGenerator.Models;
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

            string code = await antropicChatService.CompleteChatAsync(prompt);
            return code;
            //return JsonSerializer.Deserialize<Functionality>(codeJson);
        }

        public async Task<string> MenuItemCodeGenAsync(Specification spec)
        {
            string rawPrompt = """
                ## Task

                Modify the existing code, change the menu items with feature data. Each feature corresponds to a menu item.
                    
                Return a the new code as output.

                ### Existing code

                ```js
                window.menuItems = [
                    { id: 'home', label: 'Dashboard', icon: 'cog' }
                ];
                ```

                ### feature data 

                Here are name of features, each feature corresponds to a menu item:

                ###{feature_data}###

                ## Output Format
                    
                Return the pure code only without any explaination, markdown symboles and other characters.

                ## Output Example

                window.menuItems = [
                    { id: 'item-1', label: 'Item one', icon: 'cog' },
                    { id: 'item-2', label: 'Item one', icon: 'cog' },
                    { id: 'item-3', label: 'Item one', icon: 'cog' }
                ];

                """;

            List<string> menuItems = spec.Features.Select(f => "- " + f.MenuItem).ToList();
            string prompt = rawPrompt
                .Replace("###{feature_data}###", string.Join("\n", menuItems));

            return await openaiChatService.CompleteChatAsync(prompt);
            //return await antropicChatService.CompleteChatAsync(prompt);
        }
    }
}
