using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.Model;
using KnowledgeBaseAPIs.Models;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBaseAPIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomReportController(IFbClient fbClient) : ControllerBase
    {
        [HttpPost("GetReport")]
        public IActionResult GetReport([FromBody] ClientParam client)
        {
            var fbUser = FbUser.Builder(client.Id)
                               .Name(client.Name)
                               .Custom("region", client.Region)
                               .Build();
            
            var customReportFlag = fbClient.BoolVariation("custom-report", fbUser, defaultValue: false);

            return customReportFlag switch
            {
                true => Ok($"True for client {client.Id}"),
                _ => NotFound()
            };
        }
    }
}