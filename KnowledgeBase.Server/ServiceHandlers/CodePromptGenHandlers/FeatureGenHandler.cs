using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator;
using MediatR;

namespace FeatGen.Server.ServiceHandlers
{
    public class FeatureGenRequest : IRequest<string>
    {
        public string ReportId { get; set; }
        public string FeatureId { get; set; }
        public string GenVersion { get; set; }
        public string ChangeDescription { get; set; }
        public string AdditionalSpec { get; set; }
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

            var feature = reportCode.Code.CodeFeatures.FirstOrDefault(p => p.FeatureId == request.FeatureId);
            string featureCode = feature == null ? "" : feature.FeatureCode;

            var code = await codePromptGenService.FeatureHomePageGenAsync(
                spec, request.FeatureId, 
                reportCode.Theme.PrimaryColor, reportCode.Theme.SecondaryColor,
                request.GenVersion, request.ChangeDescription, featureCode);

            await reportCodeRepo.UpsertFeatureCodeAsync(code, request.ReportId, request.FeatureId);

            return code;
        }
    }
}