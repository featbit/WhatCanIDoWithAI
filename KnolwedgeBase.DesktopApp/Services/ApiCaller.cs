using KnowledgeBase.Models.ReportGenerator;
using System.Text.Json;

namespace KnowledgeBase.CodingAgent
{
    public class ApiCaller
    {
        private static string _baseUrl = "https://localhost:7009";

        public static async Task<Specification> GetSpecificationAsync(string reportId)
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

        public static async Task<string> GenerateMenuItemsCodeAsync(string reportId)
        {
            string endpoint = _baseUrl + $"/api/reportgen/code/menuitems";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);

                    var requestData = new
                    {
                        ReportId = reportId
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

        public static async Task<string> GetMenuItemsMetadataAsync(string reportId)
        {
            string endpoint = $"/api/reportgen/db/menuitems-metadata/{reportId}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    HttpResponseMessage response = await client.GetAsync(endpoint);

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
