using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;
using System;
using System.Reflection.Emit;

namespace FeatGen.ReportGenerator.Prompts
{
    public class Step9PageDb
    {
        public static string ApiDbInterfacesPrompt(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string sharedMemoryDBCode)
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

                - "###{db_shared_file_path_n_name}###" file:

                ```javascript
                ###{memorydb_code}###
                ```

                - "###{db_file_path_n_name}###" file has not been generated until now, because it depends on the result of this task
                               

                ## Task 

                Based on the requirement, specification and the code provided above. We need to regenerate the "###{db_file_path_n_name}###" file to let "###{api_file_path_n_name}###" file to provide a more detailed and accurate data to the front-end file "###{frontend_file_name}###" file. 

                This task's job is not to regenerate code immediately, but for better defining the data structure and the queries between API file "###{api_file_path_n_name}###" and new database file "###{db_file_path_n_name}###", means:

                - Which exportable functions or variables should be added to the "###{db_file_path_n_name}###" file?
                - What are the response and request data structures when API file "###{api_file_path_n_name}###" calls the database file "###{db_file_path_n_name}###"?
                - What data in the database file for each function and variables should be simulated?

                ## Output Format

                [
                	{
                		 "reason": "", // reason why this function or variable should be added to the database file "###{db_file_path_n_name}###" and used by API file "###{api_file_path_n_name}###"; in chinese
                		 "name": "", // function or variable name defined in "###{db_file_path_n_name}###" and will be called in "###{api_file_path_n_name}###"
                		 "interact_type": "", // function or variable to be called in the API file, should between "function" and "variable"
                		 "interact_action": "", // a value between 'create', 'read', 'update' and 'delete', 
                		 "request_params_description", "", // request params for the function or variable ; description should be in chinese
                		 "response_data_description": "", // response data and data structure for the function or variable;
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
                .Replace("###{db_file_path_n_name}###", $"@/app/db/db-{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{frontend_file_name}###", $"@/app/pages/{menuItem.Replace("_", "-").Trim().ToLower()}/page.js")
                .Replace("###{memorydb_code}###", sharedMemoryDBCode);

            return prompt;
        }

        public static string DbModelsPrompt(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string interfaceDefinition)
        {
            string rawPrompt = """

                ## Context
                
                We're developing a software named "###{service_name}###" - ###{service_desc}###.
                
                We're now developing a page:
                
                ###{page_desc}###.
                
                We have resources below:
                
                - the definition of exportable functions and variables (interface) for creating a new dedicated database file "###{db_file_path_n_name}" that "###{api_file_path_n_name}###" will communicate with:
                
                ```json
                ###{interface_definition}###
                ```

                ## Task

                We need to use the information above to generate the models and models's data structure for the new dedicated database file "###{db_file_path_n_name}###".

                NOTE:

                - For model that has nested data structure, you should also generate the code for the nested data structure.
                - You need to generate a model in json data structure, then the sql schema as a table for the model.
                - 

                ## Output Format

                Return the pure code only without any explaination, markdown symboles and other characters based on the existing code.

                ## Output Example
          
                // roles data structure
                const roles = [
                  {
                    id: "uuid",
                    name: "string",
                    description: "string",
                    permissions: "JSON string",
                    created_at: "ISO date string",
                    updated_at: "ISO date string"
                  }
                ];

                // sql schema for roles
                "CREATE TABLE roles (\n  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),\n  name VARCHAR(50) NOT NULL UNIQUE,\n  description TEXT,\n  permissions JSONB NOT NULL DEFAULT '{}',\n  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,\n  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP\n);"

                // units data structure
                const units = [
                  {
                    id: "uuid",
                    unit_number: "string",
                    building: "string",
                    floor: "number",
                    room: "number",
                    area: "number",
                    created_at: "ISO date string",
                    updated_at: "ISO date string"
                  }
                ];

                // users data structure
                const users = [
                  {
                    id: "uuid",
                    username: "string",
                    password: "string (hashed)",
                    name: "string",
                    phone: "string",
                    email: "string",
                    address: "string",
                    role_id: "uuid",
                    status: "string (active, inactive, locked)",
                    unit_id: "uuid or null",
                    last_login_time: "ISO date string or null",
                    login_fail_count: "number",
                    created_at: "ISO date string",
                    updated_at: "ISO date string"
                  }
                ];
                
                // sql schema for users
                "CREATE TABLE users (\n  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),\n  username VARCHAR(50) NOT NULL UNIQUE,\n  password VARCHAR(255) NOT NULL,\n  name VARCHAR(100) NOT NULL,\n  phone VARCHAR(20),\n  email VARCHAR(100),\n  address TEXT,\n  role_id UUID REFERENCES roles(id),\n  status VARCHAR(20) NOT NULL DEFAULT 'active',\n  unit_id UUID REFERENCES units(id),\n  last_login_time TIMESTAMP,\n  login_fail_count INTEGER DEFAULT 0,\n  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,\n  updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP\n);"
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
                .Replace("###{db_file_path_n_name}###", $"@/app/db/db-{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{interface_definition}###", interfaceDefinition);

            return prompt;
        }

        public static string DbCodePrompt(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string interfacesDefinition, string dedicatedDbModels)
        {
            string rawPrompt = """

                ## Context

                We're developing a software named "###{service_name}###" - ###{service_desc}###.

                We're now developing a page:
                
                ###{page_desc}###.

                We also have resources below:

                - Interfaces definition of exportable functions and variables for creating a new dedicated database file "###{db_file_path_n_name}" that "###{api_file_path_n_name}###" will communicate with.
                - Data models that will be used in dedicated database file "###{db_file_path_n_name}" and file "###{api_file_path_n_name}".

                Frontend code calls the APIs in "###{api_file_path_n_name}###" file to create, read, update and delete the data from the database and render it on the page. All data is stored (or is generated) in the database file "###{db_file_path_n_name}###" and the APIs are used to interact with the data.

                ## Content and Code of Files

                - The definition of the functions and variables interface that "###{api_file_path_n_name}###" file will call from "###{db_file_path_n_name}###" file:

                ```json
                ###{interfaces_definition}###
                ```
                
                - Data models that will be used in dedicated database file "###{db_file_path_n_name}"
              
                ```json
                ###{db_models}###
                ```
                
                ## Task 

                Based on the requirement, specification, interfaces definition and Data models provided above, please generate the code for "###{db_file_path_n_name}###" file.

                Note:

                - This "###{db_file_path_n_name}###" file should be a new file, and it should not be the same as the "###{db_shared_file_path_n_name}###" file.
                - The "###{api_file_path_n_name}###" file will also update the code to call the exportable functions that will be generated in new "###{db_file_path_n_name}###" file. but not in this task.
                - In the "###{db_file_path_n_name}###" file, we suggest to generate fake data or simulate data dynamically. It means using program to generate data and avoid hard code data.
                - Consider introducing a TEST_USER_ID constant to represent the currently logged-in user in the system. This constant should be defined in the file and used throughout the code to simulate the logged-in user. If needed, simulation data should be related to the TEST_USER_ID.
                - Generate Chinese for data, not English.
                - The simulated data and informations should be in Chinese.

                ## Output Format
                
                Return the pure code only without any explaination, markdown symboles and other characters.

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
                .Replace("###{db_file_path_n_name}###", $"@/app/db/db-{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{interfaces_definition}###", interfacesDefinition)
                .Replace("###{db_models}###", dedicatedDbModels);

            return prompt;
        }
    }    
}
