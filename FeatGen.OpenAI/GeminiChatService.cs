using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json; 

namespace FeatGen.OpenAI
{
    public interface IGeminiChatService
    {
        Task<string> CompleteChatAsync(string message, bool enforceJson = false);
    }

    public class GeminiChatService : IGeminiChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeminiChatService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(600 * 1000);
        }

        public async Task<string> CompleteChatAsync(string message, bool enforceJson = false)
        {
            var requestData = new
            {
                messages = new List<dynamic>() {
                    new {
                        role = "user",
                        content = message
                    }
                }
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(requestData),
                System.Text.Encoding.UTF8,
                "application/json");

            try
            {
                var response = await _httpClient.PostAsync(_configuration["Gemini:EndpointUrl"], content);
                //var requestTimeout = TimeSpan.FromSeconds(600); // 10 minute timeout
                //using var cts = new CancellationTokenSource(requestTimeout);
                //var response = await _httpClient.PostAsync(_configuration["Gemini:EndpointUrl"], content, cts.Token);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                    //return JsonConvert.DeserializeObject<GeminiResponse>(responseContent);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch(Exception exp)
            {
                throw new Exception($"Error: {exp.Message}");
            }


        }
    }

    public class GeminiResponse
    {
        [JsonPropertyName("success")]
        public string Success { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("token_used")]
        public GeminiResponseTokenUsed TokenUsed { get; set; }
    }

    public class GeminiResponseTokenUsed
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }
}