using KnowledgeBase.Server.Services;
using MediatR;

namespace KnowledgeBase.Server.ServiceHandlers
{
    public class KnowledgeSearchRequest : IRequest<string>
    {
        public string RequestText { get; set; }
        public string? RequestSerieId { get; set; }
    }

    public class KnowledgeSearchHandler(IQuestionVectorSearchService questionSearchService) : IRequestHandler<KnowledgeSearchRequest, string>
    {
        public async Task<string> Handle(KnowledgeSearchRequest request, CancellationToken cancellationToken)
        {
            float[] targetEmbeddingData = Enumerable.Repeat(1f, 768).ToArray();
            var csResults = await questionSearchService.CosineSimilarityAsync(new Pgvector.Vector(targetEmbeddingData));
            return $"cosine result number: {csResults.Count}";
        }
    }
}
