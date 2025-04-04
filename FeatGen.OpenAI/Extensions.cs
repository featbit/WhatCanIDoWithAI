﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FeatGen.OpenAI
{
    public static class Extensions
    {
        public static TBuilder AddOpenAIServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            builder.Services.AddTransient<IOpenAiChatService, OpenAiChatService>();
            builder.Services.AddScoped<IAntropicChatService, AntropicChatService>();
            builder.Services.AddScoped<IGeminiChatService, GeminiChatService>();
            return builder;
        }
    }
}
