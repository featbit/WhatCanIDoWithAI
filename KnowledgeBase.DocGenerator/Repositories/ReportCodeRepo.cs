using KnowledgeBase.Models;
using KnowledgeBase.Models.ReportGenerator;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.ReportGenerator
{
    public interface IReportCodeRepo
    {
        Task UpsertFunctaionalityCodeAsync(
            string code, string reportId, string featureId, string functionalityId);
        Task UpsertReportCodeMenuItemsAsync(string code, string reportId);
        Task<ReportCode?> GetReportCodeAsync(string id);
    }

    public class ReportCodeRepo(KnowledgeBaseDbContext dbContext) : IReportCodeRepo
    {
        public async Task UpsertFunctaionalityCodeAsync(
            string code, string reportId, string featureId, string functionalityId)
        {
            var rc = await dbContext.ReportCodes.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(reportId));
            if (rc == null)
            {
                rc = new ReportCode
                {
                    Id = Guid.NewGuid(),
                    ReportId = Guid.Parse(reportId),
                    Code = new CodeForReport
                    {
                        CodeMenuItems = code,
                        CodeFeatures = new List<ReportCodeFeature>()
                        {
                            new ReportCodeFeature
                            {
                                FeatureId = featureId,
                                CodeFunctionalities = new List<ReportCodeFunctionality>
                                {
                                    new ReportCodeFunctionality
                                    {
                                        Code = code,
                                        FunctionalityId = functionalityId
                                    }
                                }
                            }
                        }
                    }
                };
                dbContext.ReportCodes.Add(rc);
            }
            else
            {
                var feature = rc.Code.CodeFeatures.FirstOrDefault(p => p.FeatureId == featureId);
                if(feature != null)
                {
                    var functionality = feature.CodeFunctionalities.FirstOrDefault(p => p.FunctionalityId == functionalityId);
                    if(functionality != null)
                    {
                        functionality.Code = code;
                    }
                    else
                    {
                        feature.CodeFunctionalities.Add(new ReportCodeFunctionality
                        {
                            Code = code,
                            FunctionalityId = functionalityId
                        });
                    }
                }
                else
                {
                    rc.Code.CodeFeatures.Add(new ReportCodeFeature
                    {
                        FeatureId = featureId,
                        //CodeFunctionalities = new List<ReportCodeFunctionality>
                        //{
                        //    new ReportCodeFunctionality
                        //    {
                        //        Code = code,
                        //        FunctionalityId = functionalityId
                        //    }
                        //}
                    });
                }
                dbContext.ReportCodes.Update(rc);
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task<ReportCode?> GetReportCodeAsync(string id)
        {
            return await dbContext.ReportCodes.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(id));
        }

        public async Task UpsertReportCodeMenuItemsAsync(string code, string reportId)
        {
            var rc = await dbContext.ReportCodes.FirstOrDefaultAsync(p => p.ReportId == Guid.Parse(reportId));
            if(rc == null)
            {
                rc = new ReportCode
                {
                    Id = Guid.NewGuid(),
                    ReportId = Guid.Parse(reportId),
                    Code = new CodeForReport
                    {
                        CodeMenuItems = code,
                        CodeFeatures = new List<ReportCodeFeature>()
                    }
                };
                dbContext.ReportCodes.Add(rc);
            }
            else
            {
                rc.Code.CodeMenuItems = code;
                dbContext.ReportCodes.Update(rc);
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
