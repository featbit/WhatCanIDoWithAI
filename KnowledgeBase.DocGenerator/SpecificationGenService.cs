﻿using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.Models;
using KnowledgeBase.OpenAI;
using KnowledgeBase.ReportGenerator.Models;
using System.Text.Json;

namespace KnowledgeBase.ReportGenerator
{
    public interface ISpecificationGenService
    {
        Task<List<Feature>> GenerateFeatureContentAsync(Specification spec, List<string> features);
        Task<Specification> GetSpecificationByReportIdAsync(string id);
        Task<Feature?> GenerateSubFeatureDetailAsync(
            string softwareTitle, string serviceDescription, Feature feature);
        Task<Definition?> GenerateDefinitionAsync(string title);
        Task<Content?> GenerateContentAsync(string title, Definition definition);
    }

    public class SpecificationGenService(
        IOpenAiChatService openaiChatService,
        IReportRepo reportRepo) : ISpecificationGenService
    {
        public async Task<Specification> GetSpecificationByReportIdAsync(string id)
        {
            return await reportRepo.GetSpecificationByReportIdAsync(id) ??
                throw new Exception("Specification not found");
        }

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
                        "module_detail_description": "", // detailed description of the module, in chinese
                        "module_name": "" // name of the module with less than 100 characters, in chinese
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
                        "service_description": "" // define what the SaaS "###{title}###" should looks like, in chinese
                        "saas_features": [
                            {
                                "feature_description": "", // main feature of the SaaS "###{title}###", should describe details as a specification. at least more than 100 characters
                                "feature_name": "", // name of the main feature
                                "sub_features": [], // list 1,2,or 3 sub features (modules) of the main feature, describing its functionalities with details as a specification. at least more than 100 characters
                                "menu_item": "" // menu item name for the main feature
                            }
                        ] // list 3-5 main features of the SaaS "###{title}###"
                    }

                    ## Output Example
                    {
                        "service_description": "An AIGC product where help people to generate content without a profession skill", 
                        "saas_features": [
                            {
                                "feature_description": "prompt input module that user can input aigc command and manage them. here should describe more details about the feature.",
                                "feature_name": "Prompt Input Management",
                                "sub_features": [
                                    "result showing panel to display generated result such as image, video and article. here should describe more details about the feature.",
                                    "user management module to manage user account and subscription. here should describe more details about the feature."
                                ],
                                "menu_item": "Prompt Input"
                            },
                            {
                                "feature_description": "result showing panel to display generated result such as image, video and article. here should describe more details about the feature.",
                                "feature_name": "Result Management Panel",
                                "sub_features": [
                                    "prompt input module that user can input aigc command and manage them. here should describe more details about the feature.",
                                    "user management module to manage user account and subscription. here should describe more details about the feature."
                                ],
                                "menu_item": "Result Panel"
                            }
                        ]
                    }

                    """;

            string prompt = rawPrompt
                .Replace("###{title}###", title)
                .Replace("###{service_description}###", definition.ServiceDescription)
                .Replace("###{featurenumber}###", definition.SaasFeatures.Count.ToString());

            string result = await openaiChatService.CompleteChatAsync(prompt, "spec-gen-content", true);

            return JsonSerializer.Deserialize<Content>(result);
        }

        public async Task<List<Feature>> GenerateFeatureContentAsync(Specification spec, List<string> features)
        {
            string rawPrompt = """
                    ## Task

                    Write functionalities in the feature of the software.

                    ## Information

                    - Software name: ###{title}###
                    - Software description: ###{service_description}###

                    We have defined ###{featurenumber}### features:

                    """ +
                    string.Join("\n", features.Select(feature => $"- {feature}"))
                    + """

                    You need to write functionalities for feature:

                    ###{feature_description}###

                    ## Task Detail

                    For feature "###{feature_description}###", generate the following information:
                    
                    - Name of feature
                    - Detailed description of the feature.
                    - Functionality or modules of the feature.
                    - Menu item name for the feature.


                    ## Output format

                    Return the result in json format without any other characters:

                    {
                        "feature_description": "", // detailed description of the feature, in chinese
                        "feature_name": "", // name of the feature with less than 100 characters, in chinese
                        "feature_functionalities": [], // list 1,2,or 3 functionalities of the feature, describing functionalities with details. at least more than 100 characters
                        "menu_item": "" // menu item name for the feature
                    }

                    ## Output Example

                    {
                        "feature_description": "n AIGC product where help people to generate content without a profession skill. could have more characters here.", 
                        "feature_name": "Prompt Input Management",
                        "feature_functionalities": [
                            "result showing panel to display generated result such as image, video and article. here should describe more details about the feature.",
                            "user management module to manage user account and subscription. here should describe more details about the feature."
                        ],
                        "menu_item": "Prompt Input" 
                    }

                    """;


            // Create tasks for each subfeature
            var tasks = features.Select(async f =>
            {
                string prompt = rawPrompt
                    .Replace("###{title}###", spec.Title)
                    .Replace("###{service_description}###", spec.Definition)
                    .Replace("###{featurenumber}###", features.Count.ToString())
                    .Replace("###{feature_description}###", f);

                string result = await openaiChatService.CompleteChatAsync(prompt, "spec-gen-module", true);
                var detail = JsonSerializer.Deserialize<FeatureFunctionalities>(result);
                return detail;
            });

            List<FeatureFunctionalities> ffs = (await Task.WhenAll(tasks)).ToList();

            return ffs.Select(ff => new Feature
            {
                Description = ff.FeatureDescription,
                Name = ff.FeatureName,
                MenuItem = ff.MenuItem,
                Modules = ff.Functionalities.Select(f => new Module
                {
                    ShortDescription = f,
                    Id = Guid.NewGuid().ToString()
                }).ToList()
            }).ToList();
        }

        public async Task<Definition?> GenerateDefinitionAsync(string title)
        {
            string rawPrompt = """
                    ## Task description

                    Please define what the SaaS "###{title}###" should looks like.

                    ## Output format

                    Return the result in json format without any other characters:

                    {
                        "service_description": "", // define what the SaaS "###{title}###" should looks like, in chinese
                        "saas_features": [] // list from 2 to 5 main features randomly of the SaaS "###{title}###". at least more than 100 characters
                    }

                    ## Output Example

                    {
                        "service_description": "An AIGC product where help people to generate content without a profession skill", 
                        "saas_features": [
                            "prompt input module that user can input aigc command and manage them. here should describe more details about the feature.",
                            "result showing panel to display generated result such as image, video and article. here should describe more details about the feature.",
                            ""user management module to manage user account and subscription. here should describe more details about the feature."
                        ]
                    }
                    """;
            string prompt = rawPrompt.Replace("###{title}###", title);
            string result = await openaiChatService.CompleteChatAsync(prompt, "spec-gen-definition", true);

            return JsonSerializer.Deserialize<Definition>(result);
        }
    }
}
