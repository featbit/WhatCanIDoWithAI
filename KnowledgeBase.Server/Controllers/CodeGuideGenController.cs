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
        public async Task<IActionResult> ProjectModels([FromBody] ProjectModelsRequest request)
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
        public async Task<IActionResult> Pages([FromBody] PagesRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideService.GeneratePagesAsync(request.ReportId);
            return Ok(result);
        }

        
    }

    public class ProjectModelsRequest
    {
        public string ReportId { get; set; }
    }
    public class PagesRequest
    {
        public string ReportId { get; set; }
    }
}
