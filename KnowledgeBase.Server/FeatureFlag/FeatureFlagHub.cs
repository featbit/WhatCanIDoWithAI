using FeatBit.Sdk.Server;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace KnowledgeBase.Server.FeatureFlag
{
    public class FeatureFlagHub() : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
