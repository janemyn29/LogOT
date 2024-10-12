using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Note.Commands;
using mentor_v1.Application.OvertimeLog.Queries.GetOvertimeLog;
using mentor_v1.Domain.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace mentor_v1.Application.OvertimeLog.Commands.CreateOvertimeLog;

public class CreateOvertimeLogCommand : IRequest<Guid>
{
    //public string applicationUserId { get; set; }
    public CreateOvertimeLogViewModel createOvertimeLogViewModel { get; set; }

}

// Handler to handle the request (Can be written to another file)
// CreateOvertimeLogCommand : IRequest<Guid> => IRequestHandler<CreateOvertimeLogCommand, Guid>
public class CreateOvertimeLogCommandHandler : IRequestHandler<CreateOvertimeLogCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;


    public CreateOvertimeLogCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateOvertimeLogCommand request, CancellationToken cancellationToken)
    {
        if (request.createOvertimeLogViewModel.Date < DateTime.UtcNow)
        {
            throw new Exception("Ngày yêu cầu không thể trước thời gian hiện tại");
        }

        // create new OvertimeLog from request data
        var OvertimeLog = new Domain.Entities.OvertimeLog()
        {
            ApplicationUserId = request.createOvertimeLogViewModel.employeeId,
            Date = request.createOvertimeLogViewModel.Date,
            Hours = request.createOvertimeLogViewModel.Hours,
            Status = Domain.Enums.LogStatus.Request
        };

        // add new OvertimeLog
        _context.Get<Domain.Entities.OvertimeLog>().Add(OvertimeLog);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        
        // return the Guid
        return OvertimeLog.Id;
    }
}