using KnowledgeBase.FeatureFlag;
using KnowledgeBase.Server.ServiceHandlers;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBase.Server.Controllers
{
    [Route("api/spec-gen")]
    public class SpecificationGenController(IFeatureFlagService flagService, ISender mediator) : ControllerBase
    {
        [HttpPost("definition")]
        [RequestTimeout(600)]
        public async Task<IActionResult> GenDefinitionAsync([FromBody] SpecGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}
