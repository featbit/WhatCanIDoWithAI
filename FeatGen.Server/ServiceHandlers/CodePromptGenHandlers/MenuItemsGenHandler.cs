using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator;
using MediatR;

namespace FeatGen.Server.ServiceHandlers
{
    public class MenuItemsGenRequest : IRequest<string>
    {
        public string ReportId { get; set; }
    }

    public class MenuItemsGenHandler(
        ICodePromptGenService codePromptGenService,
        IReportRepo reportRepo,
        IReportCodeRepo reportCodeRepo) : IRequestHandler<MenuItemsGenRequest, string>
    {
        public async Task<string> Handle(MenuItemsGenRequest request, CancellationToken cancellationToken)
        {
           Specification spec = await reportRepo.GetSpecificationByReportIdAsync(request.ReportId) ??
                throw new Exception("Failed to get specification");

            var menuItemsCode = await codePromptGenService.MenuItemCodeGenAsync(spec);
            await reportCodeRepo.UpsertReportCodeMenuItemsAsync(menuItemsCode, request.ReportId);

            return menuItemsCode;

        }
    }
}