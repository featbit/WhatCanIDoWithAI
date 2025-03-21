using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Net;
using System.Reflection;

namespace FeatGen.OpenAI
{
    public interface IAntropicChatService
    {
        Task<string> CompleteChatAsync(string message, bool enforceJson = false);
        Task<string> CompleteChatWithJsonAsync(string message);
    }

    public class AntropicChatService : IAntropicChatService
    {
        private readonly ChatClient _chatClient;

        public AntropicChatService(IConfiguration configuration)
        {
            ApiKeyCredential credential = new ApiKeyCredential(
                configuration["Antropic:ApiKey"] ?? throw new Exception("Antropic API key is missing"));
            string endpointUrl = configuration["Antropic:EndpointUrl"] ??
                throw new Exception("Antropic endpoint URL is missing");
            string model = configuration["Antropic:Model"] ??
                throw new Exception("Antropic model is missing");
            OpenAIClientOptions options = new()
            {
                Endpoint = new Uri(endpointUrl),
                NetworkTimeout = TimeSpan.FromSeconds(600)
            };
            _chatClient = new(model: model, credential, options);
        }

        public async Task<string> CompleteChatAsync(string message, bool enforceJson = false)
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync(
            [
                new UserChatMessage(message)
            ]);

            string response = completion.Content[0].Text;

            return enforceJson ? response.CleanJsonCodeQuote() : response;
        }

        public async Task<string> CompleteChatWithJsonAsync(string message)
        {
            // Add a system message instructing the model to return JSON format
            ChatCompletion completion = await _chatClient.CompleteChatAsync(
            [
                new SystemChatMessage("You are a helpful assistant that always responds in valid JSON format. " +
                                      "Your output should be properly formatted JSON that can be parsed by a standard JSON parser."),
                new UserChatMessage(message)
            ], new ChatCompletionOptions
            {
                ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
            });

            string response = completion.Content[0].Text;
            
            // Clean JSON response to ensure it's valid
            return response.CleanJsonCodeQuote();
        }
    }
}