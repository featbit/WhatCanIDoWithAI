using KnowledgeBase.OpenAI;
using System.Numerics;
using System.Text.Json;
using KnowledgeBase.SpecGenerator.Models;

namespace KnowledgeBase.SpecGenerator
{
    public interface ISpecificationGenService
    {
        Task<Definition?> GenerateDefinitionAsync(string title);
    }

    public class SpecificationGenService(IOpenAiChatService openaiChatService) : ISpecificationGenService
    {
        public async Task<Definition?> GenerateDefinitionAsync(string title)
        {
            string rawPrompt = """
## Task description

Please define what the SaaS "###{title}###" should looks like.

## Output format

Return the result in json format without any other characters:

{
    "service_description": "", // define what the SaaS "###{title}###" should looks like
    "saas_features": [] // list from 2 to 5 main features randomly of the SaaS "###{title}###"
}

## Output Example

{
    ""service_description"": ""An AIGC product where help people to generate content without a profession skill"",
    ""saas_features"": [
        ""prompt input module that user can input aigc command and manage them"",
        ""result showing panel to display generated result such as image, video and article"",
        ""user management module to manage user account and subscription""
    ]
}
""";
            string prompt = rawPrompt.Replace("###{title}###", title);
            string result = await openaiChatService.CompleteChatAsync(prompt, true);
            
            return JsonSerializer.Deserialize<Definition>(result);
        }
    }
}
