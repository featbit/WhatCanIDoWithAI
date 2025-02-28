using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KnowledgeBase.SpecGenerator
{
    public static class Extensions
    {
        public static TBuilder AddSpecificationGenServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            builder.Services.AddScoped<ISpecificationGenService, SpecificationGenService>();
            return builder;
        }
    }
}
