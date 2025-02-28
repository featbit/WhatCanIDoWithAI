using KnowledgeBase.Server.Services;
using KnowledgeBase.SpecGenerator;
using KnowledgeBase.SpecGenerator.Models;
using MediatR;
using Pgvector.EntityFrameworkCore;

namespace KnowledgeBase.Server.ServiceHandlers
{
    public class SpecGenRequest : IRequest<Specification>
    {
        public string Query { get; set; }
        public string? Step { get; set; }
    }

    public class SpecificationGenHandler(ISpecificationGenService specGenService) : IRequestHandler<SpecGenRequest, Specification>
    {
        public async Task<Specification> Handle(SpecGenRequest request, CancellationToken cancellationToken)
        {
            Specification spec = new()
            {
                Title = request.Query,
                Definition = await specGenService.GenerateDefinitionAsync(request.Query)
            };
            if (spec.Definition != null)
                spec.Content = await specGenService.GenerateContentAsync(spec.Title, spec.Definition);
            return spec;
        }
    }
}
