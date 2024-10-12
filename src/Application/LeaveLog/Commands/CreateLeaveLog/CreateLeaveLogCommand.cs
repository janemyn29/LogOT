using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;
using mentor_v1.Application.Note.Commands;
using Microsoft.AspNetCore.Identity;

namespace mentor_v1.Application.LeaveLog.Commands.CreateLeaveLog;

public class CreateLeaveLogCommand : IRequest<Guid>
{
    public Domain.Identity.ApplicationUser user { get; set; }
    public CreateLeaveLogViewModel createLeaveLogViewModel { get; set; }

}

// Handler to handle the request (Can be written to another file)
// CreateLeaveLogCommand : IRequest<Guid> => IRequestHandler<CreateLeaveLogCommand, Guid>
public class CreateLeaveLogCommandHandler : IRequestHandler<CreateLeaveLogCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<Domain.Identity.ApplicationUser> _userManager;
    private readonly IMediator _mediator;

    public CreateLeaveLogCommandHandler(IApplicationDbContext context, UserManager<Domain.Identity.ApplicationUser> userManager, IMediator mediator)
    {
        _context = context;
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateLeaveLogCommand request, CancellationToken cancellationToken)
    {
        /*if (request.createLeaveLogViewModel.StartDate < DateTime.UtcNow)
        {
            throw new Exception("Ngày yêu cầu không thể trước thời gian hiện tại");
        } else if (request.createLeaveLogViewModel.StartDate > request.createLeaveLogViewModel.EndDate)
        {
            throw new Exception("Ngày bắt đầu phải trước ngày kết thúc");
        }*/

        // create new LeaveLog from request data
        var LeaveLog = new Domain.Entities.LeaveLog()
        {
            ApplicationUserId = request.user.Id,
            LeaveDate = request.createLeaveLogViewModel.LeaveDate,
            LeaveShift = request.createLeaveLogViewModel.LeaveShift,
            Reason = request.createLeaveLogViewModel.Reason,
            Status = Domain.Enums.LogStatus.Request
        };

        /*var noti = new CreateNotiCommand()
        {
            ApplicationUserId = request.createLeaveLogViewModel.ApplicationUserId,
            Title = "Thông báo về việc nhận yêu cầu OT",
            Description = "Bạn vừa có 1 yêu cầu OT " + request.createOvertimeLogViewModel.Hours + " tiếng, vào lúc: " + DateTime.Now + ", vui lòng xác nhận trong thời gian sớm nhất!"
        };*/

        var listManager = await _userManager.GetUsersInRoleAsync("Manager");

        foreach (var item in listManager)
        {

            var noti = await _mediator.Send(new CreateNotiCommand()
            {
                ApplicationUserId = item.Id,
                Title = "Thông báo về việc yêu cầu xin nghỉ làm tạm thời của nhân viên",
                Description = "Nhân viên: " + request.user.Fullname + " \n" +
                " đã gửi yêu cầu nghỉ làm tạm thời ca làm: " + request.createLeaveLogViewModel.LeaveShift + "\n" +
                "vào ngày: " + request.createLeaveLogViewModel.LeaveDate
            });
        }

        // add new LeaveLog
        _context.Get<Domain.Entities.LeaveLog>().Add(LeaveLog);

        // commit change to database
        // because the function is async so we await it
        await _context.SaveChangesAsync(cancellationToken);

        // return the Guid
        return LeaveLog.Id;
    }
}