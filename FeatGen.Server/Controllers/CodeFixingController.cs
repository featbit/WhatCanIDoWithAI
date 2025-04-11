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

        [HttpPost("single-file")]
        [RequestTimeout(600)]
        public async Task<IActionResult> SingleFile([FromBody] CodeFixingSignleFileRequest request)
        {
            var result = await codeFixingService.FixingSingleFileCodeError(request.FileCode, request.RequirementPrompt);
            return Ok(result);
        }
    }

    public class CodeFixingSignleFileRequest
    {
        public string RequirementPrompt { get; set; }
        public string FileCode { get; set; }
    }
}
