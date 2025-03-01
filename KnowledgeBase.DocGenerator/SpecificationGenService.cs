using KnowledgeBase.OpenAI;
using KnowledgeBase.SpecGenerator.Models;
using System.Text.Json;

namespace KnowledgeBase.SpecGenerator
{
    public interface ISpecificationGenService
    {
        Task<Feature?> GenerateSubFeatureDetailAsync(
            string softwareTitle, string serviceDescription, Feature feature);
        Task<Definition?> GenerateDefinitionAsync(string title);
        Task<Content?> GenerateContentAsync(string title, Definition definition);
    }

    public class SpecificationGenService(IOpenAiChatService openaiChatService) : ISpecificationGenService
    {
        public async Task<Feature> GenerateSubFeatureDetailAsync(
            string softwareTitle, string serviceDescription, Feature feature)
        {
            string rawPrompt = """
                    ## Task

                    I am writing a specification for software "###{title}###". I need you to write the detailed specification of a specific module of a feature in the software. 
                    
                    ## Information

                    ### Software description
                    ###{service_desc}###
                    
                    ### Feature description
                    ###{feature_desc}###
                    
                    ### Module short description
                    ###{module_desc}###

                    ## Output Format
                    
                    Return the result in json format without any other characters:

                    {
                        "module_detail_description": "", // detailed description of the module
                        "module_name": "" // name of the module with less than 100 characters
                    }

                    """;

            // Create tasks for each subfeature
            var tasks = feature.Modules.Select(async module =>
            {
                string moduleId = Guid.NewGuid().ToString();

                string prompt = rawPrompt
                    .Replace("###{title}###", softwareTitle)
                    .Replace("###{service_desc}###", serviceDescription)
                    .Replace("###{feature_desc}###", feature.Description)
                    .Replace("###{module_desc}###", module.ShortDescription);

                string result = await openaiChatService.CompleteChatAsync(prompt, "spec-gen-module", true);
                ModuleDetail detail = JsonSerializer.Deserialize<ModuleDetail>(result);
                return new Module
                {
                    DetailDescription = detail.DetailDescription,
                    Id = moduleId,
                    Name = detail.Name,
                    ShortDescription = module.ShortDescription
                };
            });

            feature.Modules = (await Task.WhenAll(tasks)).ToList();

            return feature;
        }

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

            string result = await openaiChatService.CompleteChatAsync(prompt, "spec-gen-content", true);

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
            string result = await openaiChatService.CompleteChatAsync(prompt, "spec-gen-definition", true);

            return JsonSerializer.Deserialize<Definition>(result);
        }
    }
}
