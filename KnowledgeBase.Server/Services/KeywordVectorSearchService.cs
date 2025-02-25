using KnowledgeBase.Server.Models;
using Pgvector.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace KnowledgeBase.Server.Services
{
    public interface IKeywordVectorSearchService
    {
        Task<List<Guid>> CosineSimilarityAsync(Pgvector.Vector searchVector, int retrieveNumber = 5);
    }

    public class KeywordVectorSearchService(KnowledgeBaseDbContext dbContext) : IKeywordVectorSearchService
    {
        public async Task<List<Guid>> CosineSimilarityAsync(Pgvector.Vector searchVector, int retrieveNumber = 5)
        {
            var documentIds = await dbContext.KeywordVectors
                .OrderBy(qv => qv.GeTextEmb004Vector!.CosineDistance(searchVector))
                .Take(retrieveNumber)
                .Select(qv => qv.DocumentId)
                .ToListAsync();

            return documentIds;
        }
    }
}
