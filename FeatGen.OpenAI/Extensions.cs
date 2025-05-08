using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;

namespace FeatGen.OpenAI
{
    public static class Extensions
    {
        public static TBuilder AddOpenAIServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            builder.Services.AddTransient<IOpenAiChatService, OpenAiChatService>();
            builder.Services.AddScoped<IAntropicChatService, AntropicChatService>();

            builder.Services.ConfigureHttpClientDefaults(http =>
            {
                http.AddStandardResilienceHandler(o =>
                {
                    o.AttemptTimeout.Timeout = TimeSpan.FromSeconds(600);
                    o.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(1200); // Must be larger than AttemptTimeout
                    o.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(1200); //Must be at least 2 x AttemptTimeout
                });

                // Turn on service discovery by default
                //http.UseServiceDiscovery();
            });
            builder.Services.AddHttpClient("llm", c =>
            {
                c.BaseAddress = new Uri($"{builder.Configuration.GetSection("Gemini").GetSection("EndpointUrl").Value}");
                c.Timeout = TimeSpan.FromSeconds(540);
            });

            builder.Services.AddScoped<IGeminiChatService, GeminiChatService>();
            return builder;
        }
    }
}
