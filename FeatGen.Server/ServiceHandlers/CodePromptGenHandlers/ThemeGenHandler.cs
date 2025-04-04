﻿using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator;
using MediatR;
using System.Text.Json;

namespace FeatGen.Server.ServiceHandlers
{
    public class ThemeGenRequest : IRequest<string>
    {
        public string ReportId { get; set; }
    }

    public class ThemeGenHandler(
        ICodePromptGenService codePromptGenService,
        IReportRepo reportRepo,
        IReportCodeRepo reportCodeRepo) : IRequestHandler<ThemeGenRequest, string>
    {
        public async Task<string> Handle(ThemeGenRequest request, CancellationToken cancellationToken)
        {
            Specification spec = await reportRepo.GetSpecificationByReportIdAsync(request.ReportId) ??
                 throw new Exception("Failed to get specification");

            var code = await codePromptGenService.ThemeGenAsync(spec);

            var codeTheme = JsonSerializer.Deserialize<CodeTheme>(code.CleanResult());

            await reportCodeRepo.UpsertThemeCodeAsync(codeTheme, request.ReportId);

            return code;
        }
    }
}