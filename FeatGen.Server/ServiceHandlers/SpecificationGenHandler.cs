using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using MediatR;

namespace FeatGen.Server.ServiceHandlers
{
    public class SpecGenRequest : IRequest<Specification>
    {
        public string Query { get; set; }
        public string? Step { get; set; }
        public string Requirement { get; set; }
    }

    public class SpecificationGenHandler(
        ISpecificationGenService specGenService,
        IReportRepo reportRepo) : IRequestHandler<SpecGenRequest, Specification>
    {
        public async Task<Specification> Handle(SpecGenRequest request, CancellationToken cancellationToken)
        {
            Specification spec = new()
            {
                Title = request.Query,
            };
            request.Requirement = request.Requirement ?? "no additional requirement";
            Definition def = await specGenService.GenerateDefinitionAsync(request.Query, request.Requirement) ??
                throw new Exception("Failed to generate definition");
            spec.Definition = def.ServiceDescription;

            spec.Features = await specGenService.GenerateFeatureContentAsync(spec, def.SaasFeatures, request.Requirement) ??
                    throw new Exception("Failed to generate feature functionalities");

            for (int i = 0; i < spec.Features.Count; i++)
            {
                Feature f = spec.Features[i];
                f = await specGenService.GenerateFunctionalityDetailAsync(spec.Title, spec.Definition, f, request.Requirement) ??
                    throw new Exception("Failed to generate subfeature detail");
                spec.Features[i] = f;
            }

            await reportRepo.AddReportAsync(spec);

            return spec;
        }
    }
}