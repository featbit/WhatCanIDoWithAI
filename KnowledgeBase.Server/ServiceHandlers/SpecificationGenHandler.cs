﻿using KnowledgeBase.Models.Components.SpecGenerator;
using KnowledgeBase.SpecGenerator;
using KnowledgeBase.SpecGenerator.Models;
using MediatR;

namespace KnowledgeBase.Server.ServiceHandlers
{
    public class SpecGenRequest : IRequest<Specification>
    {
        public string Query { get; set; }
        public string? Step { get; set; }
    }

    public class SpecificationGenHandler(
        ISpecificationGenService specGenService,
        ISpecificationGenRepo specGenRepo) : IRequestHandler<SpecGenRequest, Specification>
    {
        public async Task<Specification> Handle(SpecGenRequest request, CancellationToken cancellationToken)
        {
            Specification spec = new()
            {
                Title = request.Query,
            };
            Definition def = await specGenService.GenerateDefinitionAsync(request.Query) ??
                throw new Exception("Failed to generate definition");
            spec.Definition = def.ServiceDescription;
            Content cnt = await specGenService.GenerateContentAsync(spec.Title, def) ??
                throw new Exception("Failed to generate content");
            spec.Features = cnt.SaasFeatures.Select(f => new Feature
            {
                Description = f.Feature,
                Modules = f.SubFeatures.Select(sf => new Module
                {
                    ShortDescription = sf,
                    Id = Guid.NewGuid().ToString()
                }).ToList(),
                MenuItem = f.MenuItem,
            }).ToList();
            for (int i = 0; i < spec.Features.Count; i++)
            {
                Feature f = spec.Features[i];
                f = await specGenService.GenerateSubFeatureDetailAsync(spec.Title, spec.Definition, f) ??
                    throw new Exception("Failed to generate subfeature detail");
                spec.Features[i] = f;
            }

            await specGenRepo.AddReportAsync(spec);

            return spec;
        }
    }
}