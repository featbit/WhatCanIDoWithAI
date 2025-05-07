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
using Microsoft.Extensions.Logging;

namespace FeatGen.OpenAI
{
    public interface IGeminiChatService
    {
        Task<string> CompleteChatAsync(string message, bool enforceJson = false, string customEndpoint = "gemini-25-pro-exp-03-25");
    }

    public class GeminiChatService : IGeminiChatService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiChatService> _logger;

        public GeminiChatService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<GeminiChatService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<string> CompleteChatAsync(string message, bool enforceJson = false, string customEndpoint = "gemini-25-pro-exp-03-25")
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromMinutes(10); // 10 minute timeout

            int retryNumber = 0;

            while(retryNumber < 3)
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
                    string endpoint = $"{_configuration["Gemini:EndpointUrl"]}/{customEndpoint}";
                    var response = await httpClient.PostAsync(endpoint, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        retryNumber++;
                        _logger.LogError($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception exp)
                {
                    retryNumber++;
                    _logger.LogError(exp.Message, exp);
                }
                await Task.Delay(10 * 1000);
            }
            _logger.LogError("Max retry limit reached");
            return null;
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