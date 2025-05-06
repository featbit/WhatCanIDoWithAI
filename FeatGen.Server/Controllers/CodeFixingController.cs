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
        [RequestTimeout(600000)]
        public async Task<IActionResult> DbCodeFixing([FromBody] CodeFixingSignleFileRequest request)
        {
            var result = await codeFixingService.DbCodeFixing(request.FileCode, request.RequirementPrompt);
            return Ok(result);
        }

        [HttpPost("api-code")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> ApiCodeFixing([FromBody] CodeFixingSignleFileRequest request)
        {
            var result = await codeFixingService.ApiCodeFixing(request.FileCode, request.RequirementPrompt);
            return Ok(result);
        }

        [HttpPost("page-code")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> PageCodeFixing([FromBody] CodeFixingSignleFileRequest request)
        {
            var result = await codeFixingService.PageCodeFixing(request.FileCode, request.RequirementPrompt);
            return Ok(result);
        }

        [HttpPost("choose-files")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> ChooseFiles([FromBody] CodeFixingFilePickerRequest request)
        {
            var result = await codeFixingService.ChooseFiles(
                request.ReportId,
                request.MenuItem,
                request.DbFileCode,
                request.ApisFileCode,
                request.PageFileCode,
                request.HumanInput);
            return Ok(result);
        }

    }

    public class CodeFixingSignleFileRequest
    {
        public string RequirementPrompt { get; set; }
        public string FileCode { get; set; }
    }

    public class CodeFixingFilePickerRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
        public string MenuItem { get; set; }
        public string DbFileCode { get; set; }
        public string ApisFileCode { get; set; }
        public string PageFileCode { get; set; }
        public string HumanInput { get; set; }
    }
}
