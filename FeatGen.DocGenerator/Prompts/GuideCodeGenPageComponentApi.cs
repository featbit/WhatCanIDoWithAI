using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

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
    }


    
}
