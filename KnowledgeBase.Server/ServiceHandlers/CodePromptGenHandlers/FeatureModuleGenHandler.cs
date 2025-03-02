using KnowledgeBase.DataModels.ReportGenerator;
using KnowledgeBase.Models;
using KnowledgeBase.ReportGenerator;
using MediatR;

namespace KnowledgeBase.Server.ServiceHandlers
{
    public class FeatureModuleGenRequest : IRequest<string>
    {
        public string Id { get; set; }
        public string FeatureId { get; set; }
        public string ModuleId { get; set; }
    }

    public class FeatureModuleGenHandler(
        ICodePromptGenService codePromptGenService,
        IReportRepo reportRepo) : IRequestHandler<FeatureModuleGenRequest, string>
    {
        public async Task<string> Handle(FeatureModuleGenRequest request, CancellationToken cancellationToken)
        {
            Specification report = await reportRepo.GetSpecificationByIdAsync(request.Id) ??
                 throw new Exception("Failed to get specification");

            return await codePromptGenService.FeatureModuleGenAsync(
                    report.Title,
                    report.Definition,
                    report.Features[0].Description,
                    report.Features[0].Modules[0].DetailDescription);
        }
    }
}