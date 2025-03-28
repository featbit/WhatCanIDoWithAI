﻿using FeatGen.FeatureFlag;
using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator;
using FeatGen.Server.ServiceHandlers;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FeatGen.Server.Controllers
{
    [Route("api/reportgen")]
    public class ReportGenController(
        IFeatureFlagService flagService, 
        IReportRepo reportRepo,
        IReportCodeRepo reportCodeRepo,
        ISender mediator,
        IAntropicChatService antropicChatService) : ControllerBase
    {
        [HttpPost("specification")]
        [RequestTimeout(600)]
        public async Task<IActionResult> SpecificationGenAsync([FromBody] SpecGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("code/theme")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> CodeDefineThemeAsync([FromBody] ThemeGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.CodeFunctionatlityGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("code/menuitems")]
        [RequestTimeout(600)]
        public async Task<IActionResult> CodeMenuItemsGenAsync([FromBody] MenuItemsGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.CodeMenuItemsGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("code/feature")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> CodeFeatureGenAsync([FromBody] FeatureGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.CodeFunctionatlityGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("code/functionality")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> CodeFunctionalityGenAsync([FromBody] FunctionalityGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.CodeFunctionatlityGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }


        [HttpPost("code/login")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> CodeloginGenAsync([FromBody] LoginGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.CodeFunctionatlityGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }
        

        [HttpGet("db/menuitems-metadata/{reportId}")]
        public async Task<string> GetComponentPagesMetadataAsync(string reportId)
        {
            Specification spec = await reportRepo.GetSpecificationByReportIdAsync(reportId);
            List<object> metadata = spec.Features.Select(f =>
            {
                return new
                {
                    Id = f.MenuItem,
                    FeatureName = f.Name,
                    FunctionName = $"render{f.MenuItem.Replace("-", "").ToUpper()}Page"
                };
            }).ToList<object>();
            return JsonSerializer.Serialize(metadata);
        }

        [HttpGet("db/specification/{reportId}")]
        public async Task<Specification> GetSpecificationByReportIdAsync(string reportId)
        {
            return await reportRepo.GetSpecificationByReportIdAsync(reportId);
        }

        [HttpGet("db/reportcode/{reportId}")]
        public async Task<ReportCode> GetReportCodeByReportIdAsync(string reportId)
        {
            return await reportCodeRepo.GetReportCodeAsync(reportId);
        }

        [HttpGet("db/reports/")]
        public async Task<List<Report>> GetReportsAsync()
        {
            return await reportCodeRepo.GetReportsAsync();
        }
    }
}
