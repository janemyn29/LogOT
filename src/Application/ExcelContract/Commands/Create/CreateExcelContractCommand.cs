using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.ExcelContract.Commands.Create;
public class CreateExcelContractCommand : IRequest<Guid>
{
    public Guid JobReportId { get; set; }
    public string ContractCode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndTime { get; set; }
    public string EmployeeName { get; set; }
    public string IdentityNumber { get; set; }
    public string ContractStatus { get; set; }
    public ActionType Action { get; set; }
    public DateTime ActionDate { get; set; }

}

public class CreateExcelContractCommandHandler : IRequestHandler<CreateExcelContractCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public CreateExcelContractCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(CreateExcelContractCommand request, CancellationToken cancellationToken)
    {

        var job = new Domain.Entities.ExcelContract()
        {
            JobReportId = request.JobReportId,
            ContractCode = request.ContractCode,
            StartDate = request.StartDate,
            EndTime = request.EndTime,
            EmployeeName = request.EmployeeName,
            IdentityNumber = request.IdentityNumber,
            ContractStatus = request.ContractStatus,
            Action = request.Action,
            ActionDate = request.ActionDate
        };
        // add new category
        _context.Get<Domain.Entities.ExcelContract>().Add(job);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}
