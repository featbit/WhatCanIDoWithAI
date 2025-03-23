using FeatGen.Models.ReportGenerator;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;

namespace FeatGen.ReportGenerator
{
    public interface ISpecificationGenService
    {
        Task<List<Feature>> GenerateFeatureContentAsync(Specification spec, List<string> features, string requirement);
        Task<Specification> GetSpecificationByReportIdAsync(string id);
        Task<Feature?> GenerateFunctionalityDetailAsync(
            string softwareTitle, string serviceDescription, Feature feature, string requirement);
        Task<Definition?> GenerateDefinitionAsync(string title, string requirement);
    }

    public class SpecificationGenService(
        IOpenAiChatService openaiChatService,
        IAntropicChatService antropicChatService,
        IReportRepo reportRepo,
        IReportCodeGuideRepo rcgRepo) : ISpecificationGenService
    {
        public async Task<Specification> GetSpecificationByReportIdAsync(string id)
        {
            return await reportRepo.GetSpecificationByReportIdAsync(id) ??
                throw new Exception("Specification not found");
        }

        public async Task<Feature> GenerateFunctionalityDetailAsync(
            string softwareTitle, string serviceDescription, Feature feature, string requirement)
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
                        "module_detail_description": "", // detailed description of the functionality; add steps of how to use this functionalities if needed ; in chinese
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

                //string result = await antropicChatService.CompleteChatAsync(prompt, true);
                string result = await openaiChatService.CompleteChatAsync(prompt, true);
                ModuleDetail detail = JsonSerializer.Deserialize<ModuleDetail>(result);
                return new FeatGen.Models.ReportGenerator.Functionality
                {
                    DetailDescription = detail.DetailDescription,
                    Id = moduleId,
                    Name = detail.Name,
                    ShortDescription = functionality.ShortDescription
                };
            });

            var functionalities = (await Task.WhenAll(tasks)).ToList();

            List<FeatGen.Models.ReportGenerator.Functionality> distinctedFunctionalities = new();
            for (int i = 0; i < functionalities.Count; i++)
            {
                if (distinctedFunctionalities.All(f => f.Name != functionalities[i].Name))
                    distinctedFunctionalities.Add(functionalities[i]);
            }

            feature.Modules = distinctedFunctionalities;

            return feature;
        }


        public async Task<List<Feature>> GenerateFeatureContentAsync(Specification spec, List<string> features, string requirement)
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

                    Here's my software requirement that you need to use to ajust your feature description. Means you shouldn't generate a feature which is out of the space of the requirement. Here's my requirement:
                    
                    ###{requirement}###

                    Note: 
                    
                    - Feature Functionalities should differ from each other.
                    - Each functionality has an independent page in the software. A complete operation process shouldn't be splited into different functionalities. 
                    - Please think if the feature need a list items CRUD management functaionality. If not need, don't add this functionality.
                    - Each functionality should have independent task to do.
                    - If feature description is not good for generating functionalities, please add more details in the feature description or even rewrite it.

                    ## Output format

                    Return the result in json format without any other characters:

                    {
                        "feature_description": "", // detailed description of the feature, in chinese
                        "feature_name": "", // name of the feature with less than 100 characters, in chinese
                        "feature_functionalities": [], // list functionalities of the feature, describing functionalities with details. at least more than 100 characters
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
                    .Replace("###{feature_description}###", f)
                    .Replace("###{requirement}###", requirement);

                string result = await openaiChatService.CompleteChatAsync(prompt, true);
                //string result = await antropicChatService.CompleteChatAsync(prompt, true);
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
                Modules = ff.Functionalities.Select(f => new FeatGen.Models.ReportGenerator.Functionality
                {
                    ShortDescription = f,
                    Id = Guid.NewGuid().ToString(),
                }).ToList()
            }).ToList();
        }

        public async Task<Definition?> GenerateDefinitionAsync(string title, string requirement)
        {
            int featureNumbers = new Random().Next(5, 7);
            string rawPrompt = """
                    ## Task description

                    Please define what the Software "###{title}###" should looks like. Define ###{feature_number}### features of the Software. Login should be included as the first feature without sub functionalities.

                    Think for if system need features to manage (CRUD) users, devices, products and so on. If need, please add it as a feature or a functionality of a feature. If not, don't add this feature.

                    Each feature should responsable for an independent work.

                    Here's my requirement fo the software "###{title}###" that you need to use for the definition:

                    ###{requirement}###

                    ## Output format

                    Return the result in json format without any other characters:

                    {
                        "service_description": "", // define what the SaaS "###{title}###" should looks like, in chinese
                        "saas_features": [] // list from ###{feature_number}### features of the SaaS "###{title}###".  more than 100 characters
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
            string prompt = rawPrompt.Replace("###{title}###", title).Replace("###{feature_number}###", featureNumbers.ToString())
                .Replace("###{requirement}###", requirement);

            //string result = await antropicChatService.CompleteChatAsync(prompt, true);
            string result = await openaiChatService.CompleteChatAsync(prompt, true);

            return JsonSerializer.Deserialize<Definition>(result);
        }
    
    }
}
