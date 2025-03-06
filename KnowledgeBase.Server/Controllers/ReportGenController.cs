using KnowledgeBase.FeatureFlag;
using KnowledgeBase.Models;
using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.OpenAI;
using KnowledgeBase.ReportGenerator;
using KnowledgeBase.Server.ServiceHandlers;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KnowledgeBase.Server.Controllers
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
    }
}
