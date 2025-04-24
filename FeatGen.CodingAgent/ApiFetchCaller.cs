using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System;
using System.Reflection;
using System.Text.Json;

namespace FeatGen.CodingAgent
{
    public class ApiFetchCaller
    {
        private static string _baseUrl = "http://localhost:5177";

        public static async Task<Specification> GetSpecificationAsync(string reportId, string projectName)
        {
            string endpoint = $"/api/reportgen/db/specification/{reportId}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    HttpResponseMessage response = await client.GetAsync(endpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<Specification>(json);
                    }
                    else
                    {
                        Console.WriteLine($"{projectName} Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{projectName} Exception occurred: {ex.Message}");
                return null;
            }
        }

        public static async Task<List<GuidePageItem>> GetGuideGeneratedPagesAsync(string reportId, string projectName)
        {
            string endpoint = $"/api/codeguide/generated-pages/{reportId}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    HttpResponseMessage response = await client.GetAsync(endpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<List<GuidePageItem>>(json);
                    }
                    else
                    {
                        Console.WriteLine($"{projectName} Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{projectName} Exception occurred: {ex.Message}");
                return null;
            }
        }

        public static async Task<List<GuideMenuItem>> GetGuideGeneratedMenuItemsAsync(string reportId, string projectName)
        {
            string endpoint = $"/api/codeguide/generated-menu-items/{reportId}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    HttpResponseMessage response = await client.GetAsync(endpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<List<GuideMenuItem>>(json);
                    }
                    else
                    {
                        Console.WriteLine($"{projectName} Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{projectName} Exception occurred: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GetReportIdByTitleAsync(string title)
        {
            string endpoint = $"/api/utils/reportid-by-title/{title}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    HttpResponseMessage response = await client.GetAsync(endpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        Console.WriteLine($"GetReportIdByTitleAsync Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetReportIdByTitleAsync: Exception occurred: {ex.Message}");
                return null;
            }
        }
    }
}
