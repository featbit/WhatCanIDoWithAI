using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.ReportGenerator.Models;
using System.Text.Json;
using Functionality = KnowledgeBase.Models.ReportGenerator.Functionality;

namespace KnowledgeBase.ReportGenerator.Prompts
{
    public class FunctionalityPrompts
    {
        public static string V1(Specification spec, string featureId, string functionalityId, string primaryColor, string secondaryColor, string additionalSpec = "No addtional spec", bool generateThirdParty = false)
        {
            Feature selectedFeature = spec.Features.FirstOrDefault(p => p.FeatureId == featureId);
            Functionality selectedFunctionality = selectedFeature.Modules.FirstOrDefault(p => p.Id == functionalityId);
            int funcIndex = selectedFeature.Modules.IndexOf(selectedFunctionality);
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
                            }
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

                
                Additional Specicifcation:
                
                ###{additional_spec}###

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
                .Replace("###{method_name}###", selectedFeature.MenuItem.Replace("-", "").ToUpper() + funcIndex.ToString())
                .Replace("###{feature_files}###", string.Join("\n", spec.Features.Select(f => $"- components/pages/{f.MenuItem}.js")))
                .Replace("###{primary_color}###", primaryColor)
                .Replace("###{secondary_color}###", secondaryColor)
                .Replace("###{additional_spec}###", additionalSpec);

            return prompt;
        }

        public static string ModifyExistingCode(
                Specification spec, string featureId, string functionalityId, 
                string primaryColor, string secondaryColor, string changeDescription, string existingCode, bool generateThirdParty = false)
        {
            Feature selectedFeature = spec.Features.FirstOrDefault(p => p.FeatureId == featureId);
            Functionality selectedFunctionality = selectedFeature.Modules.FirstOrDefault(p => p.Id == functionalityId);
            string rawPrompt = """

                ## Task
                
                In our SaaS "###{service_name}###", has one feature:
                
                - Software: ###{service_name}###. ###{service_desc}###
                - Feature Description: ###{feature_desc}###
                - Functionality Description: ###{functionality_desc}###
                
                You should modify (updating) the existing code based on the change or issue description:
                
                ###{change_description}###
                
                
                The code you will generate will be in a restricted .js file that have already the existing code below:
                
                ```javascript
                ###{existing_code}###
                ```
                
                Theme of the project:
                
                - Primary Color: ###{primary_color}###
                - Secondary Color: ###{secondary_color}###
                
                ## Output Format
                
                Return the pure code only without any explaination, markdown symboles and other characters. Keep your answer under 48000 characters with a finished code.
                """;

            var feature = spec.Features.FirstOrDefault(p => p.FeatureId == featureId);

            List<SubMenuItem> subMenuItems = feature.Modules.Select((m, i) => new SubMenuItem
            {
                MenuItem = feature.MenuItem + "-" + i.ToString(),
                Name = m.Name,
                ShortDescription = m.ShortDescription
            }).ToList();

            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{feature_name}###", feature.Name)
                .Replace("###{feature_desc}###", feature.Description)
                .Replace("###{functionality_desc}###", selectedFunctionality?.Name + ". " + selectedFunctionality?.DetailDescription)
                .Replace("###{change_description}###", changeDescription)
                .Replace("###{existing_code}###", existingCode)
                .Replace("###{primary_color}###", primaryColor)
                .Replace("###{secondary_color}###", secondaryColor);
            return prompt;
        }
    }
}
