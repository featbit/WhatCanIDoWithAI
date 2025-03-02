using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KnowledgeBase.ReportGenerator
{
    public static class DefaultServices
    {
        public static TBuilder AddSpecificationGenServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            builder.Services.AddScoped<ISpecificationGenService, SpecificationGenService>();
            builder.Services.AddScoped<IReportRepo, ReportRepo>();

            builder.Services.AddScoped<ICodePromptGenService, CodePromptGenService>();

            return builder;
        }
    }
}