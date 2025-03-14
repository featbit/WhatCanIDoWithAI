using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KnowledgeBase.ReportGenerator.Prompts
{
    public class MenuItemCodePrompts
    {
        public static string VBefor20250314(Specification spec)
        {
            string rawPrompt = """
                ## Task
                
                Modify the existing code, change the menu items with feature data. Each feature corresponds to a menu item.
                
                Each menu item should has its id, label (in chinese) and icon (svg).
                    
                Return a the new code as output.
                
                ### Existing code
                
                ```js
                window.menuItems = [
                    { id: 'home', label: '控制面板', icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" /></svg>' }
                ];
                ```
                
                ### feature data 
                
                Here are features information, includes: MenuItem (id), feature name, and feature description
                
                each feature corresponds to a menu item:
                
                ###{feature_data}###
                
                ## Output Format
                    
                Return the pure code only without any explaination, markdown symboles and other characters.
                
                ## Output Example
                
                window.menuItems = [
                    { id: 'user-login', label: '登录页面', icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" /></svg>' },
                    { id: 'users-management', label: '用户管理', icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>' },
                    { id: 'calendar-schedule', label: '预约管理', icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>' }
                ];
                """;
            List<MenuItemFeature> menuItems = spec.Features.Select(f => new MenuItemFeature
            {
                MenuItem = f.MenuItem,
                Name = f.Name,
                Description = f.Description
            }).ToList();

            string prompt = rawPrompt
                .Replace("###{feature_data}###",
                        JsonSerializer.Serialize<List<MenuItemFeature>>(
                            menuItems, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) }));
            return prompt;
        }

        public static string V2After20250314(Specification spec)
        {
            var rawPrompt = """
                ## Task
                
                Modify the existing code, change the menu items with feature data. Each feature corresponds to a menu item.
                
                Each menu item should has its id, label (in chinese) and icon (svg).
                    
                Return a the new code as output.
                
                ### Existing code
                
                ```js
                window.menuItems = [
                    { 
                        id: 'home', 
                        label: '控制面板', 
                        icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" /></svg>',
                        subMenuItems: [
                            {
                                id: 'home-0',
                                label: '子功能 1',
                                icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 14l9-5-9-5-9 5 9 5z" /></svg>'
                            },
                            {
                                id: 'home-1',
                                label: '子功能 2',
                                icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 14l9-5-9-5-9 5 9 5z" /></svg>'
                            }
                        ]
                    }
                ];
                ```
                
                ### feature data 
                
                Here are features information, includes: MenuItem (id), feature name, feature description, sub functionalities of a feature
                
                each feature corresponds to a menu item, each feature has zero or multiple functionalities, each functionality corresponds to a sub menu item of a menu item:
                
                ###{feature_data}###
                
                ## Output Format
                    
                Return the pure code only without any explaination, markdown symboles and other characters.
                
                ## Output Example
                
                window.menuItems = [
                    { 
                        id: 'users-management', 
                        label: '用户管理', 
                        icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>',
                        subMenuItems: [
                            {
                                id: 'user-management-0',
                                label: '普通用户管理1',
                                icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 14l9-5-9-5-9 5 9 5z" /></svg>'
                            },
                            {
                                id: 'user-management-1',
                                label: 'VIP用户管理',
                                icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 14l9-5-9-5-9 5 9 5z" /></svg>'
                            }
                        ]
                    },
                    { 
                        id: 'calendar-schedule', 
                        label: '预约管理', 
                        icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>' ,
                        subMenuItems: [
                            {
                                id: 'calendar-schedule-0',
                                label: '我的预约',
                                icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 14l9-5-9-5-9 5 9 5z" /></svg>'
                            },
                            {
                                id: 'calendar-schedule-1',
                                label: '已完成预约',
                                icon: '<svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 14l9-5-9-5-9 5 9 5z" /></svg>'
                            }
                        ]
                    }
                ];
                """;
            List<MenuItemFeature> menuItems = spec.Features.Select(f => new MenuItemFeature
            {
                MenuItem = f.MenuItem,
                Name = f.Name,
                Description = f.Description,
                SubMenuItems = f.Modules.Select((m,i) => new SubMenuItem
                 {
                     Name = m.Name,
                     ShortDescription = m.ShortDescription,
                     MenuItem = f.MenuItem + "-" + i.ToString()
                 }).ToList()
            }).ToList();

            string prompt = rawPrompt
                .Replace("###{feature_data}###",
                        JsonSerializer.Serialize<List<MenuItemFeature>>(
                            menuItems, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) }));
            return prompt;
        }
    }
}
