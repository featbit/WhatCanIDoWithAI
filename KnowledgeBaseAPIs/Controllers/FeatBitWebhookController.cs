using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using KnowledgeBaseAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text.Json;

namespace KnowledgeBaseAPIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FeatBitWebhookController(IFbClient fbClient) : ControllerBase
    {
        [HttpPost("GetReport")]
        public IActionResult GetReport([FromBody] ClientParam client)
        {
            return Ok("Good");
        }

        [HttpPost("WebhookTest")]
        public IActionResult WebhookTest([FromBody] ExpandoObject body)
        {
            string bodyJson = JsonSerializer.Serialize(body);
            return Ok(bodyJson);
        }
    }
}