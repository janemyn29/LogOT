using MediatR;
using mentor_v1.Application.Attendance.Commands.CreateAttendance;
using mentor_v1.Application.Attendance.Commands.UpdateAttendance;
using mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebUI.Services.AttendanceServices;

public class AttendanceService : IAttendanceService
{
    private readonly IMediator _mediator;

    public AttendanceService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<string> AttendanceFullDay(DateTime now,ApplicationUser user)
    {
        var time = now.TimeOfDay;

        //lấy cấu hình ca làm:
        var listShift = await _mediator.Send(new GetListShiftRequest { });
        TimeSpan start1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Morning).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
        TimeSpan end1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Morning).FirstOrDefault().EndTime.Value.TimeOfDay;
        TimeSpan start2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Afternoon).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
        TimeSpan end2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Afternoon).FirstOrDefault().EndTime.Value.TimeOfDay;

        //bắt dầu phân loại ca.
        if (time >= start1 && time < end1)
        {
            try
            {
                var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Morning, userId = user.Id });
                throw new Exception( "Bạn đã chấm công Ca Sáng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Sáng!");
            }
            catch (Exception)
            {
                try
                {
                    var Attendance = await _mediator.Send(new CreateAttendanceCommand
                    {
                        ApplicationUserId = user.Id,
                        Day = now,
                        StartTime = now,
                        ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Morning
                    });
                    return "Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                }
                catch (Exception ex)
                {
                    throw new Exception("Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                }
            }
        }
        else if (time >= end1 && time < start2)
        {
                await _mediator.Send(new UpdateEndTimeCommand { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
            //chấm công kết ca 1.
        }
        else if (time >= start2 && time < end2)
        {
            try
            {
                var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Afternoon, userId = user.Id });
                throw new Exception("Bạn đã chấm công Ca Chiều Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Chiều!");
            }
            catch (Exception)
            {
                try
                {
                    var Attendance = await _mediator.Send(new CreateAttendanceCommand
                    {
                        ApplicationUserId = user.Id,
                        Day = now,
                        StartTime = now,
                        ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Afternoon

                    });
                    return "Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                }
                catch (Exception ex)
                {
                    throw new Exception("Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                }
            }
        }
        else if (time >= end2 && time <= TimeSpan.Parse("23:30:00"))
        {
            //chấm công kết ca 2
           
                await _mediator.Send(new UpdateEndTimeCommand { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Afternoon });
                return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
           
        }
        else
        {
            throw new Exception("Hiện đang không trong ca làm việc nên bạn không thể chấm công được!");
        }
    }
}
