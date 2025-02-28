using FeatBit.Sdk.Server;
using KnowledgeBase.Server.ServiceHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBase.Server.Controllers
{
    [Route("api/spec-gen")]
    public class SpecificationGenController(IFbClient fbClient, ISender mediator) : ControllerBase
    {
        [HttpPost("definition")]
        public async Task<IActionResult> GenDefinitionAsync([FromBody]SpecGenRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}
