﻿using FeatGen.FeatureFlag;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;

namespace FeatGen.Server.Controllers
{
    [Route("api/codeguide")]
    public class CodeGuideGenController(
        IFeatureFlagService flagService,
        ISender mediator,
        IAntropicChatService antropicChatService,
        ICodeGuideGenService codeGuideService,
        ICodeGuideFetchService codeGuideFetchService) : ControllerBase
    {

        [HttpPost("pages")]
        [RequestTimeout(600)]
        public async Task<IActionResult> Pages([FromBody] CodeGuideRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideService.GeneratePagesAsync(request.ReportId);
            return Ok(result);
        }

        [HttpGet("generated-pages/{reportId}")]
        [RequestTimeout(600)]
        public async Task<IActionResult> GetGeneratedPages(string reportId)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideFetchService.GetGeneratedPagesAsync(reportId);
            return Ok(result);
        }

        [HttpPost("menu-items")]
        [RequestTimeout(600)]
        public async Task<IActionResult> MenuItems([FromBody] CodeGuideRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideService.GenerateMenuItemsAsync(request.ReportId);
            return Ok(result);
        }

        [HttpPost("menu-items-code")]
        [RequestTimeout(600)]
        public async Task<IActionResult> MenuItemsCode([FromBody] CodeGuideRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideService.GenerateMenuItemsCodeAysnc(request.ReportId);
            return Ok(result);
        }

        [HttpGet("generated-menu-items/{reportId}")]
        [RequestTimeout(600)]
        public async Task<IActionResult> GetGeneratedMenuItems(string reportId)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideFetchService.GetGeneratedMenuItemsAsync(reportId);
            return Ok(result);
        }

        [HttpPost("project-models")]
        [RequestTimeout(600)]
        public async Task<IActionResult> ProjectModels([FromBody] CodeGuideRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideService.GenerateDataModelsAsync(request.ReportId);
            return Ok(result);
        }

        [HttpPost("fake-data-base")]
        [RequestTimeout(600)]
        public async Task<IActionResult> FakeDataBase([FromBody] CodeGuideRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideService.GenerateFakeDataBaseAsync(request.ReportId);
            return Ok(result);
        }

        [HttpPost("api-code")]
        [RequestTimeout(600)]
        public async Task<IActionResult> ApiCode([FromBody] CodeGuideCodeRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideService.GenerateApiCodeAsync(request.ReportId, request.PageId);
            return Ok(result);
        }


        [HttpPost("component-code")]
        [RequestTimeout(600)]
        public async Task<IActionResult> ComponentCode([FromBody] CodeGuideComponentCodeRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideService.GenerateComponentCodeAsync(
                request.ReportId, request.PageId, request.PageComponentName, request.ApiCode, request.CssCode);
            return Ok(result);
        }
    }

    public class CodeGuideRequest
    {
        public string ReportId { get; set; }
    }

    public class CodeGuideCodeRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
    }

    public class CodeGuideComponentCodeRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
        public string PageComponentName { get; set; }
        public string ApiCode { get; set; }
        public string CssCode { get; set; }
    }
}
