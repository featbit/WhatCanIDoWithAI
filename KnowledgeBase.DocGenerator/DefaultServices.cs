using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KnowledgeBase.SpecGenerator
{
    public static class DefaultServices
    {
        public static TBuilder AddSpecificationGenServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            builder.Services.AddScoped<ISpecificationGenService, SpecificationGenService>();
            builder.Services.AddScoped<IFrontendGenService, FrontendGenService>();
            builder.Services.AddScoped<ISpecificationGenRepo, SpecificationGenRepo>();
            return builder;
        }
    }
}