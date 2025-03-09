using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace KnowledgeBase.OpenAI
{
    public interface IAntropicChatService
    {
        Task<string> CompleteChatAsync(string message, bool enforceJson = false);
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
            //ChatCompletionOptions options = new ChatCompletionOptions
            //{
            //    MaxOutputTokenCount = 128000
            //};
            ChatCompletion completion = await _chatClient.CompleteChatAsync(
            [
                new UserChatMessage(message)
            ]);

            string response = completion.Content[0].Text;

            return enforceJson ? response.CleanResult() : response;
        }
    }
}