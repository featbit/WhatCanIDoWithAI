using KnowledgeBase.OpenAI;
using System.Numerics;
using System.Text.Json;
using KnowledgeBase.SpecGenerator.Models;
using System.Linq;

namespace KnowledgeBase.SpecGenerator
{
    public interface ISpecificationGenService
    {
        Task<Definition?> GenerateDefinitionAsync(string title);
        Task<Content?> GenerateContentAsync(string title, Definition definition);
    }

    public class SpecificationGenService(IOpenAiChatService openaiChatService) : ISpecificationGenService
    {
        public async Task<Content?> GenerateContentAsync(string title, Definition definition)
        {
            string rawPrompt = """
                    ## Task

                    I am writing a specification for software "###{title}###", please help me to generate the following data:

                    1. main features of the software. maximum 4 main features.
                    2. sub features in each main feature. maximum 3 sub features for each main feature.
                    3. the menu item name for each main feature.

                    ## Information

                    ###{service_description}###

                    It has mainly ###{featurenumber}### features:

                    """ +
                    string.Join("\n", definition.SaasFeatures.Select(feature => $"- {feature}"))
                    + """

                    ## Output Format
                
                    Return the result in json format without any other characters:


                    {
                        "service_description": "" // define what the SaaS "###{title}###" should looks like
                        "saas_features": [
                            {
                                "feature": "", // main feature of the SaaS "###{title}###"
                                "sub_features": [] // list 1,2,or 3 sub features of the main feature, describing its functionalities with details as a specification
                                "menu_item": "" // menu item name for the main feature
                            }
                        ] // list 3-5 main features of the SaaS "###{title}###"
                    }

                    """;

            string prompt = rawPrompt
                .Replace("###{title}###", title)
                .Replace("###{service_description}###", definition.ServiceDescription)
                .Replace("###{featurenumber}###", definition.SaasFeatures.Count.ToString());

            string result = await openaiChatService.CompleteChatAsync(prompt, true);

            return JsonSerializer.Deserialize<Content>(result);
        }

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
