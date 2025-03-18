using KnowledgeBase.Models;
using KnowledgeBase.Models.ReportGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KnowledgeBase.ReportGenerator.Prompts
{
    public class SpecPageGenPrompts
    {
        public static string PagesV1(Specification spec, string requirement)
        {
            string rawPrompt = """
                ## Software information

                - Software: ###{service_name}###. ###{service_desc}###
                - Features and functionalities of feature: 
                    ###{feature_desc}###

                ## Additional requirement

                ###{requirement}###
                
                ## Task
                
                You need to design:
                
                - What pages should be included in a software (SaaS)
                - What components should be included in a page
                - The mapping between page and features and functionalities
                - The mapping between pages and pages, the navigation structure

                If the features described in the Software information are missed, you can also add related new features.

                NOTE: Don't consider the backend services, only think about how the front-end should be designed.

                ## Output format
                
                Output should return only the data structure in JSON format without any explaination, markdown symboles and other characters. 

                [
                    {
                        "page_id": "", // id of page
                        "page_name": "", // name of page
                        "page_description": "", // name of menu item displayed in the menu bar
                        "mapping_features": [
                            {
                                "feature_name": "", // name of feature
                                "feature_desc": "", // description of feature, should contains more than  250 characters
                                "feature_id": "", // id of feature described above
                                "functionalities": [
                                    
                                ] // list of functionalities with detailed description. Each functionality should contains more than  250 characters
                            }
                        ],
                        "related_pages": [
                            {
                                "page_id": "", // related page id
                                "direction": "", // forward or backward. forward means from this page to related page, backward means from related page to this page
                            }
                        ],
                        "page_design": "" // description of page design: 1. what elements or components should be included in the page; 2. Where elements and components should be positioned where in the page; 3. What actions should be triggered by user interactions; more than 250 characters
                    }
                ]

                ## Output Example

                [
                    {
                        "page_id": "history-management", 
                        "page_name": "Chat History",
                        "page_description": "in a chatbot system, there should be a page to list all chat dialog history. by click on one row, should be navigated to that specific chat dialog to see existing dialog. this page should also be able to add new dialog to do chat with AI assistant", 
                        "mapping_features": [
                            {
                                "feature_name": "历史记录管理模块",
                                "feature_desc": "历史记录管理模块允许用户查看和管理他们之前的问答记录。用户可以根据日期、关键词等条件查询历史记录，查看以前的问答内容，还可以删除不需要的记录，帮助用户高效地回顾和整理历史信息", 
                                "feature_id": "xxx-xx112-xxxsd",
                                "functionalities": [
                                    "查询历史记录功能：用户可以根据关键词、日期范围、问题类型等条件筛选和查询之前的问答记录，方便快速定位到需要的信息。点击历史项后，可以进入具体的聊天对话页面，查看具体历史记录和继续聊天",
                                    "删除历史记录功能：用户可以选择性地删除不再需要的历史记录，以便清理系统中的信息，确保数据管理的方便性和隐私安全。",
                                    "创建新的聊天对话：点击新对话按钮后，打开或跳转至QA智能问答页面"
                                ]
                            }
                        ],
                        "related_pages": [
                            {
                                "page_id": "intelligent-qa", 
                                "direction": "forward"
                            }
                        ],
                        "page_design": "一个列表展示所有历史记录；一个按钮新增对话；对历史记录，应该可以点击查询跳转到具体聊天界面，可以删除聊天记录，应该展示一些内容。"
                    },
                    {
                        "page_id": "intelligent-qa", 
                        "page_name": "Chat Dialog",
                        "page_description": "the page to chat with AI for asking questions and get feedbacks. It should be multiple turn round talks. if this is a existing dialog, should load talk history", 
                        "mapping_features": [
                            {
                                "feature_name": "智能问答",
                                "feature_desc": "模块允许用户查看和管理他们之前的问答记录。用户可以根据日期、关键词等条件查询历史记录，查看以前的问答内容，还可以删除不需要的记录，帮助用户高效地回顾和整理历史信息", // description of feature
                                "feature_id": "xxx-xx112-xxxsd",
                                "functionalities": [
                                    "用户输入问题后，系统通过自然语言处理技术，快速生成并展示准确答案。系统支持多领域问题的回答，确保用户能及时获得所需信息。",
                                    "领域知识拓展模块：根据用户提问的领域，系统动态拓展知识库，自动更新各类专业领域的知识，确保涵盖最新的学术、科技和行业信息"
                                ]
                            }
                        ],
                        "related_pages": [
                            {
                                "page_id": "history-management",
                                "direction": "backward"
                            }
                        ],
                        "page_design": "这个界面应该有与AI相关的对话内容；应该有输入的地方；等等其他 AI 问答机器人应该有的部分。并且 fake 一些对话数据。"
                    }
                ]

                """;

            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{feature_desc}###", JsonSerializer.Serialize<List<Feature>>(
                            spec.Features, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) }))
                .Replace("###{requirement}###", requirement);
            return prompt;
        }

        public static string MenuItems(Specification spec, ReportCodeGuide reportCodeGuide)
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
                        "reason": "", // reason of adding this menu item
                        "menu_item": "", // url path of the menu item
                        "menu_name": "", // name will be displayed in the menu bar 
                        "page_id": "", // the page id that this menu item will navigate to
                        "reason_for_sub_menu": "", // reason of adding sub menu items, or why sub_menu_items is empty
                        "sub_menu_items": [
                            {
                                "menu_item": "", // url path of the sub menu item
                                "menu_name": "", // name will be displayed in the sub menu bar
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
    }
}
