using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

namespace FeatGen.ReportGenerator.Prompts
{
    public class GuideCodeGenMenuItems
    {
        public static string V1(ReportCodeGuide rcg, string serviceName)
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
