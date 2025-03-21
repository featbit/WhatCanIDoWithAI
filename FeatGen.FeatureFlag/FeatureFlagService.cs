using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using System.Diagnostics;

namespace FeatGen.FeatureFlag
{
    public interface IFeatureFlagService
    {
        bool IsEnabled(string flagKey);
    }

    public class FeatureFlagService(IFbClient fbClient): IFeatureFlagService
    {
        /// <summary>
        /// Learning Resource:
        /// https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/
        /// https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel
        /// </summary>


        // Create an ActivitySource for FeatBit operations
        private static readonly ActivitySource _activitySource = new("FeatBit.Evaluation");

        public bool IsEnabled(string flagKey)
        {
            // Create a span for this operation
            using var activity = _activitySource.StartActivity("FeatureFlag.IsEnabled", ActivityKind.Internal);

            try
            {
                // Add attributes to the span
                activity?.SetTag("flagKey", flagKey);

                FbUser fbUser = FbUser.Builder("no-user").Build();

                // Call the actual method with the correct flag key parameter
                bool isEnabled = fbClient.BoolVariation(flagKey, fbUser, false);

                // Add the result to the span
                activity?.SetTag("result", isEnabled);
                activity?.SetStatus(ActivityStatusCode.Ok);

                return isEnabled;
            }
            catch (Exception ex)
            {
                // Record the exception in the span
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                // Use the AddEvent method to record the exception
                activity?.AddEvent(new ActivityEvent("Exception", default, new ActivityTagsCollection { { "exception", ex.ToString() } }));
                throw;
            }
        }
    }
}
