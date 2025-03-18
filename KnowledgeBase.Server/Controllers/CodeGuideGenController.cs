using KnowledgeBase.FeatureFlag;
using KnowledgeBase.OpenAI;
using KnowledgeBase.ReportGenerator;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBase.Server.Controllers
{
    [Route("api/codeguide")]
    public class CodeGuideGenController(
        IFeatureFlagService flagService,
        ISender mediator,
        IAntropicChatService antropicChatService,
        ICodeGuideGenService codeGuideService) : ControllerBase
    {
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

        
    }

    public class CodeGuideRequest
    {
        public string ReportId { get; set; }
    }
}
