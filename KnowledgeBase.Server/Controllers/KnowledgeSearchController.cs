using FeatBit.Sdk.Server;
using KnowledgeBase.Server.Models;
using KnowledgeBase.Server.ServiceHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace KnowledgeBase.Server.Controllers
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