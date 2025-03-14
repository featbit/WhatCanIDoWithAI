using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.ReportGenerator.Models;
using System.Text.Json;

namespace KnowledgeBase.ReportGenerator.Prompts
{
    public class FeaturePagePrompts
    {
        public static string V1(Specification spec, string featureId, string primaryColor, string secondaryColor, bool generateThirdParty = false)
        {
            string rawPrompt = """

                ## Task

                In our SaaS "###{service_name}###", has one feature:
                
                - Feature Name: ###{feature_name}###
                - Feature Description : ###{feature_description}###
                
                This feature includes ###{functionalities_number}### functionalities, described as below:
                
                ###{functionality_data}###
                
                You should generate the code for the feature page described above. 

                - When user click on each functinality, the page can be navigated to its corresponds sub page. Don't generate code for functionalities, only code for feature home page as a guide menu page. 
                - "MenuItem" indicates each functionality's hash path.
                - The code should be finished with a closing curly brace. 
                
                The code you will generate will be in a restricted .js file that have already the code below:
                
                ```javascript
                window.render###{method_name}###Page = function(container) {
                    container.innerHTML = `
                        <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
                        </div>
                    `;
                }
                ```
                
                Theme of the project:
                
                - Primary Color: ###{primary_color}###
                - Secondary Color: ###{secondary_color}###

                ## Output Format
                
                Return the pure code only without any explaination, markdown symboles and other characters. Keep your answer under 24000 characters with a finished code.
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
                .Replace("###{feature_description}###", feature.Description)
                .Replace("###{functionalities_number}###", feature.Modules.Count.ToString())
                .Replace("###{functionality_data}###",
                        JsonSerializer.Serialize<List<SubMenuItem>>(
                            subMenuItems, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) }))
                .Replace("###{method_name}###", feature.MenuItem.Trim().Replace("-", "").ToUpper())
                .Replace("###{primary_color}###", primaryColor)
                .Replace("###{secondary_color}###", secondaryColor);
            return prompt;
        }

        /// <summary>
        /// Render functionalities together, instead of 1 by 1 to avoid inconsistent scenarios.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="featureId"></param>
        /// <param name="primaryColor"></param>
        /// <param name="secondaryColor"></param>
        /// <param name="generateThirdParty"></param>
        /// <returns></returns>
        public static string V2(Specification spec, string featureId, string primaryColor, string secondaryColor, bool generateThirdParty = false)
        {
            return "";
        }
    }
}
