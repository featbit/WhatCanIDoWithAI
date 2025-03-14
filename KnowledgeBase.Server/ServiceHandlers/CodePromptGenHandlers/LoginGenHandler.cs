using KnowledgeBase.Models;
using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.ReportGenerator;
using MediatR;

namespace KnowledgeBase.Server.ServiceHandlers
{
    public class LoginGenRequest : IRequest<string>
    {
        public string ReportId { get; set; }
    }

    public class LoginGenHandler(
        ICodePromptGenService codePromptGenService,
        IReportRepo reportRepo,
        IReportCodeRepo reportCodeRepo) : IRequestHandler<LoginGenRequest, string>
    {
        public async Task<string> Handle(LoginGenRequest request, CancellationToken cancellationToken)
        {
           Specification spec = await reportRepo.GetSpecificationByReportIdAsync(request.ReportId) ??
                throw new Exception("Failed to get specification");
            var code = await reportCodeRepo.GetReportCodeAsync(request.ReportId);

            var loginCode = await codePromptGenService.LoginPageGenAsync(
                spec, code.Theme.PrimaryColor, code.Theme.SecondaryColor);
            await reportCodeRepo.UpsertLoginCodeMenuItemsAsync(loginCode, request.ReportId);

            return loginCode;

        }
    }
}