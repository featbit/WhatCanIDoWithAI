﻿using FeatGen.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace FeatGen.Server.Services
{
    public interface IKeywordVectorSearchService
    {
        Task<List<Guid>> CosineSimilarityAsync(Pgvector.Vector searchVector, int retrieveNumber = 5);
    }

    public class KeywordVectorSearchService(FeatGenDbContext dbContext) : IKeywordVectorSearchService
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
