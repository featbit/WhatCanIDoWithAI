using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatGen.ReportGenerator.Prompts
{
    public class PostSteps
    {
        public static string SingleFileErrorFixingPrompt(string fileCode, string requirementPrompt)
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
    }
}
