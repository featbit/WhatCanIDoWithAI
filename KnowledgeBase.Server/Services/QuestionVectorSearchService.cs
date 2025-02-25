using KnowledgeBase.Server.Models;
using Pgvector.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace KnowledgeBase.Server.Services
{
    public interface IQuestionVectorSearchService
    {
        public Task<List<Document>> CosineSimilarityAsync(Pgvector.Vector searchVector, int retriveNumber = 5);
    }

    public class QuestionVectorSearchService(KnowledgeBaseDbContext dbContext) : IQuestionVectorSearchService
    {
        public async Task<List<Document>> CosineSimilarityAsync(Pgvector.Vector searchVector, int retriveNumber = 5)
        {
            List<Document> documents = await (
                from qv in dbContext.QuestionVectors
                orderby qv.GeTextEmb004Vector!.CosineDistance(searchVector)
                join doc in dbContext.Documents
                on qv.DocumentId equals doc.Id into docGroup
                from doc in docGroup.DefaultIfEmpty()
                select doc
            ).Where(d => d != null).Take(retriveNumber).ToListAsync();

            return documents;
        }
    }
}
