using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;

namespace FeatGen.CodingAgent
{
    public class ApiGenCaller
    {
        private static string _baseUrl = "https://localhost:7009";

        public static async Task<Specification> Step0_SpecificationGen(string serviceName)
        {
            string endpoint = _baseUrl + $"/api/reportgen/specification";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

                    var requestData = new
                    {
                        query = serviceName,
                        step = "definition",
                        requirement = ""
                    }
                ;

                    var content = new StringContent(
                        JsonSerializer.Serialize(requestData),
                        System.Text.Encoding.UTF8,
                        "application/json");

                    HttpResponseMessage response = await client.PostAsync(endpoint, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var sc = await response.Content.ReadAsStringAsync();
                        var spec = JsonSerializer.Deserialize<Specification>(sc);
                        return spec;
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

        public static async Task<string> Step1_GuidePages(string reportId)
        {
            string endpoint = _baseUrl + $"/api/codeguide/pages";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

                    var requestData = new
                    {
                        ReportId = reportId
                    }
                ;

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

        public static async Task<string> Step4_GuideMenuItems(string reportId)
        {
            string endpoint = _baseUrl + $"/api/codeguide/menu-items";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

                    var requestData = new
                    {
                        ReportId = reportId
                    }
                ;

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

        public static async Task<string> Step5_GuideMenuItemsCode(string reportId)
        {
            string endpoint = _baseUrl + $"/api/codeguide/menu-items-code";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

                    var requestData = new
                    {
                        ReportId = reportId
                    }
                ;

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

        public static async Task<string> Step6_GenerateDataModel(string reportId)
        {
            string endpoint = _baseUrl + $"/api/codeguide/project-models";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

                    var requestData = new
                    {
                        ReportId = reportId
                    }
                ;

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

        public static async Task<string> Step7_GenerateFakeDataBase(string reportId)
        {
            string endpoint = _baseUrl + $"/api/codeguide/fake-data-base";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

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

        public static async Task<string> Step7_ExtractFakeDataBase(string reportId)
        {
            string endpoint = _baseUrl + $"/api/codeguide/fake-data-base-extract";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

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


        public static async Task<string> Step9_1_GenerateGuideAPIEndpoints(string reportId, string pageId)
        {
            string endpoint = _baseUrl + $"/api/codeguide/api-code";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

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

        public static async Task<string> Step9_2_GenerateGuidePageComponentFiles(
            string reportId, string pageId, string apiCode)
        {
            string endpoint = _baseUrl + $"/api/codeguide/page-component-files";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

                    var requestData = new
                    {
                        ReportId = reportId,
                        PageId = pageId,
                        ApiCode = apiCode
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

        public static async Task<string> Step9_3_GenerateGuidePageComponent(
            string reportId, string pageId, string pageComponentName, string apiCode, string cssCode)
        {
            string endpoint = _baseUrl + $"/api/codeguide/component-code";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

                    var requestData = new
                    {
                        ReportId = reportId,
                        PageId = pageId,
                        PageComponentName = pageComponentName,
                        ApiCode = apiCode,
                        CssCode = cssCode
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

        public static async Task<string> Step9_4_GenerateApiDbInterfaces(
            string reportId, string pageId, string menuItem, string apiCode, string memoryDbCode)
        {
            string endpoint = _baseUrl + $"/api/codeguide/page-api-db-interfaces";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

                    var requestData = new
                    {
                        ReportId = reportId,
                        PageId = pageId,
                        MenuItem = menuItem,
                        ApiCode = apiCode,
                        MemoryDbCode = memoryDbCode,
                        PageCode = ""
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

        public static async Task<string> GenerateGuideUserManualByPage(
            string reportId, string pageId, string pageComponent)
        {
            string endpoint = _baseUrl + $"/api/codeguide/user-manual-page";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

                    var requestData = new
                    {
                        ReportId = reportId,
                        PageId = pageId,
                        PageComponent = pageComponent
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

        public static async Task<string> GenerateApplicationForm(string reportId)
        {
            string endpoint = _baseUrl + $"/api/codeguide/application-form";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(600);

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
    }
}
