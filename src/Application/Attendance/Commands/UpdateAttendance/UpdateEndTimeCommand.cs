using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.Common.Interfaces;
using mentor_v1.Application.ShiftConfig.Queries;
using mentor_v1.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace mentor_v1.Application.Attendance.Commands.UpdateAttendance;
public record UpdateEndTimeCommand : IRequest
{
    public DateTime DayTime { get; set; }
    public ShiftEnum Shift { get; set; }
}
public class UpdateEndTimeCommandHandler : IRequestHandler<UpdateEndTimeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateEndTimeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateEndTimeCommand request, CancellationToken cancellationToken)
    {
        var CurrentAttendance = await _context.Get<Domain.Entities.Attendance>().Where(x =>x.IsDeleted==false && x.Day.Date == request.DayTime.Date && x.ShiftEnum == request.Shift).FirstOrDefaultAsync();

        var listShift = await _context.Get<Domain.Entities.ShiftConfig>().Where(x => x.IsDeleted == false).OrderBy(x => x.StartTime).AsNoTracking().ToListAsync();
        TimeSpan start1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Morning).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
        TimeSpan end1 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Morning).FirstOrDefault().EndTime.Value.TimeOfDay;
        TimeSpan start2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Afternoon).FirstOrDefault().StartTime.Value.AddMinutes(-30).TimeOfDay;
        TimeSpan end2 = listShift.Where(x => x.IsDeleted == false && x.ShiftEnum == mentor_v1.Domain.Enums.ShiftEnum.Afternoon).FirstOrDefault().EndTime.Value.TimeOfDay;

        var time = request.DayTime.TimeOfDay;

        if (CurrentAttendance == null && request.Shift == ShiftEnum.Morning)
        {
            throw new NotFoundException( "Ngày " + request.DayTime.Date.ToString("dd/MM/yyyy") + " , bạn đã không chấm công vào Ca Sáng vì vậy không thể chấm công kết Ca Sáng");
        }
        else if (CurrentAttendance == null && request.Shift == ShiftEnum.Afternoon)
        {
            throw new NotFoundException( "Ngày " + request.DayTime.Date.ToString("dd/MM/yyyy") + " , bạn đã không chấm công vào Ca Chiều vì vậy không thể chấm công kết Ca Chiều");
        }else if(CurrentAttendance != null && CurrentAttendance.EndTime != null && CurrentAttendance.ShiftEnum == ShiftEnum.Morning)
        {
            throw new NotFoundException( "Bạn đã chấm công kết ca Sáng " + request.DayTime.Date.ToString("dd/MM/yyyy") + " , vì vậy không thể chấm công kết Ca Sáng nữa");
        }
        else if (CurrentAttendance != null && CurrentAttendance.EndTime != null && CurrentAttendance.ShiftEnum == ShiftEnum.Afternoon)
        {
            throw new NotFoundException( "Bạn đã chấm công kết ca Chiều " + request.DayTime.Date.ToString("dd/MM/yyyy") + " , vì vậy không thể chấm công kết Ca Chiều nữa");

        }

        bool lated = false;

        if (time > listShift.ElementAt(0).StartTime.Value.TimeOfDay && time < start2)
        {
            
            start1 = listShift.ElementAt(0).StartTime.Value.TimeOfDay;
            var defaultTime = (end1 - start1);
            TimeSpan timework;

            //nếu đi trễ thì tính h từ lúc chấm công vào 
            if (CurrentAttendance.StartTime.TimeOfDay > start1)
            {
                //nếu tg out trc thời gian hết giờ làm 
                if (end1 > time)
                {
                    timework = time- CurrentAttendance.StartTime.TimeOfDay;
                }
                else
                {
                    timework = end1 - CurrentAttendance.StartTime.TimeOfDay;
                }
            }
            else
            {
                //nếu tg out trc thời gian hết giờ làm 
                if (end1 > time)
                {
                    timework = time - start1;

                }
                else
                {
                    timework = end1 - start1;

                }
            }
            double final = timework.TotalSeconds / 3600.0;
            CurrentAttendance.EndTime = request.DayTime;
            CurrentAttendance.TimeWork  = final;
            CurrentAttendance.OverTime = 0;
            //chấm công kết ca 1.
        }
        else if (time > listShift.ElementAt(1).StartTime.Value.TimeOfDay && time <= TimeSpan.Parse("23:30:00"))
        {
            start2 = listShift.ElementAt(1).StartTime.Value.TimeOfDay;
            //chấm công kết ca 2
           
            var OtRequest = await _context.Get<Domain.Entities.OvertimeLog>()
                .Where(x => x.IsDeleted == false && x.Date.Date == request.DayTime.Date
                && x.ApplicationUserId == CurrentAttendance.ApplicationUserId
                && x.Status == LogStatus.Approved).AsNoTracking()
                .FirstOrDefaultAsync();

            var defaultTime = (end2 - start2);
            
            TimeSpan timework;
            TimeSpan tempOT = TimeSpan.Zero;

            if (CurrentAttendance.StartTime.TimeOfDay> start2)
            {
                if(end2> time)
                {
                    timework = time - CurrentAttendance.StartTime.TimeOfDay;
                }
                else
                {
                    timework = end2 - CurrentAttendance.StartTime.TimeOfDay;
                    tempOT = time - end2;
                }
                
            }
            else
            {
                //nếu tg out trc thời gian hết giờ làm 
                if(end2 > time)
                {
                    timework =time - start2;
                }
                else
                {
                    timework = end2 - start2;
                    tempOT = time - end2;
                }
            }
            double final = 0;
            double OtHour = 0;
            if (OtRequest == null) //nếu KO có yêu cầu làm thêm giờ
            {
               final = timework.TotalSeconds / 3600.0;
            }
            else //nếu có yêu cầu làm thêm giờ
            {
               final = timework.TotalSeconds / 3600.0;
               OtHour = tempOT.TotalSeconds / 3600.0;
                if (OtHour > OtRequest.Hours)
                {
                    OtHour = OtRequest.Hours;
                }

            }
            CurrentAttendance.EndTime = request.DayTime;
            CurrentAttendance.TimeWork = final;
            CurrentAttendance.OverTime = OtHour;
        }
        else
        {
            throw new Exception("Hiện đang không trong ca làm việc nên bạn không thể chấm công được!");
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

