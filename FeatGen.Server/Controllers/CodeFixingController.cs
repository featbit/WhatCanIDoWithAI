using FeatGen.FeatureFlag;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator;
using FeatGen.ReportGenerator.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Emit;

namespace FeatGen.Server.Controllers
{
    [Route("api/code-fixing")]
    public class CodeFixingController(ICodeFixService codeFixingService) : ControllerBase
    {

        [HttpPost("db-code")]
        [RequestTimeout(600)]
        public async Task<IActionResult> DbCodeFixing([FromBody] CodeFixingSignleFileRequest request)
        {
            var result = await codeFixingService.DbCodeFixing(request.FileCode, request.RequirementPrompt);
            return Ok(result);
        }

        [HttpPost("api-code")]
        [RequestTimeout(600)]
        public async Task<IActionResult> ApiCodeFixing([FromBody] CodeFixingSignleFileRequest request)
        {
            var result = await codeFixingService.ApiCodeFixing(request.FileCode, request.RequirementPrompt);
            return Ok(result);
        }

        [HttpPost("page-code")]
        [RequestTimeout(600)]
        public async Task<IActionResult> PageCodeFixing([FromBody] CodeFixingSignleFileRequest request)
        {
            var result = await codeFixingService.PageCodeFixing(request.FileCode, request.RequirementPrompt);
            return Ok(result);
        }
    }

    public class CodeFixingSignleFileRequest
    {
        public string RequirementPrompt { get; set; }
        public string FileCode { get; set; }
    }
}
