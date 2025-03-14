using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.ReportGenerator;
using MediatR;

namespace KnowledgeBase.Server.ServiceHandlers
{
    public class FeatureGenRequest : IRequest<string>
    {
        public string ReportId { get; set; }
        public string FeatureId { get; set; }
    }

    public class FeatureGenHandler(
        ICodePromptGenService codePromptGenService,
        IReportRepo reportRepo,
        IReportCodeRepo reportCodeRepo) : IRequestHandler<FeatureGenRequest, string>
    {
        public async Task<string> Handle(FeatureGenRequest request, CancellationToken cancellationToken)
        {
            Specification spec = await reportRepo.GetSpecificationByReportIdAsync(request.ReportId) ??
                 throw new Exception("Failed to get specification");
            var reportCode = await reportCodeRepo.GetReportCodeAsync(request.ReportId);

            var code = await codePromptGenService.FeatureHomePageGenAsync(
                spec, request.FeatureId, reportCode.Theme.PrimaryColor, reportCode.Theme.SecondaryColor);

            await reportCodeRepo.UpsertFeatureCodeAsync(code, request.ReportId, request.FeatureId);

            return code;
        }
    }
}