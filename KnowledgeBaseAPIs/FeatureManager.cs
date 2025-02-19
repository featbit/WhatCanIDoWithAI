using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;

namespace KnowledgeBaseAPIs
{
    public interface IFeatureManager
    {
        Task<bool> IsEnabledAsync(string featureName);
    }   
    public class FeatureManager: IFeatureManager
    {
        private readonly IFbClient _fbClient;
        public FeatureManager(IFbClient fbClient)
        {
            _fbClient = fbClient;
        }

        public Task<bool> IsEnabledAsync(string featureName)
        {
            return IsFeatureEnabledAsync("344", featureName, null);
        }

        private async Task<bool> IsFeatureEnabledAsync(string userId, string featureName, Dictionary<string, object> attributes)
        {
            var fbUser = FbUser.Builder(userId).Build();

            return _fbClient.BoolVariation(featureName, fbUser, defaultValue: false);
        }
    }
}
