using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.ApplicationUser.Queries.GetUser;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.JobReport.Commands.Create;
public class CreateJobReportCommand : IRequest<Guid>
{
    public string Title { get; set; }
    
    public string Job { get; set; }
    public DateTime ActionDate { get; set; }

    public ActionType ActionType { get; set; }
}

public class CreateJobReportCommandHandler : IRequestHandler<CreateJobReportCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;


    public CreateJobReportCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(CreateJobReportCommand request, CancellationToken cancellationToken)
    {
      
            var job = new Domain.Entities.JobReport()
            {
                Title= request.Title, Job= request.Job, ActionDate= request.ActionDate, ActionType = request.ActionType
            };
            // add new category
            _context.Get<Domain.Entities.JobReport>().Add(job);
            await _context.SaveChangesAsync(cancellationToken);
        
        return job.Id;
    }
}

