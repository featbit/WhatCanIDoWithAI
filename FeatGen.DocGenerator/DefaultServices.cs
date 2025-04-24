using FeatGen.ReportGenerator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FeatGen.ReportGenerator
{
    public static class DefaultServices
    {
        public static TBuilder AddSpecificationGenServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
        {
            builder.Services.AddScoped<ISpecificationGenService, SpecificationGenService>();
            builder.Services.AddScoped<ICodeGuideGenService, CodeGuideGenService>();
            builder.Services.AddScoped<IReportRepo, ReportRepo>();
            builder.Services.AddScoped<IReportCodeRepo, ReportCodeRepo>();
            builder.Services.AddScoped<IReportCodeGuideRepo, ReportCodeGuideRepo>();
            builder.Services.AddScoped<ICodeGuideFetchService, CodeGuideFetchService>();
            builder.Services.AddScoped<ICodeUtilsService, CodeUtilsService>();
            builder.Services.AddScoped<ICodeFixService, CodeFixService>();

            return builder;
        }
    }
}