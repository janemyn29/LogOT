using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.ExcelEmployeeQuit.Commands;
public class CreateExcelEmployeeQuitCommand : IRequest<Guid>
{
    public Guid JobReportId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Identity { get; set; }
    public WorkStatus WorkStatus { get; set; }
    public ActionType ActionType { get; set; }
    public DateTime ActionDate { get; set; }

}

public class CreateExcelEmployeeQuitCommandHandler : IRequestHandler<CreateExcelEmployeeQuitCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public CreateExcelEmployeeQuitCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(CreateExcelEmployeeQuitCommand request, CancellationToken cancellationToken)
    {

        var job = new Domain.Entities.ExcelEmployeeQuit()
        {
            JobReportId = request.JobReportId,
            ActionType = request.ActionType,
            ActionDate = request.ActionDate,
            FullName = request.FullName,
            Username = request.Username, 
            Email = request.Email,
            Phone = request.Phone,
            WorkStatus = request.WorkStatus,
            Identity = request.Identity,
        };
        // add new category
        _context.Get<Domain.Entities.ExcelEmployeeQuit>().Add(job);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}

