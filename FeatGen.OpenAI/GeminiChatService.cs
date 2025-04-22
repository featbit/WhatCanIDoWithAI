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
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiChatService> _logger;

        public GeminiChatService(IConfiguration configuration, HttpClient httpClient, ILogger<GeminiChatService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(600 * 1000);
        }

        public async Task<string> CompleteChatAsync(string message, bool enforceJson = false, string customEndpoint = "gemini-25-pro-exp-03-25")
        {
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
                    var response = await _httpClient.PostAsync(endpoint, content);
                    //var requestTimeout = TimeSpan.FromSeconds(600); // 10 minute timeout
                    //using var cts = new CancellationTokenSource(requestTimeout);
                    //var response = await _httpClient.PostAsync(_configuration["Gemini:EndpointUrl"], content, cts.Token);
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
            throw new Exception("Max retry limit reached");
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