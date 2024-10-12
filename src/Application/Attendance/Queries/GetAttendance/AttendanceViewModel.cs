using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mentor_v1.Application.Common.Mappings;
using mentor_v1.Domain.Enums;

namespace mentor_v1.Application.Attendance.Queries.GetAttendance;

public class AttendanceViewModel : IMapFrom<Domain.Entities.Attendance>
{

    public Guid Id { get; init; }
    public string ApplicationUserId { get; set; }
    public DateTime Day { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string ShiftEnum { get; set; }
    public double? TimeWork { get; set; }
    public double? OverTime { get; set; }
    public virtual Domain.Identity.ApplicationUser applicationUser { get; set; }

}