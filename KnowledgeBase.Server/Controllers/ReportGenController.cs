﻿using KnowledgeBase.FeatureFlag;
using KnowledgeBase.Server.ServiceHandlers;
using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBase.Server.Controllers
{
    [Route("api/reportgen")]
    public class ReportGenController(IFeatureFlagService flagService, ISender mediator) : ControllerBase
    {
        [HttpPost("specification")]
        [RequestTimeout(600)]
        public async Task<IActionResult> SpecificationGenAsync([FromBody] SpecGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.SpecGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("code/menuitems")]
        [RequestTimeout(600)]
        public async Task<IActionResult> CodeMenuItemsGenAsync([FromBody] MenuItemsGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.CodeMenuItemsGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("code/module")]
        [RequestTimeout(600)]
        public async Task<IActionResult> CodeSingleModuleGenAsync([FromBody] FeatureModuleGenRequest request)
        {
            if (!flagService.IsEnabled(FeatureFlagKeys.CodeSingleModulesGen))
            {
                return NotFound();
            }

            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}
