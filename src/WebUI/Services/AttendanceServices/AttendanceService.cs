using MediatR;
using mentor_v1.Application.Attendance.Commands.CreateAttendance;
using mentor_v1.Application.Attendance.Commands.UpdateAttendance;
using mentor_v1.Application.Attendance.Queries.GetAttendanceWithRelativeObject;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.LeaveLog.Queries.GetLeaveLogByRelativeObject;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Domain.Entities;
using mentor_v1.Domain.Enums;
using mentor_v1.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Services.AttendanceServices;
//Ở chấm công đi làm chiều thì nếu buối sáng đã làm hết OT rồi thì chiều ko cho chấm công nữa
public class AttendanceService : IAttendanceService
{
    private readonly IMediator _mediator;
    private readonly IApplicationDbContext _context;

    public AttendanceService(IMediator mediator, IApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }
    public async Task<string> AttendanceFullDay(DateTime now, ApplicationUser user)
    {
        var leaveLog = await _mediator.Send(new GetLeaveLogByUserIdRequest { UserId = user.Id , day = now});
        if (leaveLog == null) //ko nghỉ
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
                var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Morning, userId = user.Id });
                if (attendace != null && attendace.EndTime != null)
                {
                    throw new Exception("Bạn đã chấm công Ca Sáng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Sáng!");

                }
                else if (attendace != null && attendace.EndTime == null)
                {
                    await _mediator.Send(new UpdateEndTimeCommand { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                    return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    //chấm công kết ca 1.
                }
                else
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

                var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Afternoon, userId = user.Id });
                if (attendace != null && attendace.EndTime != null)
                {
                    throw new Exception("Bạn đã chấm công Ca Chiều Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Chiều!");
                }
                else if (attendace != null && attendace.EndTime == null)
                {

                    await _mediator.Send(new UpdateEndTimeCommand { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Afternoon });
                    return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                }
                else
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
        else //có nghỉ
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
                if (leaveLog.LeaveShift == LeaveShift.Morning || leaveLog.LeaveShift == LeaveShift.Full)
                {
                    throw new Exception("Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " bạn có lịch nghỉ ca sáng nên không thể chấm công được!");

                }
                var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Morning, userId = user.Id });
                    if (attendace != null && attendace.EndTime != null)
                    {
                        throw new Exception("Bạn đã chấm công Ca Sáng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Sáng!");

                    }
                    else if (attendace != null && attendace.EndTime == null)
                    {
                        await _mediator.Send(new UpdateEndTimeCommand { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                        return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                        //chấm công kết ca 1.
                    }
                    else
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
                if (leaveLog.LeaveShift == LeaveShift.Afternoon || leaveLog.LeaveShift == LeaveShift.Full)
                {
                    throw new Exception("Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " bạn có lịch nghỉ ca chiều nên không thể chấm công được!");

                }
                var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Afternoon, userId = user.Id });
                    if (attendace != null && attendace.EndTime != null)
                    {
                        throw new Exception("Bạn đã chấm công Ca Chiều Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Chiều!");
                    }
                    else if (attendace != null && attendace.EndTime == null)
                    {

                        await _mediator.Send(new UpdateEndTimeCommand { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Afternoon });
                        return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    }
                    else
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

    public async Task<string> AttendanceMorningOnly(DateTime now, ApplicationUser user)
    {
        var leaveLog = await _mediator.Send(new GetLeaveLogByUserIdRequest { UserId = user.Id, day = now });
        if (leaveLog == null) //ko nghỉ
        {
            var time = now.TimeOfDay;

            //lấy cấu hình ca làm:
            var listShift = await _mediator.Send(new GetListShiftRequest { });
            TimeSpan start1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().EndTime.Value.TimeOfDay;
            TimeSpan start2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().EndTime.Value.TimeOfDay;

            //bắt dầu phân loại ca.
            if (time >= start1 && time < end1)
            {

                var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Morning, userId = user.Id });
                if (attendace != null && attendace.EndTime != null)
                {
                    throw new Exception("Bạn đã chấm công Ca Sáng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Sáng!");
                }
                else if (attendace != null && attendace.EndTime == null)
                {
                    await _mediator.Send(new UpdateEndtimeForWorkMorningRequest { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                    return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    //chấm công kết ca 1.
                }
                else
                {
                    try
                    {
                        var Attendance = await _mediator.Send(new CreateAttendanceCommand
                        {
                            ApplicationUserId = user.Id,
                            Day = now,
                            StartTime = now,
                            ShiftEnum = ShiftEnum.Morning
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
                await _mediator.Send(new UpdateEndtimeForWorkMorningRequest { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                //chấm công kết ca 1.
            }
            else if (time >= start2 && time < end2)
            {
                var OtRequest = await _context.Get<OvertimeLog>().Where(x => x.IsDeleted == false && x.Date.Date == now.Date && x.ApplicationUserId == user.Id
                   && x.Status == LogStatus.Approved).AsNoTracking().FirstOrDefaultAsync();
                if (OtRequest != null)
                {
                    var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = ShiftEnum.Afternoon, userId = user.Id });
                    if (attendace != null && attendace.EndTime != null)
                    {
                        throw new Exception("Bạn đã chấm công Ca Chiều Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Chiều!");
                    }
                    else if (attendace != null && attendace.EndTime == null)
                    {
                        await _mediator.Send(new UpdateEndtimeForWorkMorningRequest { DayTime = now, Shift = ShiftEnum.Afternoon });
                        return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    }
                    else
                    {
                        try
                        {
                            var Attendance = await _mediator.Send(new CreateAttendanceCommand
                            {
                                ApplicationUserId = user.Id,
                                Day = now,
                                StartTime = now,
                                ShiftEnum = ShiftEnum.Afternoon

                            });
                            return "Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                        }
                    }
                }
                else
                {
                    throw new Exception("Hôm nay ngày " + DateTime.Now.ToString("dd/MM/yyyy") + ", bạn không có lịch Tăng ca nên không thể chấm công!");
                }

            }
            else if (time >= end2 && time <= TimeSpan.Parse("23:30:00"))
            {
                //chấm công kết ca 2
                await _mediator.Send(new UpdateEndtimeForWorkMorningRequest { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Afternoon });
                return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";

            }
            else
            {
                throw new Exception("Hiện đang không trong ca làm việc nên bạn không thể chấm công được!");
            }
        }
        else
        {
            var time = now.TimeOfDay;

            //lấy cấu hình ca làm:
            var listShift = await _mediator.Send(new GetListShiftRequest { });
            TimeSpan start1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().EndTime.Value.TimeOfDay;
            TimeSpan start2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().EndTime.Value.TimeOfDay;

            //bắt dầu phân loại ca.
            if (time >= start1 && time < end1)
            {
                if (leaveLog.LeaveShift == LeaveShift.Morning || leaveLog.LeaveShift == LeaveShift.Full)
                {
                    throw new Exception("Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " bạn có lịch nghỉ ca sáng nên không thể chấm công được!");
                }
                var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = mentor_v1.Domain.Enums.ShiftEnum.Morning, userId = user.Id });
                if (attendace != null && attendace.EndTime != null)
                {
                    throw new Exception("Bạn đã chấm công Ca Sáng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Sáng!");
                }
                else if (attendace != null && attendace.EndTime == null)
                {
                    await _mediator.Send(new UpdateEndtimeForWorkMorningRequest { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                    return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    //chấm công kết ca 1.
                }
                else
                {
                    try
                    {
                        var Attendance = await _mediator.Send(new CreateAttendanceCommand
                        {
                            ApplicationUserId = user.Id,
                            Day = now,
                            StartTime = now,
                            ShiftEnum = ShiftEnum.Morning
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
                await _mediator.Send(new UpdateEndtimeForWorkMorningRequest { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                //chấm công kết ca 1.
            }
            else if (time >= start2 && time < end2)
            {
                if (leaveLog.LeaveShift == LeaveShift.Afternoon || leaveLog.LeaveShift == LeaveShift.Full)
                {
                    throw new Exception("Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " bạn có lịch nghỉ ca chiều nên không thể chấm công được!");
                }
                var OtRequest = await _context.Get<OvertimeLog>().Where(x => x.IsDeleted == false && x.Date.Date == now.Date && x.ApplicationUserId == user.Id
                   && x.Status == LogStatus.Approved).AsNoTracking().FirstOrDefaultAsync();
                if (OtRequest != null)
                {
                    var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = ShiftEnum.Afternoon, userId = user.Id });
                    if (attendace != null && attendace.EndTime != null)
                    {
                        throw new Exception("Bạn đã chấm công Ca Chiều Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Chiều!");
                    }
                    else if (attendace != null && attendace.EndTime == null)
                    {
                        await _mediator.Send(new UpdateEndtimeForWorkMorningRequest { DayTime = now, Shift = ShiftEnum.Afternoon });
                        return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    }
                    else
                    {
                        try
                        {
                            var Attendance = await _mediator.Send(new CreateAttendanceCommand
                            {
                                ApplicationUserId = user.Id,
                                Day = now,
                                StartTime = now,
                                ShiftEnum = ShiftEnum.Afternoon

                            });
                            return "Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                        }
                    }
                }
                else
                {
                    throw new Exception("Hôm nay ngày " + DateTime.Now.ToString("dd/MM/yyyy") + ", bạn không có lịch Tăng ca nên không thể chấm công!");
                }

            }
            else if (time >= end2 && time <= TimeSpan.Parse("23:30:00"))
            {
                //chấm công kết ca 2
                await _mediator.Send(new UpdateEndtimeForWorkMorningRequest { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Afternoon });
                return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";

            }
            else
            {
                throw new Exception("Hiện đang không trong ca làm việc nên bạn không thể chấm công được!");
            }
        }
    }

    public async Task<string> AttendanceAfternoonOnly(DateTime now, ApplicationUser user)
    {
        var leaveLog = await _mediator.Send(new GetLeaveLogByUserIdRequest { UserId = user.Id, day = now });
        if (leaveLog == null)
        {
            var time = now.TimeOfDay;

            //lấy cấu hình ca làm:
            var listShift = await _mediator.Send(new GetListShiftRequest { });
            TimeSpan start1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().EndTime.Value.TimeOfDay;
            TimeSpan start2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().EndTime.Value.TimeOfDay;

            //bắt dầu phân loại ca.
            if (time >= start1 && time < end1)
            {
                var OtRequest = await _context.Get<OvertimeLog>().Where(x => x.IsDeleted == false && x.Date.Date == now.Date && x.ApplicationUserId == user.Id
                  && x.Status == LogStatus.Approved).AsNoTracking().FirstOrDefaultAsync();
                if (OtRequest != null)
                {

                    var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = ShiftEnum.Morning, userId = user.Id });
                    if (attendace != null && attendace.EndTime != null)
                    {
                        throw new Exception("Bạn đã chấm công Ca Sáng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Sáng!");
                    }
                    else if (attendace != null && attendace.EndTime == null)
                    {
                        await _mediator.Send(new UpdateAttendanceForWorkAfternoon { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                        return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    }
                    else
                    {
                        try
                        {
                            var Attendance = await _mediator.Send(new CreateAttendanceCommand
                            {
                                ApplicationUserId = user.Id,
                                Day = now,
                                StartTime = now,
                                ShiftEnum = ShiftEnum.Morning
                            });
                            return "Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                        }
                    }
                }
                else
                {
                    throw new Exception("Hôm nay ngày " + DateTime.Now.ToString("dd/MM/yyyy") + ", bạn không có lịch Tăng ca nên không thể chấm công ca Sáng!");
                }
            }
            else if (time >= end1 && time < start2)
            {
                await _mediator.Send(new UpdateAttendanceForWorkAfternoon { DayTime = now, Shift = ShiftEnum.Morning });
                return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                //chấm công kết ca 1.
            }
            else if (time >= start2 && time < end2)
            {

                var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = ShiftEnum.Afternoon, userId = user.Id });
                if (attendace != null && attendace.EndTime != null)
                {
                    throw new Exception("Bạn đã chấm công Ca Chiều Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Chiều!");
                }
                else if (attendace != null && attendace.EndTime == null)
                {
                    await _mediator.Send(new UpdateAttendanceForWorkAfternoon { DayTime = now, Shift = ShiftEnum.Afternoon });
                    return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                }
                else
                {
                    try
                    {
                        var Attendance = await _mediator.Send(new CreateAttendanceCommand
                        {
                            ApplicationUserId = user.Id,
                            Day = now,
                            StartTime = now,
                            ShiftEnum = ShiftEnum.Afternoon

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

                await _mediator.Send(new UpdateAttendanceForWorkAfternoon { DayTime = now, Shift = ShiftEnum.Afternoon });
                return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";

            }
            else
            {
                throw new Exception("Hiện đang không trong ca làm việc nên bạn không thể chấm công được!");
            }
        }
        else
        {
            var time = now.TimeOfDay;

            //lấy cấu hình ca làm:
            var listShift = await _mediator.Send(new GetListShiftRequest { });
            TimeSpan start1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().EndTime.Value.TimeOfDay;
            TimeSpan start2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().EndTime.Value.TimeOfDay;

            //bắt dầu phân loại ca.
            if (time >= start1 && time < end1)
            {

                if (leaveLog.LeaveShift == LeaveShift.Morning || leaveLog.LeaveShift == LeaveShift.Full)
                {
                    throw new Exception("Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " bạn có lịch nghỉ ca sáng nên không thể chấm công được!");
                }
                var OtRequest = await _context.Get<OvertimeLog>().Where(x => x.IsDeleted == false && x.Date.Date == now.Date && x.ApplicationUserId == user.Id
                  && x.Status == LogStatus.Approved).AsNoTracking().FirstOrDefaultAsync();
                if (OtRequest != null)
                {

                    var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = ShiftEnum.Morning, userId = user.Id });
                    if (attendace != null && attendace.EndTime != null)
                    {
                        throw new Exception("Bạn đã chấm công Ca Sáng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Sáng!");
                    }
                    else if (attendace != null && attendace.EndTime == null)
                    {
                        await _mediator.Send(new UpdateAttendanceForWorkAfternoon { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                        return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    }
                    else
                    {
                        try
                        {
                            var Attendance = await _mediator.Send(new CreateAttendanceCommand
                            {
                                ApplicationUserId = user.Id,
                                Day = now,
                                StartTime = now,
                                ShiftEnum = ShiftEnum.Morning
                            });
                            return "Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                        }
                    }
                }
                else
                {
                    throw new Exception("Hôm nay ngày " + DateTime.Now.ToString("dd/MM/yyyy") + ", bạn không có lịch Tăng ca nên không thể chấm công ca Sáng!");
                }
            }
            else if (time >= end1 && time < start2)
            {
                await _mediator.Send(new UpdateAttendanceForWorkAfternoon { DayTime = now, Shift = ShiftEnum.Morning });
                return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                //chấm công kết ca 1.
            }
            else if (time >= start2 && time < end2)
            {

                if (leaveLog.LeaveShift == LeaveShift.Afternoon || leaveLog.LeaveShift == LeaveShift.Full)
                {
                    throw new Exception("Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " bạn có lịch nghỉ ca chiều nên không thể chấm công được!");
                }
                var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = ShiftEnum.Afternoon, userId = user.Id });
                if (attendace != null && attendace.EndTime != null)
                {
                    throw new Exception("Bạn đã chấm công Ca Chiều Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Chiều!");
                }
                else if (attendace != null && attendace.EndTime == null)
                {
                    await _mediator.Send(new UpdateAttendanceForWorkAfternoon { DayTime = now, Shift = ShiftEnum.Afternoon });
                    return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                }
                else
                {
                    try
                    {
                        var Attendance = await _mediator.Send(new CreateAttendanceCommand
                        {
                            ApplicationUserId = user.Id,
                            Day = now,
                            StartTime = now,
                            ShiftEnum = ShiftEnum.Afternoon

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

                await _mediator.Send(new UpdateAttendanceForWorkAfternoon { DayTime = now, Shift = ShiftEnum.Afternoon });
                return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";

            }
            else
            {
                throw new Exception("Hiện đang không trong ca làm việc nên bạn không thể chấm công được!");
            }
        }

    }

    public async Task<string> AttendanceNotWork(DateTime now, ApplicationUser user)
    {
        var leaveLog = await _mediator.Send(new GetLeaveLogByUserIdRequest { UserId = user.Id, day = now });
        if (leaveLog == null)
        {
            var time = now.TimeOfDay;

            //lấy cấu hình ca làm:
            var listShift = await _mediator.Send(new GetListShiftRequest { });
            TimeSpan start1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().EndTime.Value.TimeOfDay;
            TimeSpan start2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().EndTime.Value.TimeOfDay;

            //bắt dầu phân loại ca.
            if (time >= start1 && time < end1)
            {
                var OtRequest = await _context.Get<OvertimeLog>().Where(x => x.IsDeleted == false && x.Date.Date == now.Date && x.ApplicationUserId == user.Id
                  && x.Status == LogStatus.Approved).AsNoTracking().FirstOrDefaultAsync();
                if (OtRequest != null)
                {

                    var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = ShiftEnum.Morning, userId = user.Id });
                    if (attendace != null && attendace.EndTime != null)
                    {
                        throw new Exception("Bạn đã chấm công Ca Sáng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Sáng!");
                    }
                    else if (attendace != null && attendace.EndTime == null)
                    {
                        await _mediator.Send(new UpdateAttendanceNotWork { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                        return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    }
                    else
                    {
                        try
                        {
                            var Attendance = await _mediator.Send(new CreateAttendanceCommand
                            {
                                ApplicationUserId = user.Id,
                                Day = now,
                                StartTime = now,
                                ShiftEnum = ShiftEnum.Morning
                            });
                            return "Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                        }
                    }
                }
                else
                {
                    throw new Exception("Hôm nay ngày " + DateTime.Now.ToString("dd/MM/yyyy") + ", bạn không có lịch Tăng ca nên không thể chấm công ca Sáng!");
                }
            }
            else if (time >= end1 && time < start2)
            {
                await _mediator.Send(new UpdateAttendanceNotWork { DayTime = now, Shift = ShiftEnum.Morning });
                return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                //chấm công kết ca 1.
            }
            else if (time >= start2 && time < end2)
            {
                var OtRequest = await _context.Get<OvertimeLog>().Where(x => x.IsDeleted == false && x.Date.Date == now.Date && x.ApplicationUserId == user.Id
                 && x.Status == LogStatus.Approved).AsNoTracking().FirstOrDefaultAsync();
                if (OtRequest != null)
                {

                    var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = ShiftEnum.Afternoon, userId = user.Id });
                    if (attendace != null && attendace.EndTime != null)
                    {
                        throw new Exception("Bạn đã chấm công Ca Chiều Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Chiều!");
                    }
                    else if (attendace != null && attendace.EndTime == null)
                    {
                        await _mediator.Send(new UpdateAttendanceNotWork { DayTime = now, Shift = ShiftEnum.Afternoon });
                        return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    }
                    else
                    {
                        try
                        {
                            var Attendance = await _mediator.Send(new CreateAttendanceCommand
                            {
                                ApplicationUserId = user.Id,
                                Day = now,
                                StartTime = now,
                                ShiftEnum = ShiftEnum.Afternoon

                            });
                            return "Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                        }
                    }
                }
                else
                {
                    throw new Exception("Hôm nay ngày " + DateTime.Now.ToString("dd/MM/yyyy") + ", bạn không có lịch Tăng ca nên không thể chấm công ca Chiều!");
                }

            }
            else if (time >= end2 && time <= TimeSpan.Parse("23:30:00"))
            {
                //chấm công kết ca 2

                await _mediator.Send(new UpdateAttendanceNotWork { DayTime = now, Shift = ShiftEnum.Afternoon });
                return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";

            }
            else
            {
                throw new Exception("Hiện đang không trong ca làm việc nên bạn không thể chấm công được!");
            }
        }
        else
        {
            var time = now.TimeOfDay;

            //lấy cấu hình ca làm:
            var listShift = await _mediator.Send(new GetListShiftRequest { });
            TimeSpan start1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Morning).FirstOrDefault().EndTime.Value.TimeOfDay;
            TimeSpan start2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
            TimeSpan end2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == ShiftEnum.Afternoon).FirstOrDefault().EndTime.Value.TimeOfDay;

            //bắt dầu phân loại ca.
            if (time >= start1 && time < end1)
            {
                if (leaveLog.LeaveShift == LeaveShift.Morning || leaveLog.LeaveShift == LeaveShift.Full)
                {
                    throw new Exception("Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " bạn có lịch nghỉ ca sáng nên không thể chấm công được!");
                }
                var OtRequest = await _context.Get<OvertimeLog>().Where(x => x.IsDeleted == false && x.Date.Date == now.Date && x.ApplicationUserId == user.Id
                  && x.Status == LogStatus.Approved).AsNoTracking().FirstOrDefaultAsync();
                if (OtRequest != null)
                {

                    var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = ShiftEnum.Morning, userId = user.Id });
                    if (attendace != null && attendace.EndTime != null)
                    {
                        throw new Exception("Bạn đã chấm công Ca Sáng Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Sáng!");
                    }
                    else if (attendace != null && attendace.EndTime == null)
                    {
                        await _mediator.Send(new UpdateAttendanceNotWork { DayTime = now, Shift = mentor_v1.Domain.Enums.ShiftEnum.Morning });
                        return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    }
                    else
                    {
                        try
                        {
                            var Attendance = await _mediator.Send(new CreateAttendanceCommand
                            {
                                ApplicationUserId = user.Id,
                                Day = now,
                                StartTime = now,
                                ShiftEnum = ShiftEnum.Morning
                            });
                            return "Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Chấm công ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                        }
                    }
                }
                else
                {
                    throw new Exception("Hôm nay ngày " + DateTime.Now.ToString("dd/MM/yyyy") + ", bạn không có lịch Tăng ca nên không thể chấm công ca Sáng!");
                }
            }
            else if (time >= end1 && time < start2)
            {
                await _mediator.Send(new UpdateAttendanceNotWork { DayTime = now, Shift = ShiftEnum.Morning });
                return "Chấm công kết thúc ca Sáng ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                //chấm công kết ca 1.
            }
            else if (time >= start2 && time < end2)
            {
                if (leaveLog.LeaveShift == LeaveShift.Afternoon || leaveLog.LeaveShift == LeaveShift.Full)
                {
                    throw new Exception("Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " bạn có lịch nghỉ ca chiều nên không thể chấm công được!");
                }
                var OtRequest = await _context.Get<OvertimeLog>().Where(x => x.IsDeleted == false && x.Date.Date == now.Date && x.ApplicationUserId == user.Id
                 && x.Status == LogStatus.Approved).AsNoTracking().FirstOrDefaultAsync();
                if (OtRequest != null)
                {

                    var attendace = await _mediator.Send(new GetAttendaceByUserAndShift { Day = now, ShiftEnum = ShiftEnum.Afternoon, userId = user.Id });
                    if (attendace != null && attendace.EndTime != null)
                    {
                        throw new Exception("Bạn đã chấm công Ca Chiều Ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " vì vậy không thể tiếp tục chấm công ca Chiều!");
                    }
                    else if (attendace != null && attendace.EndTime == null)
                    {
                        await _mediator.Send(new UpdateAttendanceNotWork { DayTime = now, Shift = ShiftEnum.Afternoon });
                        return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                    }
                    else
                    {
                        try
                        {
                            var Attendance = await _mediator.Send(new CreateAttendanceCommand
                            {
                                ApplicationUserId = user.Id,
                                Day = now,
                                StartTime = now,
                                ShiftEnum = ShiftEnum.Afternoon

                            });
                            return "Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Chấm công ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thất bại!");
                        }
                    }
                }
                else
                {
                    throw new Exception("Hôm nay ngày " + DateTime.Now.ToString("dd/MM/yyyy") + ", bạn không có lịch Tăng ca nên không thể chấm công ca Chiều!");
                }

            }
            else if (time >= end2 && time <= TimeSpan.Parse("23:30:00"))
            {
                //chấm công kết ca 2

                await _mediator.Send(new UpdateAttendanceNotWork { DayTime = now, Shift = ShiftEnum.Afternoon });
                return "Chấm công kết thúc ca Chiều ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " thành công!";

            }
            else
            {
                throw new Exception("Hiện đang không trong ca làm việc nên bạn không thể chấm công được!");
            }
        }

    }
}
