using Microsoft.AspNetCore.SignalR;

namespace KnowledgeBase.FeatureFlag
{
    public class FeatureFlagHub() : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}