using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FeatGen.ReportGenerator.Prompts
{
    public class Step6t7DbAbstract
    {
        public static string ProjectModelsPrompt(Specification spec, ReportCodeGuide rcg)
        {
            string rawPrompt = """
                ## Data Information
                
                We're design a software named "###{service_name}###". ###{service_desc}###.

                We have structured pages data which listed all the pages in the software. In this pages structures, it includes details like:

                - The description of features and functionalities of each page.
                - The successor and predecessor pages of each page.
                - The page design description of each page.

                We have also structured menu items data which listed all the menu items in the software. In this menu items structure, it includes details like:

                - The pages that menu item and the page it links to.
                - The sub-menu items of each menu item if it has any.
                - The reason of why the menu item is added.
                - The reason of why the sub-menu item is added.


                ### Structured Pages Data

                ###{struct_pages}###

                ### Structured Menu Items Data

                ###{struct_menuitems}###

                ## Task 

                Please generate model data structure based on the pages, features and functionalities:

                - The model data structure should be designed based on the pages and features and functionalities of the software.
                - Some model data structure can be used in multiple pages.
                - Some model data structure can be used in only one page.
                - The model data structure should be in JSON format.
                - The model data structure should include the database schema of the model in PostgreSQL format.

                ## Output Format

                Output should return only the data structure in JSON format without any explaination, markdown symboles and other characters. 

                {
                    "models_across_pages": [
                        {
                            "model_id": "", // unique id of model
                            "model_reason": "", // why design model like that
                            "model_json": {}, // model data structure
                            "model_database_schema": "", // database table schema script of this model - postgresql
                            "used_by_pages": [] // ids of pages where this model is used.
                        }
                    ], // models can be used in multiple pages
                    "models_in_single_page": [
                        {
                            "model_id": "1", // unique id of model
                            "model_reason": "", // why design model like that
                            "model_json": {}, // model data structure
                            "model_database_schema": "", // database table schema script of this model - postgresql
                            "used_by_pages": [] // ids of pages where this model is used.
                        }
                    ] // models in across pages shouldn't be repeated here.
                }

                """;
            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{struct_pages}###", rcg.Pages)
                .Replace("###{struct_menuitems}###", rcg.MenuItems);
            return prompt;
        }

        public static string FakeDatabasePrompt(Specification spec, ReportCodeGuide rcg)
        {
            string rawPrompt = """

                We're design a software named "###{service_name}###". ###{service_desc}###.
                
                We have structured pages data which listed all the pages in the software. In this pages structures, it includes details like:
                
                - The description of features and functionalities of each page.
                - The successor and predecessor pages of each page.
                - The page design description of each page.

                ###{struct_pages}###

                We have data structures that listed all the definition of the models in the software. In models data structures, it includes details like:

                - Model data strucutre in json format. Key is the field name and value is the type of the field.
                - Which pages will use a model data structure. This represents that data logical relationship between pages and models.
                - The reason of why the model data structure is designed like that.
                - Database table schema script of that model in PostgreSQL format.
                
                ###{data_structure}###

                We have an existing database simulation file  memoryDB.js. This file includes all the models data structures and fake data for the models. The fake data is generated based on the model data structure. We have currentlu existing code below:

                ```javascript
                import { v4 as uuidv4 } from 'uuid';

                // Helper function to generate random date within a range
                const randomDate = (start, end) => {
                  return new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime())).toISOString();
                };

                // Helper function to get random item from array
                const randomItem = (array) => {
                  return array[Math.floor(Math.random() * array.length)];
                };

                // Helper function to generate random integer within a range
                const randomInt = (min, max) => {
                  return Math.floor(Math.random() * (max - min + 1)) + min;
                };

                // Generate users dynamically instead of hard-coding them
                const generateExampleItems = () => {
                  const users = [];
                  // Generate 10 random users instead of hard-coding them
                  const departments = ["IT部门", "会议服务部", "研发部", "客户服务部", "市场部", "财务部", "人力资源部", "行政部"];
                  const firstNames = ["张", "李", "王", "赵", "刘", "陈", "杨", "黄", "周", "吴", "郑", "马", "朱", "胡", "林"];
                  const lastNames = ["小", "大", "明", "华", "强", "平", "勇", "军", "杰", "飞", "涛", "鹏", "伟", "峰"];

                  // Add the 4 predefined users
                  const generatedUsers = [];

                  for (let i = 0; i < 10; i++) {
                    const firstName = randomItem(firstNames);
                    const lastName = randomItem(lastNames);
                    const username = (firstName + lastName).toLowerCase() + randomInt(100, 999);

                    generatedUsers.push({
                      id: `user-${(i + 5).toString().padStart(3, '0')}`,
                      username: username,
                      real_name: firstName + lastName,
                      department: randomItem(departments)
                    });
                  }

                  users.push(...generatedUsers);


                  return users;
                };

                // Generate login users and tokens
                const generateLoginUser = () => {
                  return {
                    id: "2b3c4d5e-6f7g-8h9i-0j1k-2l3m4n5o6p7q",
                    username: "管理员",
                    email: "admin@example.com",
                    phone: "13900139000",
                    password_hash: "$2a$10$FEBywZh8u9M0Cec/0mWep.1kXrwKeiWDba6tdKvDfEBjyePJnDT7K", // "password"
                    avatar_url: "https://example.com/avatars/admin.jpg",
                    role: "admin",
                    status: "active",
                    last_login_at: "2025-03-12T09:15:00Z",
                    created_at: "2025-03-10T07:00:00Z",
                    updated_at: "2025-03-12T09:15:00Z",

                    tokens: [],
                    user_activities: [],
                    login_attempts: [],
                    verification_codes: []
                  }
                };

                // Initialize in-memory database with generated data
                const initializeMemoryDB = () => {
                  // Generate users first since they're referenced by other models
                  const example_items = generateExampleItems();
                  const login_user = generateLoginUser();

                  return {
                    example_items,
                    login_user
                  };
                };

                // Export the initialized memory database
                export const memoryDB = initializeMemoryDB();
                ```

                ## Task

                Generate fake data for the model. The fake data should be in JSON format and should be based on the model data structure. The fake data should be generated based on the model data structure and should be in JSON format.

                Note:

                - Models have relationship with other models, the fake data should consider the consistency of the data between fake data of the models.
                - All data should be able to stored in the memoryDB.
                - memoryDB could be modified, updated based on the new model data structure.
                - The data should be generated dynamically with program instead of hard-coding them.

                ## Output Format

                Return the pure code only without any explaination, markdown symboles and other characters based on the existing code.

                """;
            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{struct_pages}###", rcg.Pages)
                .Replace("###{data_structure}###", rcg.Models);
            return prompt;
        }

        public static string DbExtractPrompt(ReportCodeGuide rcg)
        {
            string rawPrompt = """

                We have a database simulator file with following code:

                ```javascript
                ###{memory_db}###
                ```

                We need to use this file code to generate api code to CRUD with the models in the memoryDB. But the file have too much code that was for generating fake data and other things. We only need the code that is important for the api code that can understand the data structure of the models.

                Please help us to extract the information that is important for the api code only. 

                NOTE:

                - For model that has nested data structure, you should also generate the code for the nested data structure.

                ## Output Format

                Return the pure code only without any explaination, markdown symboles and other characters based on the existing code.

                ## Output Example
          
                // user data structure
                const users = [
                    {
                        id: 'user-001',
                        username: 'user001',
                        real_name: 'User 001',
                        department: 'IT'
                    },
                ];

                // simple example data structure
                const simple_examples = [
                    {   
                        id: 'example-001',
                        name: 'example 001',
                        description: 'description of example 001',
                        created_at: '2025-03-18T12:23:12',
                        updated_at: '2025-03-18T12:23:12'
                    },
                ]

                // jsonb example data structure
                const jsonb_examples = [
                    {
                        id: 'jsonb-example-001',
                        description: 'description of jsonb example 001',
                        content: {
                            key_findings: "key findings sentence",
                            charts: [
                              {
                                title: "chart 1",
                                type: "line",
                                data_points: 23
                              }
                            ],
                        },
                        created_at: '2025-03-18T12:23:12',
                        updated_at: '2025-03-18T12:23:12'
                    }
                ]

                export const memoryDB = {
                    users,
                    simple_examples,
                    jsonb_examples
                }
                """;
            string prompt = rawPrompt
                .Replace("###{memory_db}###", rcg.FakeDataBase);
            return prompt;
        }
    }
}
