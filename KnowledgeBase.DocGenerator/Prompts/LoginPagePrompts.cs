using KnowledgeBase.Models.ReportGenerator;

namespace KnowledgeBase.ReportGenerator.Prompts
{
    public class LoginPagePrompts
    {
        public static string V1(Specification spec, string primaryColor, string secondaryColor, bool generateThirdParty = false)
        {
            string rawPrompt = """
                ## Task
                
                In a javascript + html + tailwind project, generate the code for the login page. 
                
                The project information:
                
                
                - Software Name: ###{service_name}###. 
                - Software Definition: ###{service_desc}###
                
                You should generate the code for the login page for this project. The code should be finished with a closing curly brace. The code you will generate will be in a restricted .js file that have already the code below:
                
                ```javascript
                window.renderLOGINMODULEPage = function(container) {
                    container.innerHTML = `
                        <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
                        </div>
                    `;
                }
                ```
                
                Here's the theme of the project:

                - Primary Color: ###{primary_color}###
                - Secondary Color: ###{secondary_color}###
                
                Here's the exisitng files in the project:
                
                - index.html
                - tailwind.config.js
                - components/main.js
                - components/footer.js
                - components/leftbar.js
                - components/topbar.js
                
                Note: 
                
                - Keep your answer under 24000 characters with a finished code (closing curly brace). Don't make the logical and code too complicated.
                - No third party login method
                - No sms or phone verification method
                - No MFA login method
                
                ## Output Format
                
                Return the pure code only without any explaination, markdown symboles and other characters. Keep your answer under 18000 characters with a finished code.
                """;

            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{primary_color}###", primaryColor)
                .Replace("###{secondary_color}###", secondaryColor);
            return prompt;
        }

    }
}
