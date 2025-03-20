﻿using Microsoft.Extensions.DependencyInjection;
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

            builder.Services.AddScoped<ICodePromptGenService, CodePromptGenService>();

            return builder;
        }
    }
}