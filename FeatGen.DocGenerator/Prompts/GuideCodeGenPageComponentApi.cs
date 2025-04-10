using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;
using System.Reflection.Emit;

namespace FeatGen.ReportGenerator.Prompts
{
    public class GuideCodeGenPageComponentApi
    {
        public static string V1(Specification spec, ReportCodeGuide rcg, string pageId)
        {
            string rawPrompt = """

                ## Context

                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to design a backend api endpoints to be called by the frontend.

                In this task, we need to generate all api endpoints for one of main pages which includes sub-pages, features and functionlities.

                The APIs is mainly for CRUD operations. We have already design the data models, and also prepared some fake data for these data models. 

                ## Main Page Description

                Here is the description of the main page with its sub-pages, features and functionalities:

                ###{page_desc}###

                ## Data Models and Fake Data 

                Existing Data Models for this main page and its sub-pages are as follows:

                ###{data_structure}###

                ## Task

                You need to generate API endpoints for the main page with its sub-pages, features and functionalities:

                - The API endpoints should be designed based on the data models and fake data. 
                - You can add new model as output of API endpoints, but you shouldn't remove existing models.
                - You can generate code to simulate the CRUD operations and fake data. The name of items should named as the test data.
                - For Update and Insert operation, you need to store the data in a temporary in-memory storage.
                - These API endpoints should be called directly in NextJs components. So it's an APIs endpoints but exist in a function format.

                Note:

                - You can add new models data structure. But you shouldn't remove existing models data structure.
                - You can update existing models data structure by adding new fileds, but not removing or modifying existing fields.
                - All code will be stored in the file /apis/###{page_id}###/page.js 
                - This is an NextJs project. So the API endpoints functions needed to be abled to export that other components can call this function.
                - Write data simulation program to generate dynamically fake and simulation data instead of hard code fake and simulation data.

                ## Output Format

                - Output should return only the backend code without any explaination, markdown symboles and other characters. 

                ## Output Example

                import { v4 as uuidv4 } from 'uuid';

                // In-memory storage for our models
                const memoryDB = {
                  qa_sessions: [
                    {
                      session_id: 'sess-001',
                      user_id: 'user-001',
                      title: '关于人工智能的提问',
                      created_at: '2023-10-15T08:30:00Z',
                      updated_at: '2023-10-15T09:45:00Z',
                      status: 'active'
                    },
                  ],
                  qa_messages: [
                    {
                      message_id: 'msg-002',
                      session_id: 'sess-001',
                      content: '机器学习是人工智能的一个分支，它使计算机系统...',
                      sender_type: 'ai',
                      created_at: '2023-10-15T08:30:30Z',
                      feedback_id: 'fb-001'
                    }
                  ], // Adding this model to store messages in sessions
                  feedback: [],
                  recommendations: [],
                  user_activities: [],
                  users: [] // Mock users for reference
                };

                // QA Messages Operations
                export const addMessageToSession = async (sessionId, userId, content, type = 'question') => {
                  // some code...

                  const newMessage = {
                    id: uuidv4(),
                    session_id: sessionId,
                    user_id: userId,
                    content,
                    type,
                    created_at: now,
                    status: type === 'question' ? 'pending' : 'delivered'
                  };

                  memoryDB.qa_messages.push(newMessage);

                  // some code...

                  return newMessage;
                };

                export const getSessionMessages = (sessionId) => {
                  return memoryDB.qa_messages
                    .filter(message => message.session_id === sessionId)
                    .sort((a, b) => new Date(a.created_at) - new Date(b.created_at));
                };

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
                .Replace("###{page_id}###", mainPage.page_id)
                .Replace("###{data_structure}###", rcg.Models); 
            return prompt;
        }

        public static string V1Login(Specification spec, ReportCodeGuide rcg, string pageId)
        {
            string rawPrompt = """

                ## Context

                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to design a backend api endpoints to be called by the frontend.

                In this task, we need to generate all api endpoints for one of main pages which includes sub-pages, features and functionlities.

                The APIs is mainly for CRUD operations. We have already design the data models, and also prepared some fake data for these data models. 

                ## Main Page Description

                Here is the description of the main page with its sub-pages, features and functionalities:

                ###{page_desc}###

                ## Data Models and Fake Data 

                Existing Data Models for this main page and its sub-pages are as follows:

                ###{data_structure}###

                ## Task

                You need to generate API endpoints for the main page with its sub-pages, features and functionalities:

                - The API endpoints should be designed based on the data models and fake data. 
                - You can add new model as output of API endpoints, but you shouldn't remove existing models.
                - You can generate code to simulate the CRUD operations and fake data. The name of items should named as the test data.
                - For Update and Insert operation, you need to store the data in a temporary in-memory storage.
                - These API endpoints should be called directly in NextJs components. So it's an APIs endpoints but exist in a function format.

                Note:

                - You can add new models data structure. But you shouldn't remove existing models data structure.
                - You can update existing models data structure by adding new fileds, but not removing or modifying existing fields.
                - All code will be stored in the file /apis/###{page_id}###/page.js 
                - This is an NextJs project. So the API endpoints functions needed to be abled to export that other components can call this function.

                ## Output Format

                - Output should return only the backend code without any explaination, markdown symboles and other characters. 

                ## Output Example

                import { v4 as uuidv4 } from 'uuid';

                // In-memory storage for our models
                const memoryDB = {
                  qa_sessions: [
                    {
                      session_id: 'sess-001',
                      user_id: 'user-001',
                      title: '关于人工智能的提问',
                      created_at: '2023-10-15T08:30:00Z',
                      updated_at: '2023-10-15T09:45:00Z',
                      status: 'active'
                    },
                  ],
                  qa_messages: [
                    {
                      message_id: 'msg-002',
                      session_id: 'sess-001',
                      content: '机器学习是人工智能的一个分支，它使计算机系统...',
                      sender_type: 'ai',
                      created_at: '2023-10-15T08:30:30Z',
                      feedback_id: 'fb-001'
                    }
                  ], // Adding this model to store messages in sessions
                  feedback: [],
                  recommendations: [],
                  user_activities: [],
                  users: [] // Mock users for reference
                };

                // QA Messages Operations
                export const addMessageToSession = async (sessionId, userId, content, type = 'question') => {
                  // some code...

                  const newMessage = {
                    id: uuidv4(),
                    session_id: sessionId,
                    user_id: userId,
                    content,
                    type,
                    created_at: now,
                    status: type === 'question' ? 'pending' : 'delivered'
                  };

                  memoryDB.qa_messages.push(newMessage);

                  // some code...

                  return newMessage;
                };

                export const getSessionMessages = (sessionId) => {
                  return memoryDB.qa_messages
                    .filter(message => message.session_id === sessionId)
                    .sort((a, b) => new Date(a.created_at) - new Date(b.created_at));
                };

                """;

            var pagesString = rcg.Pages.CleanJsCodeQuote().CleanJsonCodeQuote();
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var pages = new List<GuidePageItem>() { mainPage };

            string pageDesc = JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });


            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_desc}###", pageDesc)
                .Replace("###{page_id}###", mainPage.page_id)
                .Replace("###{data_structure}###", rcg.Models);
            return prompt;
        }

        /// <summary>
        /// This is may be needed for better data consistency
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="rcg"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static string V2WithFakeData(Specification spec, ReportCodeGuide rcg, string pageId)
        {
            string rawPrompt = """

                ## Context

                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to design a backend api endpoints to be called by the frontend.

                In this task, we need to generate all api endpoints for one of main pages which includes sub-pages, features and functionlities.

                The APIs is mainly for CRUD operations. We have already design the data models, and also prepared some fake data for these data models. 

                ## Main Page Description

                Here is the description of the main page with its sub-pages, features and functionalities:

                ###{page_desc}###

                ## Data Models and Fake Data 

                Existing Data Models for this main page and its sub-pages are as follows:

                ###{data_structure}###

                Existing Fake Data for these data models are as follows:

                ###{fake_data}###

                ## Task

                You need to generate API endpoints for the main page with its sub-pages, features and functionalities:

                - The API endpoints should be designed based on the data models and fake data. 
                - You can add new model as output of API endpoints, but you shouldn't remove existing models.
                - You can generate code to simulate the CRUD operations and fake data. The name of items should named as the test data.
                - For Update and Insert operation, you need to store the data in a temporary in-memory storage.
                - These API endpoints should be called directly in NextJs components. So it's an APIs endpoints but exist in a function format.

                Note:

                - You can add new models data structure. But you shouldn't remove existing models data structure.
                - You can update existing models data structure by adding new fileds, but not removing or modifying existing fields.
                - All code will be stored in the file /apis/###{page_id}###/page.js 
                - This is an NextJs project. So the API endpoints functions needed to be abled to export that other components can call this function.

                ## Output Format

                Output should return only the backend code without any explaination, markdown symboles and other characters. 

                ## Output Example

                import { v4 as uuidv4 } from 'uuid';

                // In-memory storage for our models
                const memoryDB = {
                  qa_sessions: [],
                  qa_messages: [], // Adding this model to store messages in sessions
                  feedback: [],
                  recommendations: [],
                  user_activities: [],
                  users: [] // Mock users for reference
                };

                // QA Messages Operations
                export const addMessageToSession = async (sessionId, userId, content, type = 'question') => {
                  // some code...

                  const newMessage = {
                    id: uuidv4(),
                    session_id: sessionId,
                    user_id: userId,
                    content,
                    type,
                    created_at: now,
                    status: type === 'question' ? 'pending' : 'delivered'
                  };

                  memoryDB.qa_messages.push(newMessage);

                  // some code...

                  return newMessage;
                };

                export const getSessionMessages = (sessionId) => {
                  return memoryDB.qa_messages
                    .filter(message => message.session_id === sessionId)
                    .sort((a, b) => new Date(a.created_at) - new Date(b.created_at));
                };

                """;

            var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(rcg.MenuItems, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(rcg.Pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var subPages = allPages.Where(p =>
                    p.related_pages.Any(p => p.page_id == pageId) &&
                    menuItems.All(m => m.page_id != p.page_id)).ToList();
            var otherMainPages = allPages.Where(p =>
                    menuItems.Any(m => m.page_id == p.page_id) &&
                    p.page_id != mainPage.page_id).ToList();

            var pages = new List<GuidePageItem>() { mainPage };
            pages.AddRange(subPages);

            string pageDesc = JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });


            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_desc}###", pageDesc)
                .Replace("###{page_id}###", mainPage.page_id)
                .Replace("###{data_structure}###", rcg.Models);
            return prompt;
        }

        public static string WithMemoryDB(Specification spec, ReportCodeGuide rcg, string pageId)
        {
            string rawPrompt = """

                ## Context
                
                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to design a backend api endpoints to be called by the frontend.
                
                In this task, we need to generate all api endpoints for one of main pages which includes sub-pages, features and functionlities.
                
                The APIs is mainly for CRUD operations. We have already design the data models, and also prepared some fake data for these data models. 
                
                ## Main Page Description
                
                Here is the description of the main page with its sub-pages, features and functionalities:
                
                ###{page_desc}###
                
                ## Data Models and DataBase 
                
                The file '../db/memoryDB' is a memory storage that encapsulates the database. The file export the memoryDB object which contains data models and fake data. You can use this memoryDB object to fetch and store data in memory.
                
                Here's an extraction of '../db/memoryDB' that contains the export object memoryDB and data models and structures:
                
                ###{data_structure}###
                
                
                ## Task
                
                You need to generate API endpoints for the main page with its sub-pages, features and functionalities:
          
                
                Note:
                
                - You can add new or update existing models data structure for better input and output of API endpoints.
                - You can generate CURD code to interact with memoryDB.
                - All code will be stored in the file /apis/###{page_id}###/page.js 
                - These API endpoints should be called directly in NextJs components. So it's an APIs endpoints but exist in a function format.
                
                ## Output Format
                
                - Output should return only the backend code without any explaination, markdown symboles and other characters. 
                
                ## Output Example
                
                import { v4 as uuidv4 } from 'uuid';
                import { memoryDB } from '../db/memoryDB';
                
                // QA Messages Operations
                export const addMessageToSession = async (sessionId, userId, content, type = 'question') => {
                  // some code...
                
                  const newMessage = {
                    id: uuidv4(),
                    session_id: sessionId,
                    user_id: userId,
                    content,
                    type,
                    created_at: now,
                    status: type === 'question' ? 'pending' : 'delivered'
                  };
                
                  memoryDB.qa_messages.push(newMessage);
                
                  // some code...
                
                  return newMessage;
                };
                
                export const getSessionMessages = (sessionId) => {
                  return memoryDB.qa_messages
                    .filter(message => message.session_id === sessionId)
                    .sort((a, b) => new Date(a.created_at) - new Date(b.created_at));
                };
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
                .Replace("###{page_id}###", mainPage.page_id)
                .Replace("###{data_structure}###", rcg.ExtractDBDataStructure);
            return prompt;
        }
    
        public static string UpdateWithDedicatedDB(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string existingApiCode, string interfacesDefinition, string dedicatedDBCode)
        {
            string rawPrompt = """

                ## Context

                We're developing a software named "###{service_name}###" - ###{service_desc}###.

                We're now developing a page:
                
                ###{page_desc}###.

                We also have resources below:

                - Existing code of file "###{db_file_path_n_name}" that "###{api_file_path_n_name}###" will communicate with.
                - Existing code of file "###{api_file_path_n_name}###".
                - Interfaces definition between "###{api_file_path_n_name}###" and "###{db_file_path_n_name}###" file.

                Frontend code calls the APIs in "###{api_file_path_n_name}###" file to create, read, update and delete the data from the database and render it on the page. All data is stored (or is generated) in the database file "###{db_file_path_n_name}###" and the APIs are used to interact with the data.

                ## Content and Code of Files

                - "###{api_file_path_n_name}###" file:

                ```javascript
                ###{api_code}###
                ```

                - "###{db_file_path_n_name}###" file:

                ```json
                ###{db_file_code}###
                ```
                
                - Interfaces definition
              
                ```json
                ###{interfaces_definition}###
                ```
                
                ## Task 

                Based on the requirement, specification, existing api code, db code, interfaces definition, please update file "###{api_file_path_n_name}###" that use the new code in file "###{db_file_path_n_name}###" to call the exportable functions to provide the correct api services.

                Note:

                - This "###{api_file_path_n_name}###" file should import db file from "###{db_file_path}###" that is where the db file located;
                - If "###{db_file_path}###" lack of interfaces or data to provide correct service, "###{api_file_path_n_name}###" can add code to simulate it.
                - Try to keep the name of functions to be exported in existing file "###{api_file_path_n_name}###".
                - Please remove import from existing shared db file like `import { memoryDB } from '@/app/db/memoryDB';` Delete it and update the code by using functions in "###{db_file_path_n_name}###".
                - Write Javascript Code, not Typescript code.

                ## Output Format
                
                Return the pure code only without any explaination, markdown symboles and other characters.

                ## Output Example
                
                import { v4 as uuidv4 } from 'uuid';
                import { memoryDB } from '@/app/db/db-###{menu_item}###';
                
                // QA Messages Operations
                export const addMessageToSession = async (sessionId, userId, content, type = 'question') => {
                  // some code...
                
                  const newMessage = {
                    id: uuidv4(),
                    session_id: sessionId,
                    user_id: userId,
                    content,
                    type,
                    created_at: now,
                    status: type === 'question' ? 'pending' : 'delivered'
                  };
                
                  memoryDB.qa_messages.push(newMessage);
                
                  // some code...
                
                  return newMessage;
                };
                
                export const getSessionMessages = (sessionId) => {
                  return memoryDB.qa_messages
                    .filter(message => message.session_id === sessionId)
                    .sort((a, b) => new Date(a.created_at) - new Date(b.created_at));
                };
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
                .Replace("###{menu_item}###", $"{menuItem.Replace("_", "-").Trim().ToLower()}")
                .Replace("###{api_file_path_n_name}###", $"@/app/apis/{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{db_file_path_n_name}###", $"@/app/db/db-{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{db_file_path}###", $"@/app/db/db-{menuItem.Replace("_", "-").Trim().ToLower()}")
                .Replace("###{interfaces_definition}###", interfacesDefinition)
                .Replace("###{api_code}###", existingApiCode)
                .Replace("###{db_file_code}###", dedicatedDBCode);

            return prompt;
        }

        public static string WithMemoryDBV2(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string interfacesDefinition, string dedicatedDBCode)
        {
            string rawPrompt = """

                ## Context
                
                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to design a backend api endpoints to be called by the frontend.
                
                In this task, we need to generate all api endpoints for one of main pages which includes sub-pages, features and functionlities.
                
                The APIs is mainly for CRUD operations. We have already the code of related database and data models, and also prepared some fake data for these data models. 
                
                ## Main Page Description
                
                Here is the description of the main page with its sub-pages, features and functionalities:
                
                ###{page_desc}###
                
                ## Data Models and DataBase 
                
                The file '@/app/db/db-###{menu_item}###' is a memory storage that encapsulates the database. The file export the memoryDB object which contains data models and fake data. You can use this memoryDB object to fetch and store data in memory. Here's the file content of databse
                
                - Database file "###{db_file_path_n_name}###" code: 

                ```json
                ###{db_file_code}###
                ```
                
                - Interfaces definition between database file and api file
                
                ```json
                ###{interfaces_definition}###
                ```
                
                ## Task
                
                You need to generate API endpoints for the main page with its sub-pages, features and functionalities:
          
                Note:
                
                - You need to use endpoints exported from database file: `import {} from '@/app/db/db-###{menu_item}###'`.
                - You can add new functions in API file to simulate data or functions that doesn't exist in database file.
                - These API endpoints should be called directly in NextJs components. So it's an APIs endpoints but exist in a function format.
                - Write Javascript code, not typescript code.
                
                ## Output Format
                
                - Output should return only the backend code without any explaination, markdown symboles and other characters. 
                
                ## Output Example
                
                import { v4 as uuidv4 } from 'uuid';
                import { memoryDB } from '@/app/db/db-###{menu_item}###';
                
                // QA Messages Operations
                export const addMessageToSession = async (sessionId, userId, content, type = 'question') => {
                  // some code...
                
                  const newMessage = {
                    id: uuidv4(),
                    session_id: sessionId,
                    user_id: userId,
                    content,
                    type,
                    created_at: now,
                    status: type === 'question' ? 'pending' : 'delivered'
                  };
                
                  memoryDB.qa_messages.push(newMessage);
                
                  // some code...
                
                  return newMessage;
                };
                
                export const getSessionMessages = (sessionId) => {
                  return memoryDB.qa_messages
                    .filter(message => message.session_id === sessionId)
                    .sort((a, b) => new Date(a.created_at) - new Date(b.created_at));
                };
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
                .Replace("###{menu_item}###", $"{menuItem.Replace("_", "-").Trim().ToLower()}")
                .Replace("###{api_file_path_n_name}###", $"@/app/apis/{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{db_file_path_n_name}###", $"@/app/db/db-{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{db_file_path}###", $"@/app/db/db-{menuItem.Replace("_", "-").Trim().ToLower()}")
                .Replace("###{interfaces_definition}###", interfacesDefinition)
                .Replace("###{db_file_code}###", dedicatedDBCode);
            return prompt;
        }
    }


    
}
