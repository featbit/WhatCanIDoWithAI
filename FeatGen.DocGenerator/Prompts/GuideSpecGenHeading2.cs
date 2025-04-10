using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

namespace FeatGen.ReportGenerator.Prompts
{
    public class GuideSpecGenHeading2
    {
        public static string V1(Specification spec, ReportCodeGuide rcg, string pageId, string pageComponent)
        {
            string rawPrompt = """

                ## Context
                
                We have programmed a software named "###{service_name}###". ###{service_desc}###. 

                We've already code for each feature and a speicification. We need to write a software User Manual based on structured page specification and the existing code. The objectif is to make the specification and code to be consistence in the User Manual.

                We don't write them all together, we write User Manual page by page. 

                ## Page Specification Data
                
                Main Page:

                ###{main_page}###

                Sub Pages:
                
                ###{sub_pages}###

                ## Existing code

                Front-end code:

                ```javascript
                ###{page_component}###
                ```

                ## Task

                You need to generate the content for chapter ###{page_name}### of  User Manual of ###{service_name}###

                Note:

                - You should generate only one heading 2 section
                - If needed, you should generate multiple heading 3 sections. This is optional
                - You should consider that each heading 3 section can have one or multiple screenshots of the software interface.
                - You can write steps of using a feature in the software interface, but don't write too much details for the steps.

                ## Output format
                
                Return the pure document text only without any explaination, table and code. The text should be in Markdown format for headings.

                ## Output example

                ```md
                ## 用户权限管理模块
                
                用户权限管理模块提供完整的用户管理功能，包括用户创建、角色分配、权限控制等，确保不同用户根据权限访问和操作相关功能。通过该模块，管理员可以灵活地管理系统内各类用户，设定不同的权限等级，实现安全有效的权限控制。
                
                ### 用户创建与管理
                
                用户创建与管理功能是大模型调度中台系统中管理员用来管理系统内用户的关键功能。通过该功能，管理员可以在系统中添加新用户，设置用户的基本信息，如用户名、联系方式、角色等，并能够查看和修改已有用户的信息。该功能确保管理员能够有效管理所有用户，确保每个用户的权限和访问控制符合安全要求。管理员还可以为每个用户分配不同的角色和权限，以便于系统资源的合理分配和控制。 使用步骤：
                
                1. 登录系统并进入‘用户创建与管理’模块。
                2. 点击‘添加用户’按钮，填写新用户的用户名、密码等基本信息，并为其分配合适的角色和权限，最后点击‘保存’以完成创建。 
                3. 若需要编辑已有用户信息，可以在用户列表中选择目标用户，点击‘编辑’按钮，修改相关信息后点击‘保存’。

                ### 角色分配功能

                角色分配主要功能是根据不同用户的需求和职责，为其分配适当的角色，以确保用户只能访问和操作其权限范围内的功能。管理员可以为用户设置不同的角色，如管理员、普通用户等，每个角色对应不同的操作权限。通过角色分配，系统可以实现精细化的权限管理，有效保障系统的安全性与数据的机密性。
                
                ### 权限控制设置

                权限控制功能，通过该功能，管理员可以根据不同角色或单个用户的需求设定访问权限，限制用户对某些功能或数据的访问。这不仅确保了系统的安全性，防止未授权用户操作敏感数据，还提升了数据保护和管理的灵活性。管理员可以灵活分配权限，控制用户在系统中的操作范围，避免权限滥用和数据泄露。系统支持通过角色来批量配置权限，也可以对单个用户进行精确的权限调整。管理员可通过权限控制来定义用户可见的数据、可操作的功能模块等。
                ```

                """;

            var menuItemsString = rcg.MenuItems;
            var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(menuItemsString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var pagesString = rcg.Pages;
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            mainPage.page_design = "";
            string mainPageString = JsonSerializer.Serialize<GuidePageItem>(
                mainPage, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var subPages = allPages.Where(p =>
                    p.related_pages.Any(p => p.page_id == pageId) &&
                    menuItems.All(m => m.page_id != p.page_id)).ToList();
            foreach(var item in subPages)
            {
                item.page_design = "";
            }
            string subPageString = (subPages != null && subPages.Count > 0) ?
                JsonSerializer.Serialize<List<GuidePageItem>>(
                subPages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) }) :
                "";

            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{main_page}###", mainPageString)
                .Replace("###{sub_pages}###", subPageString)
                .Replace("###{page_component}###", pageComponent)
                .Replace("###{page_name}###", mainPage.page_name);
            
            return prompt;
        }

        /// <summary>
        /// this is because of the V1 generated too much details, maybe we can randomly generate doc between 1 and 2
        /// v2 is just reused the structured page data to generate docs
        /// </summary>
        public static string V2UseGeneratedPageFeatureFunctionalityDescription(Specification spec, ReportCodeGuide rcg, string pageId, string pageComponent)
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



                ## Output format
                
                Return the pure document text only without any explaination, table and code. The text should be in Markdown format for headings.

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
