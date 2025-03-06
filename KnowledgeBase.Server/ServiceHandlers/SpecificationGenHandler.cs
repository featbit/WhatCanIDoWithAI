using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.ReportGenerator;
using KnowledgeBase.ReportGenerator.Models;
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
        IReportRepo reportRepo) : IRequestHandler<SpecGenRequest, Specification>
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

            spec.Features = await specGenService.GenerateFeatureContentAsync(spec, def.SaasFeatures) ??
                    throw new Exception("Failed to generate feature functionalities");

            for (int i = 0; i < spec.Features.Count; i++)
            {
                Feature f = spec.Features[i];
                f = await specGenService.GenerateFunctionalityDetailAsync(spec.Title, spec.Definition, f) ??
                    throw new Exception("Failed to generate subfeature detail");
                spec.Features[i] = f;
            }

            await reportRepo.AddReportAsync(spec);

            return spec;
        }
    }
}