using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatGen.ReportGenerator.Prompts
{
    public class PostSteps1CodeFixing
    {
        public static string DbCodeFixingPrompt(string fileCode, string requirementPrompt)
        {
            string prompt = """
                ## Task
                You are professional full-stack javascript, nextjs developer. Please edit the code or fix the code error in the Given File with the following requirements:

                ###{requirement_prompt}###

                Note:

                - This is a database operation file in a NextJs project, with React19, tailwind 4 and shadcn/ui.
                - The code in this database operation file is responsible for data initialization, simulation of data operations, fake data generation, and so on.
                - You should return entire code of the edited file.
                - Please return the pure code only without any explaination, table and code.
                - You should understand the requirement and analyze the code before editing.
                
                ### Given File Code:
            
                ```javascript
                ###{file_code}###
                ```
                
                ## Output format

                Please return the pure code only without any explaination, table and code.

            """;

            return prompt.Replace("###{requirement_prompt}###", requirementPrompt)
                         .Replace("###{file_code}###", fileCode);
        }
        public static string ApiCodeFixingPrompt(string fileCode, string requirementPrompt)
        {
            string prompt = """
                ## Task
                You are professional full-stack javascript, nextjs developer. Please edit the code or fix the code error in the Given File with the following requirements:

                ###{requirement_prompt}###

                Note:

                - This is a API file in a NextJs project, with React19, tailwind 4 and shadcn/ui.
                - The code in this API file is responsible for business logic control, such as create, read, update, and delete operations by calling the functions and variables defined in database file. Also front-end page code will call the APIs defined in this file.
                - You should return entire code of the edited file.
                - Please return the pure code only without any explaination, table and code.
                - You should understand the requirement and analyze the code before editing.
                
                ### Given File Code:
            
                ```javascript
                ###{file_code}###
                ```
                
                ## Output format

                Please return the pure code only without any explaination, table and code.

            """;

            return prompt.Replace("###{requirement_prompt}###", requirementPrompt)
                         .Replace("###{file_code}###", fileCode);
        }
        public static string PageCodeFixingPrompt(string fileCode, string requirementPrompt)
        {
            string prompt = """
                ## Task
                You are professional javascript, nextjs developer. Please edit the code or fix the code error in the Given File with the following requirements:

                ###{requirement_prompt}###

                Note:

                - This is a front-end page file in a NextJs project, with React19, tailwind 4 and shadcn/ui.
                - You should return entire code of the edited file.
                - Please return the pure code only without any explaination, table and code.
                - You should understand the requirement and analyze the code before editing.
                
                ### Given File Code:
            
                ```javascript
                ###{file_code}###
                ```
                
                ## Output format

                Please return the pure code only without any explaination, table and code.

            """;

            return prompt.Replace("###{requirement_prompt}###", requirementPrompt)
                         .Replace("###{file_code}###", fileCode);
        }

        public static string DecideWhichFileToModifiedPrompt(string menuItem, string moduleSpec, string dbCode, string apiCode, string pageCode, string errorInfo)
        {
            string prompt = """

                In our Next.js project, we have implemented a page with the following specifications:

                ###{module_spec}###

                We have implemented this specification with code in the following files:
                - `/src/app/db/db-###{menu_item}###.js`: This file contains all database-related code, such as data initialization, simulation of data operations, and fake data generation.
                - `/src/app/apis/###{menu_item}###.js`: This file contains all API code responsible for business logic control. It performs create, read, update, and delete operations by calling the functions and variables defined in `/src/app/db/db-###{menu_item}###.js`.
                - `/src/app/pages/###{menu_item}###/page.js`: This file contains the front-end code for the page, including layout, data display, and user interaction handling.

                Here is the code for each file:

                - DB File `/src/app/db/db-###{menu_item}###.js`:

                ```javascript
                ###{db_code}###
                ```

                - API File `/src/app/apis/###{menu_item}###.js` Code:

                ```javascript
                ###{api_code}###
                ```

                - Page File `/src/app/pages/###{menu_item}###/page.js` Code:

                ```javascript
                ###{page_code}###
                ```

                However, when compiling and debugging the Next.js project, we encountered the following errors:

                ```
                ###{error_info}###
                ```

                ## Task

                Based on the error information, please determine which file should be modified to resolve the issue. You should return the file name along with the reason why it needs modification.

                ## Output Format

                Please return the pure json content only without any explaination, table and code.

                ```json
                {
                    "files": [
                        {
                            "reason": "", // The reason why this file should be modified based on the error information
                            "file_path_name": "" // The file path
                        }
                    ],
                    "howtomodify": "" // Explain how to modify between files to maintain consistency in function calls across all files
                }
                
                ```

                """;


            return prompt.Replace("###{module_spec}###", moduleSpec)
                         .Replace("###{menu_item}###", menuItem)
                         .Replace("###{db_code}###", dbCode)
                         .Replace("###{api_code}###", apiCode)
                         .Replace("###{page_code}###", pageCode)
                         .Replace("###{error_info}###", errorInfo);
        }
    }
}
