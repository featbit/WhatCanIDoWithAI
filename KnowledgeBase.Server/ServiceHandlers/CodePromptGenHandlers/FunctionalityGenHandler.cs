using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.Models;
using KnowledgeBase.ReportGenerator;
using MediatR;
using KnowledgeBase.ReportGenerator.Models;

namespace KnowledgeBase.Server.ServiceHandlers
{
    public class FunctionalityGenRequest : IRequest<Functionality>
    {
        public string Id { get; set; }
        public string FeatureId { get; set; }
        public string ModuleId { get; set; }
    }

    public class FunctionalityGenHandler(
        ICodePromptGenService codePromptGenService,
        IReportRepo reportRepo) : IRequestHandler<FunctionalityGenRequest, Functionality>
    {
        public async Task<Functionality> Handle(FunctionalityGenRequest request, CancellationToken cancellationToken)
        {
            Specification report = await reportRepo.GetSpecificationByReportIdAsync(request.Id) ??
                 throw new Exception("Failed to get specification");

            return await codePromptGenService.FunctionalityGenAsync(
                    report.Title,
                    report.Definition,
                    report.Features[0].Description,
                    report.Features[0].Modules[0].DetailDescription);
        }
    }
}