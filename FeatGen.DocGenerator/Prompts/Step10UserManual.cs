using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

namespace FeatGen.ReportGenerator.Prompts
{
    public class Step10UserManual
    {
        /// <summary>
        /// this is because of the V1 generated too much details, maybe we can randomly generate doc between 1 and 2
        /// v2 is just reused the structured page data to generate docs
        /// </summary>
        public static string Prompt(Specification spec, ReportCodeGuide rcg, string pageId, string pageComponent)
        {
            string rawPrompt = """

                ## Context
                
                We have programmed a software named "###{service_name}###". ###{service_desc}###. 

                We've already code for each feature and a speicification. We need to write a software User Manual based on structured page specification and the existing code. The objectif is to make the specification and code to be consistence in the User Manual.

                We don't write them all together, we write User Manual page by page. 

                ## Page Specification Data
                
                ###{pages_description}###

                ## Existing code

                Front-end code:

                ```javascript
                ###{page_component}###
                ```

                ## Task

                You need to generate the content for chapter ###{page_name}### of  User Manual of ###{service_name}###

                - Heading 2 title: ###{heading_2_title}###
                - Heading 2 description: ###{heading_2_description}###

                Note:

                - You should contains one heading 2 with 0 to multiple heading 3 sections.
                - You should consider that each heading 3 section can have one or multiple screenshots of the software interface.
                - You can write steps of using a feature in the software interface, but don't write too much details for the steps.
                - You should generate in Chinese.

                ## Output format
                
                Return the pure document text only without any explaination, table and code. The text should be in Markdown format for headings. The text should be in Chinese.

                ## Output example

                ```md
                ## ###{heading_2_title}###
                
                ###{heading_2_description}###
                
                ### 关于此章节的某个 Feature 的描述
                
                ###{heading_2_f1_description}###
                
                ```

                """;

            var menuItemsString = rcg.MenuItems;
            var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(menuItemsString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var menuItem = menuItems.FirstOrDefault(p => p.page_id == pageId);

            if(menuItem == null)
            {
                var tmi = menuItems.FirstOrDefault(p => {
                    if (p.sub_menu_items == null || p.sub_menu_items.Count == 0) 
                        return false;
                    var exist = p.sub_menu_items.Any(m => m.page_id == pageId);
                    return exist;
                });

                var stmi = tmi?.sub_menu_items.FirstOrDefault(m => m.page_id == pageId);

                menuItem = new GuideMenuItem
                {
                    menu_item = stmi.menu_item,
                    menu_name = stmi.menu_name,
                    page_id = stmi.page_id,
                    reason = stmi.reason_for_sub_menu,
                };

            }

            var pagesString = rcg.Pages;
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            mainPage.page_design = "";
            var subPages = allPages.Where(p => 
                    mainPage.related_pages.Any(p => p.page_id == pageId && p.direction == "forward") &&
                    menuItems.All(m => m.page_id != p.page_id))
                .Select(p=> {
                    p.page_design = "";
                    return p;
                 }).ToList();
            var pages = new List<GuidePageItem>() { mainPage };
            pages.AddRange(subPages);

            string pagesDescription = JsonSerializer.Serialize<List<GuidePageItem>>(
                pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            string heading_2_f1_description = (mainPage.mapping_features != null &&
                mainPage.mapping_features.Count > 0) ?
                mainPage.mapping_features[0].feature_desc : "功能描述案例 - 个人信息更新功能是智慧医保平台统一门户系统参保信息管理模块中的一个重要功能，旨在让用户能够便捷地修改个人基本信息，确保其信息的准确性和时效性。用户可以通过该功能修改与医保相关的个人基本信息，如联系电话、家庭住址、电子邮箱等。该功能帮助用户随时更新信息，避免因信息错误或过时而影响医保服务的准确性。用户登录后进入‘参保信息管理’模块的个人信息更新选项进行操作。";
            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{pages_description}###", pagesDescription)
                .Replace("###{page_component}###", pageComponent)
                .Replace("###{page_name}###", mainPage.page_name)
                .Replace("###{heading_2_title}###", menuItem.menu_name)
                .Replace("###{heading_2_description}###", mainPage.page_description)
                .Replace("###{heading_2_f1_description}###", heading_2_f1_description);

            return prompt;
        }

    }
}
