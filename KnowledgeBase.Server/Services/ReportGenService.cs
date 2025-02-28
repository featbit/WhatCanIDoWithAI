using KnowledgeBase.Server.Models;
using Pgvector.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace KnowledgeBase.Server.Services
{
    public interface IReportGenService
    {
        Task GenerateDefinitionAsync(string title);
    }

    public class ReportGenService(KnowledgeBaseDbContext dbContext) : IReportGenService
    {
        public async Task GenerateDefinitionAsync(string title)
        {
        }
    }
}
