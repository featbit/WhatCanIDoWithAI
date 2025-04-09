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
    public class GuideCodeGenPageComponent
    {
        public static string V1(
            Specification spec, ReportCodeGuide rcg, string pageId,
            string menuItem, string apiCode, string cssCode)
        {
            string rawPrompt = """

                ## Context

                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to generate the next js page component code for frontend.

                In this task, we need to generate the next js page component code based on the information:

                - Page description 
                - Description of features and functionalities of the page
                - Description of inner pages or components in the page
                - APIs (or functions) that the page can use for data exchange

                ## Data and Preparation

                Here's the Page, Inner Pages/Components, Features and Functionalities:

                ###{page_features}###

                Here's API endpoints, functions and the fake data that the page can use for data exchange. This API endpoints are coded in the file `/app/apis/###{menu_item}###.js`:

                ```javascript
                ###{api_endpoints}###
                ```

                Here's the global css classname that you can use

                ```css
                ###{css_code}###
                ```

                Page to be generated is located at folder "/app/###{menu_item}###/page.js" . Existing code in the NextJs page file:
                
                ```javascript
                "use client";
                
                import { 
                } from '@/app/apis/###{page_component_name}###';

                export default function ###{page_component_name}###() {  
                  return (
                    <div className="py-6 flex h-[calc(100vh-64px)]">
                      {/* Left sidebar - Sessions history */}
                    </div>
                  );
                }
                ```

                ## Task

                Based on the information above, please regenerate the code for this nextjs page file located at '/app/###{menu_item}###/page.js'.

                Note:

                - You shouldn't change existing default export function name.
                - You can add more code or functions if needed.
                - You need to write all related components in one file instead write in other files and import it.

                ## Output format

                Return the pure code only without any explaination, markdown symboles and other characters.


                """;
            var menuItemsString = rcg.MenuItems.CleanJsCodeQuote().CleanJsonCodeQuote();
            var menuItems = System.Text.Json.JsonSerializer.Deserialize<List<GuideMenuItem>>(menuItemsString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var pagesString = rcg.Pages.CleanJsonCodeQuote();
            var allPages = System.Text.Json.JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var subPages = allPages.Where(p =>
                    p.related_pages.Any(p => p.page_id == pageId) &&
                    menuItems.All(m => m.page_id != p.page_id)).ToList();

            var pages = new List<GuidePageItem>() { mainPage };
            pages.AddRange(subPages);

            string pageDesc = System.Text.Json.JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            string pageComponentName = menuItem.Replace("-", "").Replace("_", "").Replace(" ", "").ToUpperInvariant();
            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_features}###", pageDesc)
                .Replace("###{api_endpoints}###", apiCode)
                .Replace("###{css_code}###", cssCode)
                .Replace("###{menu_item}###", menuItem)
                .Replace("###{page_component_name}###", pageComponentName);
            return prompt;
        }

        public static string V1Login(
            Specification spec, ReportCodeGuide rcg, string pageId, string apiCode, string cssCode)
        {
            string rawPrompt = """

                ## Context

                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to generate the next js page component code for frontend.

                In this task, we need to generate the next js page component code based on the information:

                - Page description 
                - Description of features and functionalities of the page
                - Description of inner pages or components in the page
                - APIs (or functions) that the page can use for data exchange

                ## Data and Preparation

                Here's the Page, Inner Pages/Components, Features and Functionalities:

                ###{page_features}###

                Here's API endpoints, functions and the fake data that the page can use for data exchange. This API endpoints are coded in the file `/app/apis/###{menu_item}###.js`:

                ```javascript
                ###{api_endpoints}###
                ```

                Here's the global css classname that you can use

                ```css
                ###{css_code}###
                ```

                Page to be generated is located at folder "/app/###{menu_item}###/page.js" . Existing code in the NextJs page file:
                
                ```javascript
                import { 
                } from '@/app/apis/###{page_component_name}###';

                export default function ###{page_component_name}###() {  
                  return (
                    <div className="py-6 flex h-[calc(100vh-64px)]">
                      {/* Left sidebar - Sessions history */}
                    </div>
                  );
                }

                ## Task

                Based on the information above, please regenerate the code for this nextjs page file located at '/app/###{menu_item}###/page.js'.

                Note:

                - You shouldn't change existing default export function name.
                - You can add more code or functions if needed.
                - You need to write all related components in one file instead write in other files and import it.

                ## Output format

                Return the pure code only without any explaination, markdown symboles and other characters.


                """;

            var pagesString = rcg.Pages.CleanJsonCodeQuote();
            var allPages = System.Text.Json.JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);

            var pages = new List<GuidePageItem>() { mainPage };

            string pageDesc = System.Text.Json.JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            string pageComponentName = pageId.ToUpperInvariant();
            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_features}###", pageDesc)
                .Replace("###{api_endpoints}###", apiCode)
                .Replace("###{css_code}###", cssCode)
                .Replace("###{menu_item}###", pageId)
                .Replace("###{page_component_name}###", pageComponentName);
            return prompt;
        }
    
        /// <summary>
        /// version that for seperated components of one page
        /// </summary>
        /// <returns></returns>
        public static string V2(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string apiCode, string cssCode, PageComponentFilesObject pcfo, string componentId)
        {
            string rawPrompt = """
                
                Here's the description of whole page logic and behavior relationship between the page main content and file independant components:

                ###{page_file_logic}###

                This is in a nextjs project

                """;

            if(pcfo.main_page_description.id == componentId)
            {

            }
            else
            {
                var component = pcfo.components.FirstOrDefault(p => p.component_id == componentId);

            }



                var pagesString = rcg.Pages.CleanJsonCodeQuote();
            var allPages = System.Text.Json.JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var pages = new List<GuidePageItem>() { mainPage };

            string pageDesc = System.Text.Json.JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_file_logic}###", JsonConvert.SerializeObject(pcfo))
                .Replace("###{api_code}###", apiCode)
                .Replace("###{css_code}###", cssCode);

            return "";
        }

        public static string V3GenerateWithNewAPI(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string apiCode, string cssCode, string dbCode, string dbModels, string themeIconPrompt, string themeChartPrompt)
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
                    <div className="py-6 flex h-[calc(100vh-74px)]">
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

        public static string V3UpdateWithNewAPI(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string apiCode, string cssCode, string dbCode, string exstingPageCode, string themeIconPrompt, string themeChartPrompt)
        {
            string rawPrompt = """

                ## Context

                We're developing a software named "###{service_name}###" - ###{service_desc}###.

                We're now developing a page:
                
                ###{page_desc}###.

                We also have resources below:

                - Existing code of file "###{api_file_path_n_name}" that front-end code calls to provide the API services.
                - Existing code of database file that is used to provide the data for the API services.
                - File globals.css that define the css of the theme of the page.
                - Existing code of front-end page that will be updated in this task.

                Frontend code calls the APIs in "###{api_file_path_n_name}###" file to create, read, update and delete operation for buisness logic.

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

                - Existing code of front-end page:
                
                ```javascript
                ###{existing_page_code}###
                ```
                
                ## Task 

                Based on the requirement, specification, existing api code, db code and theme css code, please update the existing front-end page code by using export functions defined in api file "###{db_file_path_n_name}###".

                Note:

                - This "###{api_file_path_n_name}###" file should import db file from "###{db_file_path}###" that is where the db file located;
                - If "###{db_file_path}###" lack of interfaces or data to provide correct service, "###{api_file_path_n_name}###" can add code to simulate it.
                - Try to keep the name of functions to be exported in existing file "###{api_file_path_n_name}###".
                - Please remove import from existing shared db file like `import { memoryDB } from '@/app/db/memoryDB';` Delete it and update the code by using functions in "###{db_file_path_n_name}###".

                - Front-end page should use APIs defined in the new version "###{api_file_path_n_name}###" file, `import {  } from '###{api_file_path}###';`
                - Please try to keep the current front-end business logic as much as possible.
                - 
                - Please use className defined in the Globals.css file to style the page for background, text color, font size and family.
                - ###{theme_icon_prompt}###
                - ###{theme_chart_prompt}###

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


            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_desc}###", pageDesc)
                .Replace("###{api_file_path_n_name}###", $"@/app/apis/{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{api_file_path}###", $"@/app/apis/{menuItem.Replace("_", "-").Trim().ToLower()}")
                .Replace("###{api_code}###", apiCode)
                .Replace("###{db_file_code}###", dbCode)
                .Replace("###{css_code}###", cssCode)
                .Replace("###{existing_page_code}###", exstingPageCode)
                .Replace("###{theme_icon_prompt}###", themeIconPrompt)
                .Replace("###{theme_chart_prompt}###", themeChartPrompt);

            return prompt;
        }
    }


    


    
}
