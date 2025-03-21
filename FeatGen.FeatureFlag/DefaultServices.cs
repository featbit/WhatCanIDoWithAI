using FeatBit.Sdk.Server.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FeatGen.FeatureFlag
{
    public static class DefaultServices
    {
        public static TBuilder AddFeatureFlagServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            builder.Services.AddFeatBit(options =>
            {
                var featBitSection = builder.Configuration.GetSection("FeatBit");
                options.EnvSecret = featBitSection["EnvSecret"];
                options.StreamingUri = new Uri(featBitSection["StreamingUri"] ?? "");
                options.EventUri = new Uri(featBitSection["EventUri"] ?? "");
                options.StartWaitTime = TimeSpan.FromSeconds(3);
            });
            builder.Services.AddScoped<IFeatureFlagService, FeatureFlagService>();
            return builder;
        }
    }
}
