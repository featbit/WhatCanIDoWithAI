using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KnowledgeBase.ReportGenerator.Prompts
{
    public class SpecModelGenPrompts
    {
        public static string Models(Specification spec)
        {
            string rawPrompt = """

                ## Software Description

                Our SaaS named "###{service_name}###", is a service about:

                ###{service_desc}###

                This SaaS includes features and functionalities:

                ###{feature_and_functionalities}###

                ## Task

                You need to generate global models data structures for the software and its features and functionalities.

                For example, you may need a list of items for a feature X. The functionality A of feature X is listing the items in a table with filters; Functionality B of feature X is to add new item; Functionality C of feature Y is to show details. Functionality D is to update the item; And so on. So you may define models for this feature like this:

                ```js
                {
                    id: 'id-of-item-001',
                    name": 'name-of-item-001',
                    customized_property_1: 'value of customized property 1',
                    customized_property_2: 'value of customized property 2',
                    customized_property_3: 333.3
                }
                ```

                This data structure can be used in the code to represent the item in the feature X. All operations of the feature X will be based on this data structure. A feature can have multiple data structures. One single data structures can be shared by multiple features. Each functionality can also have its own data structure for its operation and data representation.

                ## Output Format

                Output should return only the data structure in JSON format without any explaination, markdown symboles and other characters. \

                [
                    {
                        model_name: '',// name of the item
                        model_description: [],//some description of model that can help in the whole project, the feature and functionalities which could be use it for what.
                        model_data_structure: {} // a json object of the data structure in string.
                    }
                ]

                ## Output Example

                [
                    {
                        model_name: "Feature Flag",
                        model_description: [
                            "model for feature flag. the data structure should contains basic feature flag properties and also a configuration of the feature flag property. property rules contains nested data structures for rules of feature flag.".
                        ],
                        model_data_structure: {
                            id: "feature-flag-1",
                            name: "feature flag 1",
                            project_id: "1nxsa-12301-2vzz-2415-1212ak1",
                            env_id: "12301-2vzz-2415-1nxsa-1nxsb",
                            is_enabled: false,
                            type: "bool",
                            values: [
                                {
                                    value_text: true,
                                    id: "true"
                                },
                                {
                                    value_text: false,
                                    id: "false"
                                }
                            ],
                            rules: [
                                {
                                    rule_name: "rule 1",
                                    rule_id: "rule-001-dxa-12",
                                    conditions: [            
                                        {
                                            property: "",
                                            operation: "user-is-in-segment",
                                            value: [ "segment-001" ]
                                        },
                                        {
                                            property: "user_id",
                                            operation: "not-included",
                                            value: [ "abe-ase", "feasbe" ]
                                        }
                                    ],
                                    return_value: [
                                        {
                                            value_id: "true",
                                            percentage: [ 0, 100]
                                        },
                                        {
                                            value_id: "false",
                                            percentage: [ 0, 0]
                                        }
                                    ]
                                }
                            ]
                        }
                    },
                    {
                        model_name: "Project",
                        model_description: "Project model data structure include name, id, created time and updated time",
                        model_data_structure: {
                            id: "1nxsa-12301-2vzz-2415-1212ak1",
                            name: "FeatGen",
                            description: "Manage feature flags of FeatGen project",
                            created_time: "2025-03-18T12:23:12",
                            updated_time: "2025-03-18T12:23:12"
                        }
                    }
                ]

                """;
            
            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{feature_and_functionalities}###", JsonSerializer.Serialize<List<Feature>>(
                            spec.Features, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) })); 
            return prompt;
        }

        public static string FakeData()
        {
            string rawPrompt = """
                ## Task
                
                Giving your context below and data structure below, please generate fake data.

                ###{models}###

                ## 

                """;
            string prompt = rawPrompt;
            return prompt;
        }
    }
}
