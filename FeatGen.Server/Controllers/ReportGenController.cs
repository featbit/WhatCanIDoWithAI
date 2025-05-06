using FeatGen.FeatureFlag;
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
        [RequestTimeout(600000)]
        public async Task<IActionResult> SpecificationGenAsync([FromBody] SpecGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }
 

        [HttpGet("db/specification/{reportId}")]
        [RequestTimeout(600000)]
        public async Task<Specification> GetSpecificationByReportIdAsync(string reportId)
        {
            return await reportRepo.GetSpecificationByReportIdAsync(reportId);
        }


        [HttpGet("db/reports/")]
        [RequestTimeout(600000)]
        public async Task<List<Report>> GetReportsAsync()
        {
            return await reportCodeRepo.GetReportsAsync();
        }
    }
}
