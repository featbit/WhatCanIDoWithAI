using KnowledgeBase.FeatureFlag;
using KnowledgeBase.Models.ReportGenerator;
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
        ISender mediator) : ControllerBase
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
        [RequestTimeout(600)]
        public async Task<IActionResult> CodeFunctionalityGenAsync([FromBody] FunctionalityGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.CodeFunctionatlityGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("code/claudsonnet")]
        [RequestTimeout(600)]
        public async Task<IActionResult> ClaudeSonnet([FromBody] FunctionalityGenRequest request)
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
    }
}
