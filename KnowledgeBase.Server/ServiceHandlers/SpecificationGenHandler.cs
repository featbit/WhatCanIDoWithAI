using KnowledgeBase.Server.Services;
using KnowledgeBase.SpecGenerator;
using KnowledgeBase.SpecGenerator.Models;
using MediatR;

namespace KnowledgeBase.Server.ServiceHandlers
{
    public class SpecGenRequest : IRequest<Definition?>
    {
        public string Query { get; set; }
        public string? Step { get; set; }
    }

    public class SpecificationGenHandler(ISpecificationGenService specGenService) : IRequestHandler<SpecGenRequest, Definition?>
    {
        public async Task<Definition?> Handle(SpecGenRequest request, CancellationToken cancellationToken)
        {
            Definition? definition = await specGenService.GenerateDefinitionAsync(request.Query);
            return definition;
        }
    }
}
