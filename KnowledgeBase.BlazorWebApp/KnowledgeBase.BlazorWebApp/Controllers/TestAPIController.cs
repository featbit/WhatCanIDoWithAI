using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBaseAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAPIController(IFbClient fbClient) : ControllerBase
    {
        [HttpGet("GetTest")]
        public IActionResult GetTest()
        {
            return Ok("GetTest2");
        }
    }
}