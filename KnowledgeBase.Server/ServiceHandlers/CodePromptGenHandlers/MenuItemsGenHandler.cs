using KnowledgeBase.DataModels.ReportGenerator;
using KnowledgeBase.ReportGenerator;
using MediatR;

namespace KnowledgeBase.Server.ServiceHandlers
{
    public class MenuItemsGenRequest : IRequest<string>
    {
        public string ReportId { get; set; }
    }

    public class MenuItemsGenHandler(
        ICodePromptGenService codePromptGenService,
        IReportRepo reportRepo) : IRequestHandler<MenuItemsGenRequest, string>
    {
        public async Task<string> Handle(MenuItemsGenRequest request, CancellationToken cancellationToken)
        {
           Specification spec = await reportRepo.GetSpecificationByIdAsync(request.ReportId) ??
                throw new Exception("Failed to get specification");

            return await codePromptGenService.MenuItemCodeGenAsync(spec);

        }
    }
}