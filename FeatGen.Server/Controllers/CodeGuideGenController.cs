using FeatGen.FeatureFlag;
using FeatGen.OpenAI;
using FeatGen.ReportGenerator;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Emit;

namespace FeatGen.Server.Controllers
{
    [Route("api/codeguide")]
    public class CodeGuideGenController(
        IFeatureFlagService flagService,
        ISender mediator,
        IAntropicChatService antropicChatService,
        IGeminiChatService geminiChatService,
        ICodeGuideGenService codeGuideGenService,
        ICodeGuideFetchService codeGuideFetchService) : ControllerBase
    {

        [HttpPost("pages")]
        [RequestTimeout(600)]
        public async Task<IActionResult> Pages([FromBody] CodeGuideRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}

            var result = await codeGuideGenService.GeneratePagesAsync(request.ReportId);
            return Ok(result);
        }

        [HttpGet("generated-pages/{reportId}")]
        [RequestTimeout(600)]
        public async Task<IActionResult> GetGeneratedPages(string reportId)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}

            var result = await codeGuideFetchService.GetGeneratedPagesAsync(reportId);
            return Ok(result);
        }

        [HttpPost("css-code")]
        [RequestTimeout(600)]
        public async Task<IActionResult> GenerateCssCode([FromBody] CodeGuideRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}

            var result = await codeGuideGenService.GenerateCssCodeAsync(request.ReportId);
            return Ok(result);
        }

        [HttpPost("menu-items")]
        [RequestTimeout(600)]
        public async Task<IActionResult> MenuItems([FromBody] CodeGuideRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}

            var result = await codeGuideGenService.GenerateMenuItemsAsync(request.ReportId);
            return Ok(result);
        }

        [HttpPost("menu-items-code")]
        [RequestTimeout(600)]
        public async Task<IActionResult> MenuItemsCode([FromBody] CodeGuideRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}

            var result = await codeGuideGenService.GenerateMenuItemsCodeAysnc(request.ReportId);
            return Ok(result);
        }

        [HttpGet("generated-menu-items/{reportId}")]
        [RequestTimeout(600)]
        public async Task<IActionResult> GetGeneratedMenuItems(string reportId)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}

            var result = await codeGuideFetchService.GetGeneratedMenuItemsAsync(reportId);
            return Ok(result);
        }

        [HttpPost("project-models")]
        [RequestTimeout(600)]
        public async Task<IActionResult> ProjectModels([FromBody] CodeGuideRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}

            var result = await codeGuideGenService.GenerateDataModelsAsync(request.ReportId);
            return Ok(result);
        }

        [HttpPost("fake-data-base")]
        [RequestTimeout(600)]
        public async Task<IActionResult> FakeDataBase([FromBody] CodeGuideRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}

            var result = await codeGuideGenService.GenerateFakeDataBaseAsync(request.ReportId);
            return Ok(result);
        }

        [HttpPost("fake-data-base-extract")]
        [RequestTimeout(600)]
        public async Task<IActionResult> ExtractImportantMemoryDBCode([FromBody] CodeGuideRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}

            var result = await codeGuideGenService.ExtractImportantMemoryDBCodeAsync(request.ReportId);
            return Ok(result);
        }

        [HttpPost("page-api-db-interfaces")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> PageApiDbInterfaces([FromBody] CodeGuideInterfacesRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}
            var result = await codeGuideGenService.GeneratePageApiDbInterfacesAsync(request.ReportId, request.PageId, request.MenuItem);
            return Ok(result);
        }
        [HttpPost("page-api-db-models")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> PageDbModels([FromBody] CodeGuideDbModelsRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}
            var result = await codeGuideGenService.DefineDedicatedMemoryDbModel(request.ReportId, request.PageId, request.MenuItem, request.ApiCode, request.InterfaceDefinition);
            return Ok(result);
        }
        [HttpPost("page-api-db-code")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> PageApiDbCode([FromBody] CodeGuideDbCodeRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}
            var result = await codeGuideGenService.GenerateDedicatedMemoryDBCode(request.ReportId, request.PageId, request.MenuItem, request.InterfaceDefinition, request.DbModels);
            return Ok(result);
        }
        [HttpPost("page-api-code-update")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> PageApiCodeUpdate([FromBody] CodeGuideApiCodeRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}
            var result = await codeGuideGenService.GenerateApiCodeWithDedicatedDbCodeAsync(request.ReportId, request.PageId, request.MenuItem, request.InterfaceDefinition, request.DbCode);
            
            return Ok(result);
        }
        [HttpPost("page-code-update")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> UpdateExistingPageCodeWithNewApiCode([FromBody] CodeGuideNewPageCodeRequest request)
        {
            //if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            //{
            //    return NotFound();
            //}
            var result = await codeGuideGenService.GeneratePageCodeAsync(request.ReportId, request.PageId, request.MenuItem, request.ApiCode, request.CssCode, request.DbCode, request.DbModels, request.ThemeIconPrompt, request.ThemeChartPrompt);
            return Ok(result);
        }
        


        [HttpPost("component-code-gemini")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> ComponentCodeGemini()
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await geminiChatService.CompleteChatAsync("hello");
            return Ok(result);
        }

        [HttpPost("user-manual-page")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> UserManualByPage([FromBody] CodeGuideUserManualRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideGenService.GenerateUserManualByPage(
                request.ReportId, request.PageId, request.PageComponent);
            result = result
                .Replace("**：", "：")
                .Replace("*   **", "- ")
                .Replace("1.  **", "1. ")
                .Replace("2.  **", "2. ")
                .Replace("3.  **", "3. ")
                .Replace("4.  **", "4. ")
                .Replace("5.  **", "5. ")
                .Replace("    *   ", "    - ")
                .Replace("*   ", "- ")
                .Replace("1.  **", "1. ")
                .Replace(":**", ":");
            return Ok(result);
        }

        [HttpPost("application-form")]
        [RequestTimeout(600000)]
        public async Task<IActionResult> ApplicationForm([FromBody] CodeGuideRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await codeGuideGenService.GenerateApplicationForm(request.ReportId);
            return Ok(result);
        }

        
    }

    public class CodeGuideRequest
    {
        public string ReportId { get; set; }
    }

    public class CodeGuideCodeRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
    }

    public class CodeGuideComponentCodeRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
        public string PageComponentName { get; set; }
        public string ApiCode { get; set; }
        public string CssCode { get; set; }
    }

    public class CodeGuideInterfacesRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
        public string MenuItem { get; set; }
        public string ApiCode { get; set; }
        public string MemoryDbCode { get; set; }
        public string PageCode { get; set; }
    }

    public class CodeGuideDbModelsRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
        public string MenuItem { get; set; }
        public string ApiCode { get; set; }
        public string InterfaceDefinition { get; set; }
    }
    public class CodeGuideDbCodeRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
        public string MenuItem { get; set; }
        public string ApiCode { get; set; }
        public string InterfaceDefinition { get; set; }
        public string DbModels { get; set; }
    }
    public class CodeGuideApiCodeRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
        public string MenuItem { get; set; }
        public string ApiCode { get; set; }
        public string InterfaceDefinition { get; set; }
        public string DbCode { get; set; }
    }
    public class CodeGuideNewPageCodeRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
        public string MenuItem { get; set; }
        public string ApiCode { get; set; }
        public string CssCode { get; set; }
        public string DbCode { get; set; }
        public string DbModels { get; set; }
        public string ThemeIconPrompt { get; set; }
        public string ThemeChartPrompt { get; set; }
    }

    public class CodeGuideUserManualRequest
    {
        public string ReportId { get; set; }
        public string PageId { get; set; }
        public string PageComponent { get; set; }
    }
}
