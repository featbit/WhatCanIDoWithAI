using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System;
using System.Reflection;
using System.Text.Json;

namespace FeatGen.CodingAgent
{
    public class ApiGenCaller
    {
        private static string _baseUrl = "https://localhost:7009";

        public static async Task<string> Step9_1_GenerateGuideAPIEndpoints(string reportId, string pageId)
        {
            string endpoint = _baseUrl + $"/api/codeguide/api-code";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);

                    var requestData = new
                    {
                        ReportId = reportId,
                        PageId = pageId
                    };

                    var content = new StringContent(
                        JsonSerializer.Serialize(requestData),
                        System.Text.Encoding.UTF8,
                        "application/json");

                    HttpResponseMessage response = await client.PostAsync(endpoint, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GenerateFunctionalityCodeAsync(
            string reportId, string featureId, string moduleId)
        {
            string endpoint = _baseUrl + $"/api/reportgen/code/functionality";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

                    dynamic requestData = new
                    {
                        ReportId = reportId,
                        FeatureId = featureId,
                        ModuleId = moduleId
                    };

                    var content = new StringContent(
                        JsonSerializer.Serialize(requestData),
                        System.Text.Encoding.UTF8,
                        "application/json");

                    HttpResponseMessage response = await client.PostAsync(endpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return null;
            }
        }
    }
}
