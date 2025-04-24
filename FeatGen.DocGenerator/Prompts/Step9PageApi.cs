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
    public class Step9PageApi
    {
        public static string ApiCodePrompt(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string interfacesDefinition, string dedicatedDBCode)
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
                - Generate Chinese for data, not English.
                
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
