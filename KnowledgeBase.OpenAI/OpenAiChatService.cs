using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace KnowledgeBase.OpenAI
{
    public interface IOpenAiChatService
    {
        Task<string> CompleteChatAsync(string message, bool enforceJson = false);
    }

    public class OpenAiChatService : IOpenAiChatService
    {
        private readonly ChatClient _chatClient;

        public OpenAiChatService(IConfiguration configuration)
        {
            ApiKeyCredential credential = new ApiKeyCredential(
                configuration["OpenAI:ApiKey"] ?? throw new Exception("OpenAI API key is missing"));
            string endpointUrl = configuration["OpenAI:EndpointUrl"] ??
                throw new Exception("OpenAI endpoint URL is missing");
            OpenAIClientOptions options = new()
            {
                Endpoint = new Uri(endpointUrl),
                NetworkTimeout = TimeSpan.FromSeconds(60)
            };
            _chatClient = new(model: "o3-mini", credential, options);
        }

        public async Task<string> CompleteChatAsync(string message, bool enforceJson = false)
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync([
                new SystemChatMessage(message)
            ]);
            string response = completion.Content[0].Text;
            if (enforceJson == true)
                return response.CleanResult();
            return response;
        }
    }
}
