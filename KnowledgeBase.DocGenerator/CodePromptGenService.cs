using KnowledgeBase.DataModels.ReportGenerator;
using KnowledgeBase.OpenAI;

namespace KnowledgeBase.ReportGenerator
{
    public interface ICodePromptGenService
    {
        Task<string> FeatureModuleGenAsync(
            string serviceName, string serviceDescription, string featureDescription, string moduleDescription);

        Task<string> MenuItemCodeGenAsync(Specification spec);
    }

    public class CodePromptGenService(IOpenAiChatService openaiChatService) : ICodePromptGenService
    {
        public async Task<string> FeatureModuleGenAsync(
            string serviceName, string serviceDescription, string featureDescription, string moduleDescription)
        {
            string rawPrompt = """
                I am writing the ui code for a module of a feature of a software. You need to help me to generate the UI code with tailwind with the following information:

                ## Information

                ### Service Name
                
                ###{service_name}###

                ### Service Description

                ###{service_desc}###

                ### Feature Description
                
                ###{feature_desc}###
                
                ### Module Description
                
                ###{module_desc}###

                ## Output Format
                
                Return the pure code only without any explaination, markdown symboles and other characters.

                Note:

                1. The human language of the content should be in Chinese.
                2. Don't generate too many components, just generate the components that are necessary.
                3. The components should be simple and easy to understand.
                4. Don't use images.
                5. Use html and tailwind as css.

                """;

            string prompt = rawPrompt
                .Replace("###{service_name}###", serviceName)
                .Replace("###{service_desc}###", serviceDescription)
                .Replace("###{feature_desc}###", featureDescription)
                .Replace("###{module_desc}###", moduleDescription);

            return await openaiChatService.CompleteChatAsync(prompt, "spec-gen-module");
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

            return await openaiChatService.CompleteChatAsync(prompt, "spec-gen-module");
        }
    }
}
