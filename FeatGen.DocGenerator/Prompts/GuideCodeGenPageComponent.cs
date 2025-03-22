using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

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

                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to design a backend api endpoints to be called by the frontend.

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

                ## Output format

                Return the pure code only without any explaination, markdown symboles and other characters.


                """;
            var menuItemsString = rcg.MenuItems.CleanJsCodeQuote().CleanJsonCodeQuote();
            var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(menuItemsString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var pagesString = rcg.Pages.CleanJsonCodeQuote();
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var subPages = allPages.Where(p =>
                    p.related_pages.Any(p => p.page_id == pageId) &&
                    menuItems.All(m => m.page_id != p.page_id)).ToList();

            var pages = new List<GuidePageItem>() { mainPage };
            pages.AddRange(subPages);

            string pageDesc = JsonSerializer.Serialize<List<GuidePageItem>>(
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

                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to design a backend api endpoints to be called by the frontend.

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

                ## Output format

                Return the pure code only without any explaination, markdown symboles and other characters.


                """;

            var pagesString = rcg.Pages.CleanJsonCodeQuote();
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);

            var pages = new List<GuidePageItem>() { mainPage };

            string pageDesc = JsonSerializer.Serialize<List<GuidePageItem>>(
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
    }


    
}
