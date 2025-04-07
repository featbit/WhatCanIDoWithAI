using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;
using System;

namespace FeatGen.ReportGenerator.Prompts
{
    public class GuideCodeGenPageDedicatedMemoryDb
    {
        public static string GenerateInterfaces(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string apiCode, string sharedMemoryDBCode, string pageCode)
        {
            string rawPrompt = """

                ## Context

                We're developing a software named "###{service_name}###" - ###{service_desc}###.

                We're now developing a page:
                
                ###{page_desc}###.

                We've written 4 files:

                - an api file named "###{api_file_path_n_name}###" to handle the API requests and responses, the apis interact with the database.
                - a basic database file named "###{db_shared_file_path_n_name}" contains the basic queries and data structure, the database file is used to simulate the database data source.
                - a database file named "###{db_file_path_n_name}###" to simulate the database data source and basic queries dedicated to the page "###{current_page}###". This "###{db_file_path_n_name}###" is herited from the "###{db_shared_file_path_n_name}###" file and contains the basic queries and data structure. This file has currently only the basic code.
                - a front-end file named "###{frontend_file_name}###" to render the page and handle the user interactions.

                "###{frontend_file_name}###" file calls the APIs in "###{api_file_path_n_name}###" file to create, read, update and delete the data from the database and render it on the page. All data is stored (or is generated) in the database file "###{db_file_path_n_name}###" and the APIs are used to interact with the data.

                ## Content and Code of Files

                - "###{api_file_path_n_name}###" file:

                ```javascript
                ###{api_code}###
                ```

                - "###{db_shared_file_path_n_name}###" file:

                ```javascript
                ###{memorydb_code}###
                ```

                - "###{db_file_path_n_name}###" file has not been generated until now, because it depends on the result of this task
                                
                """ + 

                //- "###{frontend_file_name}###" file:
                
                //```javascript
                //###{page_code}###
                //```

                """


                ## Task 

                Based on the requirement, specification and the code provided above. We need to regenerate the "###{db_file_path_n_name}###" file to let "###{api_file_path_n_name}###" file to provide a more detailed and accurate data to the front-end file "###{frontend_file_name}###" file. 

                This task's job is not to regenerate code immediately, but for better defining the data structure and the queries between API file "###{api_file_path_n_name}###" and new database file "###{db_file_path_n_name}###", means:

                - Which exportable functions or variables should be added to the "###{db_file_path_n_name}###" file?
                - What are the response and request data structures when API file "###{api_file_path_n_name}###" calls the database file "###{db_file_path_n_name}###"?
                - What data in the database file for each function and variables should be simulated?

                ## Output Format

                [
                	{
                		 "reason": "", // reason why this function or variable should be added to the database file "###{db_file_path_n_name}###" and used by API file "###{api_file_path_n_name}###"
                		 "name": "", // function or variable name defined in "###{db_file_path_n_name}###" and will be called in "###{api_file_path_n_name}###"
                		 "interact_type": "", // function or variable to be called in the API file, should between "function" and "variable"
                		 "interact_action": "", // a value between 'create', 'read', 'update' and 'delete', 
                		 "request_params_description", "", // request params for the function or variable 
                		 "response_data_description": "", // response data and data structure for the function or variable
                		 "function_or_variable_description": "", // description of how the function is executed with input (request_params) and output (response_data)
                	}
                ]

                ## Out Example

                [
                	{
                		 "reason": "APIs文件需要从Database获得筛选后的外卖订单列表。所以需要传入相关的筛选参数，日期与时间，外面订单状态，区域。并且function应该返回一个筛选后的外面订单列表。列表中每项应该包含字段： 订单 id，限定交付时间，地址，收货人联系电话，收货人姓名，外卖的描述，外卖员送货单笔收入，外卖状态。此 API 是外卖小哥查看订单列表时所用的数据",
                		 "name": "getDeliveryOrders",
                		 "interact_type": "function", 
                		 "interact_action": "read", 
                		 "request_params", "1. 开始与结束日期和时间。 2. 外卖小哥的工作区域", 
                		 "response_data": "一个筛选后的外面订单列表，包含字段：限定交付时间，地址，收货人联系电话，收货人姓名，外卖的描述，外卖员送货单笔收入，外卖状态", 
                		 "function_or_variable_description": "在数据库文件中，应该在 getDeliveryOrders 方法内，应模拟生成交付给外卖小哥的外卖订单数据。且数据时间应在2025年1月到4月间"
                	},
                	{
                		 "reason": "APIs文件需要向Database更新一条信息，表示一个分配给外卖小哥的订单已经完成",
                		 "name": "completeAnOrderDelivery",
                		 "interact_type": "function", 
                		 "interact_action": "update", 
                		 "request_params", "订单id", 
                		 "response_data": "是否更新完成的状态", 
                		 "function_or_variable_description": "在数据库文件中，应该找到对应的订单数据，并将其标记为已完成，返回订单完成成功的状态"
                	}
                ]

                """;
            var menuItemsString = rcg.MenuItems;
            var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(menuItemsString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var pagesString = rcg.Pages;
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var subPages = allPages.Where(p =>
                    mainPage.related_pages.Any(p => p.page_id == pageId && p.direction == "forward") &&
                    menuItems.All(m => m.page_id != p.page_id)).ToList();
            var pages = new List<GuidePageItem>() { mainPage };
            pages.AddRange(subPages);
            string pageDesc = JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });


            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_desc}###", pageDesc)
                .Replace("###{api_file_path_n_name}###", $"@/app/apis/{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{db_shared_file_path_n_name}###", "@/app/db/memoryDB.js")
                .Replace("###{db_file_path_n_name}###", $"@/app/db/db_{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{frontend_file_name}###", $"@/app/pages/{menuItem.Replace("_", "-").Trim().ToLower()}/page.js")
                .Replace("###{api_code}###", apiCode)
                .Replace("###{memorydb_code}###", memoryDBCode)
                .Replace("###{page_code}###", pageCode);

            return prompt;
        }

    }    
}
