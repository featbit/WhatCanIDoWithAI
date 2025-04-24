using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

namespace FeatGen.ReportGenerator.Prompts
{
    public class Step4n5MenuItems
    {
        public static string GuidePrompt(Specification spec, ReportCodeGuide reportCodeGuide)
        {
            string rawPrompt = """
                ## Software information

                - Software: ###{service_name}###. ###{service_desc}###
                - Features and functionalities of feature: 
                    ###{feature_desc}###

                ## Pages definition

                Based on the software information, we have pages below to show the features and functionalities:

                ###{pages}###
                
                ## Task
                
                Based on the software information and pages definition, we need to extract the menu items that will render in the menu bar of the software.

                Note:

                - Not every page will be shown in the menu bar. Some pages are hidden and can be accessed by other pages.
                - The menu items should be in a tree structure. Some menu items have sub menu items.

                ## Output Format
                
                Output should return only the data structure in JSON format without any explaination, markdown symboles and other characters. 

                [
                    {
                        "reason": "", // reason of adding this menu item, in chinese
                        "menu_item": "", // url path of the menu item
                        "menu_name": "", // name will be displayed in the menu bar, in chinese
                        "page_id": "", // the page id that this menu item will navigate to
                        "reason_for_sub_menu": "", // reason of adding sub menu items, or why sub_menu_items is empty, in chinese
                        "sub_menu_items": [
                            {
                                "menu_item": "", // url path of the sub menu item
                                "menu_name": "", // name will be displayed in the sub menu bar, in chinese
                                "page_id": "", // the page id that this sub menu item will navigate to
                            }
                        ], // sub menu items could be empty
                    }
                ]

                ### Output Example

                [
                    {
                        "reason": "we need a page to manage feature flag.",
                        "menu_item": "flag-list",
                        "menu_name": "Feature Flags",
                        "page_id": "flag-list", 
                        "reason_for_sub_menu": "The configuration of feature flag should be navigated through list but shouldn't be a sub menu item. so sub menu should be empty",
                        "sub_menu_items": []
                    },
                    {
                        "reason": "iam is an independant module so needed to be displayed in menu bar. "
                        "menu_item": "iam",
                        "menu_name": "IAM",
                        "page_id": "iam", 
                        "reason_for_sub_menu": "Teams, Groups and Policies should be sub menus because each has an individual configuration task. So it shouldn't be empty",
                        "sub_menu_items": [
                            {
                                "menu_item": "teams",
                                "menu_name": "Teams", 
                                "page_id": "iam-teams"
                            },
                            {
                                "menu_item": "groups",
                                "menu_name": "Groups", 
                                "page_id": "iam-groups"
                            },
                            {
                                "menu_item": "policies",
                                "menu_name": "Policies", 
                                "page_id": "iam-policies"
                            }
                        ]
                    }
                ]

                """;

            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{feature_desc}###", JsonSerializer.Serialize<List<Feature>>(
                            spec.Features, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) }))
                .Replace("###{pages}###", reportCodeGuide.Pages);
            return prompt;
        }

        public static string CodePrompt(ReportCodeGuide rcg, string serviceName)
        {
            string rawPrompt = """

                ## Context

                This is a nextjs project. File at app/components/MenuItems.js contains the menu items configuration that will be used in Navigation components. We need to update this file's code with new menu items data from the new requirement.

                ## Menu items data

                ###{menu_items}###

                ## Existing code

                export const menuItems = [
                    {
                        menu_item: "dashboard",
                        menu_name: "首页",
                        path: "/",
                        icon: (
                            <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                                <path d="M10.707 2.293a1 1 0 00-1.414 0l-7 7a1 1 0 001.414 1.414L4 10.414V17a1 1 0 001 1h2a1 1 0 001-1v-2a1 1 0 011-1h2a1 1 0 011 1v2a1 1 0 001 1h2a1 1 0 001-1v-6.586l.293.293a1 1 0 001.414-1.414l-7-7z" />
                            </svg>
                        ),
                    },
                    {
                        menu_item: "user",
                        menu_name: "用户中心",
                        path: "/user-settings",
                        icon: (
                            <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                                <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clipRule="evenodd" />
                            </svg>
                        ),
                        sub_menu_items: [
                            {
                                menu_item: "user-settings",
                                menu_name: "个人设置",
                                path: "/user-settings",
                            },
                            {
                                menu_item: "feedback-management",
                                menu_name: "反馈管理",
                                path: "/feedback-management",
                            }
                        ]
                    },
                ];

                ## Task

                Update "app/components/MenuItems.js" file's existing code with new Menu items data. The svg icon style should be close to project name "###{service_name}###"

                Note:

                - You can replace the existing items in the existing code.
                - Please don't change the name of exporting variable "export const menuItems".
                - Svg Icon style should be consistence with "###{service_name}###"
                - The path value should be the value of menu_item with prefix symbol /. For example, if menu_item equals to 
                - Menu_name should be in Chinese

                ## Output format

                Return the pure code only without any explaination, markdown symboles and other characters.

                """;


            string prompt = rawPrompt
                .Replace("###{service_name}###", serviceName)
                .Replace("###{menu_items}###", rcg.MenuItems); 
            return prompt;
        }

    }
}
