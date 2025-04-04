using FeatGen.FeatureFlag;
using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator;
using FeatGen.Server.ServiceHandlers;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FeatGen.Server.Controllers
{
    [Route("api/utils")]
    public class UtilsController(
        ICodeUtilsService codeUtilService) : ControllerBase
    {
        [HttpGet("reportid-by-title/{title}")]
        [RequestTimeout(600)]
        public async Task<string> GetReportIdByTitle(string title)
        {
            return await codeUtilService.GetReportIdByTitleAsync(title);
        }
    }
}
