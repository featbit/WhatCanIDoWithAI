using FeatGen.Models.ReportGenerator;
using FeatGen.Models;
using FeatGen.ReportGenerator;
using MediatR;
using FeatGen.ReportGenerator.Models;

namespace FeatGen.Server.ServiceHandlers
{
    public class FunctionalityGenRequest : IRequest<string>
    {
        public string ReportId { get; set; }
        public string FeatureId { get; set; }
        public string ModuleId { get; set; }
        public string GenVersion { get; set; }
        public string AdditionalSpec { get; set; }
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

            string existingCode = "", primaryColor = "", secondaryColor = "";

            var reportCode = await reportCodeRepo.GetReportCodeAsync(request.ReportId);
            if (reportCode != null)
            {
                primaryColor = reportCode.Theme.PrimaryColor;
                secondaryColor = reportCode.Theme.SecondaryColor;
                var feature = reportCode.Code.CodeFeatures.FirstOrDefault(p => p.FeatureId == request.FeatureId);
                if (feature != null)
                {
                    var functionality = feature.CodeFunctionalities.FirstOrDefault(p => p.FunctionalityId == request.ModuleId);
                    if (functionality != null)
                    {
                        existingCode = functionality.Code;
                    }
                }
            }

            var code = await codePromptGenService.FunctionalityGenAsync(
                spec, request.FeatureId, request.ModuleId,
                primaryColor, secondaryColor,
                request.GenVersion, request.AdditionalSpec, existingCode);

            await reportCodeRepo.UpsertFunctaionalityCodeAsync(code, request.ReportId, request.FeatureId, request.ModuleId);

            return code;
        }
    }
}