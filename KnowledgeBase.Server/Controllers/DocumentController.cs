using FeatBit.Sdk.Server;
using FeatGen.Models;
using FeatGen.Server.ServiceHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace FeatGen.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController(IFbClient fbClient, KnowledgeBaseDbContext dbContext) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] DocumentBody body)
        {
            Document document = new()
            {
                Id = Guid.NewGuid(),
                DocumentType = DocumentType.Article.ToString(),
                Text = body.Text,
                OnlineUrl = body.OnlineUrl
            };
            dbContext.Documents.Add(document);
            await dbContext.SaveChangesAsync();

            float[] orignalEmbedding768 = Enumerable.Repeat(1f, 768).ToArray();
            float[] orignalEmbedding1536 = Enumerable.Repeat(1f, 768).ToArray();
            float[] orignalEmbedding3072 = Enumerable.Repeat(1f, 768).ToArray();
            KeywordVector kv = new()
            {
                Id = Guid.NewGuid(),
                DocumentId = document.Id,
                Keywords = "Feature Flag, Open Source, Open Feature, .NET, Microsoft Feature Management",
                GeTextEmb004Vector = new Vector(orignalEmbedding768),
                OpTextEmb3SmVector = new Vector(orignalEmbedding1536),
                OpTextEmb3LgVector = new Vector(orignalEmbedding3072)
            };
            dbContext.KeywordVectors.Add(kv);
            await dbContext.SaveChangesAsync();

            float[] targetEmbeddingData = Enumerable.Repeat(1f, 768).ToArray();
            var items = await dbContext.KeywordVectors
                        .OrderBy(x => x.GeTextEmb004Vector!.CosineDistance(new Vector(targetEmbeddingData)))
                        .Take(5)
                        .ToListAsync();

            foreach (KeywordVector item in items)
            {
                if (item.GeTextEmb004Vector != null)
                {
                    return Ok(item.DocumentId);
                }
            }

            return Ok("Empty");
        }
    }

    public class DocumentBody
    {
        public string OnlineUrl { get; set; }
        public string Text { get; set; }
    }

}