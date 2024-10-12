using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.Note.Commands;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.LeaveLog.Commands.UpdateLeaveLog;

public record UpdateLeaveLogRequestStatusCommand : IRequest
{
    public Guid Id { get; init; }
    public string applicationUserId { get; set; }
    public LogStatus status { get; init; }
    public string? cancelReason { get; init; }
}
public class UpdateLeaveLogRequestStatusCommandHandler : IRequestHandler<UpdateLeaveLogRequestStatusCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UpdateLeaveLogRequestStatusCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateLeaveLogRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var CurrentLeaveLog = await _context.Get<Domain.Entities.LeaveLog>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (CurrentLeaveLog == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.LeaveLog), request.Id);
        }

        CurrentLeaveLog.Status = request.status;
        CurrentLeaveLog.CancelReason = request.cancelReason;

        await _context.SaveChangesAsync(cancellationToken);

        string des = "được: xử lý";
        if (request.status.ToString().ToLower().Equals("approved"))
        {
            des = "được: xác nhận";
        }
        else if (request.status.ToString().ToLower().Equals("cancel"))
        {
            des = "bị: từ chối";
        }
        else if (request.status.ToString().ToLower().Equals("noaction"))
        {
            des = "bị: bỏ qua";
        }

        var noti = _mediator.Send(new CreateNotiCommand()
        {
            ApplicationUserId = request.applicationUserId,
            Title = "Thông báo về việc nhận kết quả yêu cầu nghỉ làm tạm thời",
            Description = "Yêu cầu nghỉ làm tạm thời của bạn đã " + des + ", vui lòng xem chi tiết !"
        });

        return Unit.Value;
    }
}