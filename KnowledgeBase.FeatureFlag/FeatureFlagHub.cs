using Microsoft.AspNetCore.SignalR;

namespace FeatGen.FeatureFlag
{
    public class FeatureFlagHub() : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}