using FeatBit.Sdk.Server;
using FeatGen.Server.ServiceHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FeatGen.Server.Controllers
{
    [Route("api/knwoledge-search")]
    [ApiController]
    public class KnowledgeSearchController(IFbClient fbClient, ISender mediator) : ControllerBase
    {
        [HttpPost("search")]
        public async Task<IActionResult> Search(KnowledgeSearchRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}