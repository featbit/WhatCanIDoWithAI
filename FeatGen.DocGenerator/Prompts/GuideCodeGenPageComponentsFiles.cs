using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

namespace FeatGen.ReportGenerator.Prompts
{
    public class GuideCodeGenPageComponentsFiles
    {
        public static string V1(
            Specification spec, ReportCodeGuide rcg, string pageId, string apiCode)
        {
            string rawPrompt = """

                ## Context

                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to generate the next js page components code based on the information:

                - Main Page description
                - Description of features and functionalities of the page
                - Description of inner pages or components in the page
                - APIs (or functions) that the page can use for data exchange
                - MemoryDB models data structure

                ## Data and Preparation

                Here's the Page, Inner Pages/Components, Features and Functionalities:

                ###{page_features}###

                Here's API endpoints, functions and the fake data that the page can use for data exchange. This API endpoints are coded in the file `/app/apis/###{menu_item}###.js`:

                ```javascript
                ###{api_endpoints}###
                ```

                Here's the data models of the entire project which contains the data structure to be used in this feature.

                ```javascript
                ###{extracted_models}###
                ```

                ## Task

                Based on the information above, you need to decouple the business logic in seperate components that are used in main page. 

                Note:

                - Each logic independant component should be seperated in a individual javascript file.
                - Please keep data consistency between main page and comopoents, and also component and component.

                So you need to generate, main page description, components descriptions, navigation logic between components and main page, the name of files for main page and components.

                ## Output format 
                
                Return the pure json code only without any explaination, markdown symboles and other characters.

                {
                	"main_page_description": {
                		"id": "main_page"
                		"role": "", // description the role of the main page 
                		"features_n_functionalities_description": [

                		], // the features and functionalities the main page should display and behave. Each element in the array should have more than 150 characters.
                		"design": "" // how the main page should be designed, the layout of each information, components and features
                		"behaviors_direction": [
                			{
                				"component_id": "", // the id of component in components array
                				"direction": "forward", // the direction of behavior between main page and component
                                "action": "", // the action from main to component, should be selected from the list of actions: - "nested", means component is rendered inside the main page; - "popup-modal", means a modal is show a component in a popup modal; - "close-modal", means to close or hide the modal of component; - or any others can be described in a short term.
                				"reason": "" // the logic of the behavior direction
                			}
                		] // the behaviors and navigate direction between main logic and sub pages, components.
                	}, // the description of the roles, features, actions, behaviors and designs 
                	"components": [
                		{
                			"component_name": "", // simple text of component name, should be unique; should contains only alphabeta and espace
                			"component_id": "", // the id of component. should be unique and format like xxx_xxx based on component_name
                			"component_file_name": "", // the file name of component, should be like  {component_id}.js without espace
                			"reason": "", // the logic of the behavior 
                		    "role": "", // description the role of the component
                			"features_n_functionalities_description": "" // the description of what feature, functionality and behavior should this component have
                			"behaviors_direction": [
                				{
                					"component_id": "", // the id of component or main page that his component can be navigated to
                			        "direction": "", // the direction of behavior between components, the value should be selected between 'forward' and 'backward'
                                    "action": "", // the action from component to component, should be selected from the list of actions: - "child", means component is the child component; - "father": means the component is the father component; - "popup-modal", means a modal is show a component in a popup modal; - "close-modal", means to close or hide the modal of component; - or any others can be described in a short term.
                					"reason": "" // the logic of the behavior direction
                				}
                			] 
                		}
                	] // the component descriptions, the maximum numbers of components is 5
                }


                """;
            var menuItemsString = rcg.MenuItems.CleanJsCodeQuote().CleanJsonCodeQuote();
            var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(menuItemsString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var pagesString = rcg.Pages.CleanJsonCodeQuote();
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var subPages = allPages.Where(p =>
                    p.related_pages.Any(p => p.page_id == pageId) &&
                    menuItems.All(m => m.page_id != p.page_id)).ToList();

            var menuItem = menuItems.FirstOrDefault(p => p.page_id == pageId);

            var pages = new List<GuidePageItem>() { mainPage };
            pages.AddRange(subPages);

            string pageDesc = JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            string pageComponentName = menuItem.menu_item.Replace("-", "").Replace("_", "").Replace(" ", "").ToUpperInvariant();
            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_features}###", pageDesc)
                .Replace("###{api_endpoints}###", apiCode)
                .Replace("###{extracted_models}###", rcg.ExtractDBDataStructure)
                .Replace("###{menu_item}###", menuItem.menu_item);
            return prompt;
        }


    }


}
