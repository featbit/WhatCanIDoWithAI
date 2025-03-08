using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.Models;
using KnowledgeBase.ReportGenerator;
using MediatR;
using KnowledgeBase.ReportGenerator.Models;

namespace KnowledgeBase.Server.ServiceHandlers
{
    public class FunctionalityGenRequest : IRequest<string>
    {
        public string ReportId { get; set; }
        public string FeatureId { get; set; }
        public string ModuleId { get; set; }
    }

    public class FunctionalityGenHandler(
        ICodePromptGenService codePromptGenService,
        IReportRepo reportRepo,
        IReportCodeRepo reportCodeRepo) : IRequestHandler<FunctionalityGenRequest, string>
    {
        public async Task<string> Handle(FunctionalityGenRequest request, CancellationToken cancellationToken)
        {
            Specification spec = await reportRepo.GetSpecificationByReportIdAsync(request.ReportId) ??
                 throw new Exception("Failed to get specification");


            var code = await codePromptGenService.FunctionalityGenAsync(
                spec, request.FeatureId, request.ModuleId);

            await reportCodeRepo.UpsertFunctaionalityCodeAsync(code, request.ReportId, request.FeatureId, request.ModuleId);

            return code;
        }
    }
}