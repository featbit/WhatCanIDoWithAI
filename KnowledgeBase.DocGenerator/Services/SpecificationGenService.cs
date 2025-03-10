﻿using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.OpenAI;
using KnowledgeBase.ReportGenerator.Models;
using System.Text.Json;

namespace KnowledgeBase.ReportGenerator
{
    public interface ISpecificationGenService
    {
        Task<List<Feature>> GenerateFeatureContentAsync(Specification spec, List<string> features);
        Task<Specification> GetSpecificationByReportIdAsync(string id);
        Task<Feature?> GenerateFunctionalityDetailAsync(
            string softwareTitle, string serviceDescription, Feature feature);
        Task<Definition?> GenerateDefinitionAsync(string title);
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

        public async Task<Feature> GenerateFunctionalityDetailAsync(
            string softwareTitle, string serviceDescription, Feature feature)
        {
            string rawPrompt = """
                    ## Task

                    I am writing a specification and user manual for software "###{title}###". I need you to write the detailed description and user manual of a specific Functionality of a feature in the software. The detail should includes:

                    1. The description of the functionality in the feature. This should be a detailed description of the functionality in chinese.
                    2. Steps to use the functionality. In chinese. Should include at least 2 steps.
                    
                    ## Information

                    ### Software description
                    ###{service_desc}###
                    
                    ### Feature description
                    ###{feature_desc}###
                    
                    ### Functionality short description
                    ###{functionality_desc}###

                    ## Output Format
                    
                    Return the result in json format without any other characters:

                    {
                        "module_detail_description": "", // detailed description and steps to use of the functionality, in chinese
                        "module_name": "" // name of the functionality with less than 50 characters, in chinese. should not equal to the feature name; should be generated based on Functionality short description
                    }

                    """;

            // Create tasks for each subfeature
            var tasks = feature.Modules.Select(async functionality =>
            {
                string moduleId = Guid.NewGuid().ToString();

                string prompt = rawPrompt
                    .Replace("###{title}###", softwareTitle)
                    .Replace("###{service_desc}###", serviceDescription)
                    .Replace("###{feature_desc}###", feature.Description)
                    .Replace("###{functionality_desc}###", functionality.ShortDescription);

                string result = await openaiChatService.CompleteChatAsync(prompt, true);
                ModuleDetail detail = JsonSerializer.Deserialize<ModuleDetail>(result);
                return new KnowledgeBase.Models.ReportGenerator.Functionality
                {
                    DetailDescription = detail.DetailDescription,
                    Id = moduleId,
                    Name = detail.Name,
                    ShortDescription = functionality.ShortDescription
                };
            });

            var functionalities = (await Task.WhenAll(tasks)).ToList();

            List<KnowledgeBase.Models.ReportGenerator.Functionality> distinctedFunctionalities = new();
            for (int i = 0; i < functionalities.Count; i++)
            {
                if (distinctedFunctionalities.All(f => f.Name != functionalities[i].Name))
                    distinctedFunctionalities.Add(functionalities[i]);
            }

            feature.Modules = distinctedFunctionalities;

            return feature;
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
                    - Menu item code for the feature.

                    Note: Feature Functionalities should differ from each other.

                    ## Output format

                    Return the result in json format without any other characters:

                    {
                        "feature_description": "", // detailed description of the feature, in chinese
                        "feature_name": "", // name of the feature with less than 100 characters, in chinese
                        "feature_functionalities": [], // list 1,2,or 3 functionalities of the feature, describing functionalities with details. at least more than 100 characters
                        "menu_item": "" // menu item code for the feature, in english, format:  xxx or xxx-xxx in lower case
                    }

                    ## Output Example

                    {
                        "feature_description": "n AIGC product where help people to generate content without a profession skill. could have more characters here.", 
                        "feature_name": "Prompt Input Management",
                        "feature_functionalities": [
                            "result showing panel to display generated result such as image, video and article. here should describe more details about the feature.",
                            "user management module to manage user account and subscription. here should describe more details about the feature."
                        ],
                        "menu_item": "prompt-input" 
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

                string result = await openaiChatService.CompleteChatAsync(prompt, true);
                var detail = JsonSerializer.Deserialize<FeatureFunctionalities>(result);
               
                return detail;
            });

            List<FeatureFunctionalities> ffs = (await Task.WhenAll(tasks)).ToList();

            return ffs.Select(ff => new Feature
            {
                FeatureId = Guid.NewGuid().ToString(),
                Description = ff.FeatureDescription,
                Name = ff.FeatureName,
                MenuItem = ff.MenuItem,
                Modules = ff.Functionalities.Select(f => new KnowledgeBase.Models.ReportGenerator.Functionality
                {
                    ShortDescription = f,
                    Id = Guid.NewGuid().ToString(),
                }).ToList()
            }).ToList();
        }

        public async Task<Definition?> GenerateDefinitionAsync(string title)
        {
            int featureNumbers = new Random().Next(5, 7);
            string rawPrompt = """
                    ## Task description

                    Please define what the Software "###{title}###" should looks like. Define ###{feature_number}### features of the Software. Login should be included as the first feature.

                    ## Output format

                    Return the result in json format without any other characters:

                    {
                        "service_description": "", // define what the SaaS "###{title}###" should looks like, in chinese
                        "saas_features": [] // list from ###{feature_number}### features of the SaaS "###{title}###". at least more than 100 characters
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
            string prompt = rawPrompt.Replace("###{title}###", title).Replace("###{feature_number}###", featureNumbers.ToString());
            string result = await openaiChatService.CompleteChatAsync(prompt, true);

            return JsonSerializer.Deserialize<Definition>(result);
        }
    }
}
