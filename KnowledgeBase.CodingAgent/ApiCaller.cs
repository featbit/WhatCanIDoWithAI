using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.CodingAgent
{
    public class ApiCaller
    {
        public static async Task<string> GetMenuItemsMetadataAsync(string reportId)
        {
            string baseUrl = "https://localhost:7009";
            string endpoint = $"/api/reportgen/db/menuitems-metadata/{reportId}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
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
    }


}
