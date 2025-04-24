using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;
using FeatGen.CodingAgent.Models;
using Newtonsoft.Json;

namespace FeatGen.ReportGenerator.Prompts
{
    public class Step9Page
    {
        public static string Prompt(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string apiCode, string cssCode, string dbCode, string dbModels, string themeIconPrompt, string themeChartPrompt)
        {
            string rawPrompt = """

                ## Context

                We're developing a software named "###{service_name}###" - ###{service_desc}###.

                We're now developing a page:
                
                ###{page_desc}###.

                We also have resources below:

                - Existing code of file "###{api_file_path_n_name}" that front-end code calls to provide the API services.
                - Existing code of database file that is used to provide the data for the API services.
                - Database models that are used for generating database file and api file.
                - File globals.css that define the css of the theme of the page.

                Frontend page code calls the APIs in "###{api_file_path_n_name}###" file to create, read, update and delete operation for buisness logic.

                ## Content and Code of Files

                - "###{api_file_path_n_name}###" file:

                ```javascript
                ###{api_code}###
                ```

                - Existing code of database file:

                ```json
                ###{db_file_code}###
                ```
                
                - Globals.css file:
              
                ```css
                ###{css_code}###
                ```

                - Database models
                
                ```javascript
                ###{db_models}###
                ```
                
                ## Task 

                Based on the requirement, specification, existing api code, database code, database models and theme css code, please generate front-end page codeby using export functions defined in api file "###{db_file_path_n_name}###".

                Note:
                
                - Front-end page should use APIs defined in the new version "###{api_file_path_n_name}###" file, `import {  } from '###{api_file_path}###';`
                - You shouldn't change existing default export function name.
                - You need to write all related components in one file instead write in other files and import it.
                - Please use className defined in the Globals.css file to style the page for background, text color, font size and family.
                - ###{theme_icon_prompt}###
                - ###{theme_chart_prompt}###
                - We're using React19
                - The primary language is Chinese

                Existing front-end page code:

                ```javascript
                "use client";
                
                import { 
                } from '###{api_file_path}###';
                
                export default function ###{page_component_name}###() {  
                  return (
                    <div className="">
                    </div>
                  );
                }
                ```

                ## Output Format
                
                Return the pure code only without any explaination, markdown symboles and other characters.

                """;
            var menuItemsString = rcg.MenuItems;
            var menuItems = System.Text.Json.JsonSerializer.Deserialize<List<GuideMenuItem>>(menuItemsString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var pagesString = rcg.Pages;
            var allPages = System.Text.Json.JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var subPages = allPages.Where(p =>
                    mainPage.related_pages.Any(p => p.page_id == pageId && p.direction == "forward") &&
                    menuItems.All(m => m.page_id != p.page_id)).ToList();
            var pages = new List<GuidePageItem>() { mainPage };
            pages.AddRange(subPages);
            string pageDesc = System.Text.Json.JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            string pageComponentName = menuItem.Replace("-", "").Replace("_", "").Replace(" ", "").ToUpperInvariant();

            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_desc}###", pageDesc)
                .Replace("###{api_file_path_n_name}###", $"@/app/apis/{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{api_file_path}###", $"@/app/apis/{menuItem.Replace("_", "-").Trim().ToLower()}")
                .Replace("###{api_code}###", apiCode)
                .Replace("###{db_file_code}###", dbCode)
                .Replace("###{css_code}###", cssCode)
                .Replace("###{db_models}###", dbModels)
                .Replace("###{theme_icon_prompt}###", themeIconPrompt)
                .Replace("###{theme_chart_prompt}###", themeChartPrompt)
                .Replace("###{page_component_name}###", pageComponentName);

            return prompt;
        }

    }


    


    
}
